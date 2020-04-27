using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Mitsar.Hardware;
using Mitsar.Essentials;
using System.Windows.Forms;
using System.Threading.Tasks;


namespace EEG.MitsarSmartBCI
{
    
    /// <summary>
    /// Input thread support
    /// </summary>
    class ThreadSupport
    {
        public const uint _AttrMask = 0xFC000000;
        public const uint _UIDMask = 0x03FF0000;
        public const uint _RMask = 0x00007FFF;
        /// <summary>
        /// Target input device
        /// </summary>
        DeviceInput device = null;

        /// <summary>
        /// Функция, используемая для преобразования сигнала перед его отправкой подписчикам на ReseiveData
        /// </summary>
        public Action<int[,]> ThreadSupportTransformData { get; set; }

        /// <summary>
        /// Received portion of data
        /// </summary>
        float[] inputdata = null;

        /// <summary>
        /// Number of channels
        /// </summary>
        int ChannelCount;

        /// <summary>
        /// Max number of ticks
        /// </summary>
        int TickCount;

        /// <summary>
        /// Thread stop flag 
        /// </summary>
        private volatile bool stop = false;
       
        public void Start()
        {
            var task = new Task(() =>
            {
                Run();
            });
            task.Start();
        }

        public void Stop()
        {
            //Signal thread to terminate
            //Can be replaced with ResetEvent
            stop = true;
        }
        
        volatile public bool Terminated = false;

        public void Wait()
        {
            //Waiting until thread terminated
            //Can be replaced with ResetEvent
            while (Terminated == false)
            {
                //Console.WriteLine("________________________Terminated " + Terminated);
                Thread.Sleep(1);
                Application.DoEvents();
                //Console.WriteLine("________________________Terminated " + Terminated);
            }
            
        }

        int[,] data = null;
        public ThreadSupport(DeviceInput device, int ChannelCount, int TickCount)
        {
            this.device = device;
            this.ChannelCount = ChannelCount;
            this.TickCount = TickCount;
            inputdata = new float[ChannelCount * TickCount];
        }

        /// <summary> 
        /// событие получения данных с устройства (index1 - номер канала, index2 - отсчет сигнала) 
        /// </summary> 
        public event EventHandler<EEGReseiveDataEventArgs> ThreadSupportReseiveData;

        /// <summary>
        /// событие получения строки с названием канала и его индексом
        /// </summary>
        public event EventHandler<string> ThreadSupportEmitterData;

        /// <summary>
        /// событие получения данных с акселерометра
        /// </summary>
        public event EventHandler<int[]> ThreadSupportAccelerometerData;

        /// <summary>
        /// нужна для того, чтобы в muliple box норм отоброжалось
        /// </summary>
        string newLine = Environment.NewLine;

        Dictionary<UID, int> ad0Dictionary = new Dictionary<UID, int>();
        public event EventHandler<Dictionary<UID, int>> ThreadSupportAD0Data;


        private void Run()
        {
            Terminated = false;
            while (stop == false)
            {
                string tmp = "";
                bool Result = device.Scan(); //Returns true if buffer contains valid number of tics
                if (device.HALError.Code != HALErrorCode.OK) break; //Error encountered
                if (Result == true)
                {
                    Resource_MultiChannel Resource = device.ActiveDeviceDescription.Resource as Resource_MultiChannel;

                    //Receiveing Data
                    if (device.Grab() == true)
                    {//
                        if (device.HALError.CoreCode == HALCoreErrorCode.TickMiss)
                        {
                            device.HALError.Assign(device.ActiveDeviceDescription, HALErrorCode.OK);// Code = HALErrorCode.OK;
                            device.HALError.AssignCoreCode(HALCoreErrorCode.OK);//TickMiss
                        }
                        //Remember number of ticks in input buffer, those was read from FIFO to DataOut
                        int TicksReceived = Resource.DataBuffer.TicksReceived;
                        int ChannelCount = Resource.InputHardwareChannelCountExternal;
                        int time_on_bus = 0;
                        data = new int[29, TicksReceived];
                        //process each channel in input, linked with current datasource
                        foreach (EmitterDescription em in Resource.EmitterFactory)
                        {
                            //Getting index of emitter channel in Data Out buffer
                            int HardwareIndex = em.HardwareIndex;
                            //Console.WriteLine(em.UID.ToString() + ' ' + em.HardwareIndex);
                            tmp += em.UID.ToString() + ' ' + em.HardwareIndex + newLine;
                            ThreadSupportEmitterData?.Invoke(this, tmp);
                            tmp = "";
                            if (HardwareIndex < 0) continue;
                            for (long i = 0; i < TicksReceived; i++)
                            {
                                //Сырые данные полученные с прибора в единицах АЦП.
                                int value = Resource.DataBuffer.DataOut[i * ChannelCount + HardwareIndex];

                                if (em.UID.ToString() == "AD0")
                                {
                                    if (value != 0)
                                    {
                                        ImpedanceElement el = new ImpedanceElement(value);
                                        ad0Dictionary[el.LogicalUID] = (int) el.ResultValue;
                                        
                                        //Console.WriteLine(value.ToString("X8") + " " + el.ToString());
                                    }

                                }
                                else if (em.UID.ToString() == "AD1")
                                {
                                    if (value != 0)
                                    {
                                        /* UID.AD1 - Канал акселерометра.Значения датчиков положения тела. byte XAxis; byte YAxis; 
                                        byte ZAxis;byte Reserved;  */
                                        byte[] ad1bytes = BitConverter.GetBytes(value);
                                        int[] aclrmtr_arr = new int[] { ad1bytes[0], ad1bytes[1], ad1bytes[2] };
                                        //Console.WriteLine(em.UID.ToString() + " x " + x + " y " + y + " z " + z);
                                        ThreadSupportAccelerometerData?.Invoke(this, aclrmtr_arr);
                                    }
                                }
                                else
                                {
                                    //Преобразуем значение в единицах АЦП в значение в микровольтах умножая на коэффициент квантования.
                                    float fValue = value * em.UnitsPerSample;
                                    
                                    int fValueInt = (int)fValue;
                                    //if (fValue <0 )
                                    //    Console.WriteLine(em.UID.ToString() + ' ' + fValueInt);
                                    //Преобразовываем в думерный массив
                                    //data[em.HardwareIndex, i] = fValueInt;
                                    switch (em.UID) {
                                        case UID.FP1:
                                            data[0, i] = fValueInt;
                                            break;
                                        case UID.F3:
                                            data[1, i] = fValueInt;
                                            break;
                                        case UID.C3:
                                            data[2, i] = fValueInt;
                                            break;
                                        case UID.P3:
                                            data[3, i] = fValueInt;
                                            break;
                                        case UID.O1:
                                            data[4, i] = fValueInt;
                                            break;
                                        case UID.F7:
                                            data[5, i] = fValueInt;
                                            break;
                                        case UID.T3:
                                            data[6, i] = fValueInt;
                                            break;
                                        case UID.T5:
                                            data[7, i] = fValueInt;
                                            break;
                                        case UID.FZ:
                                            data[8, i] = fValueInt;
                                            break;
                                        case UID.PZ:
                                            data[9, i] = fValueInt;
                                            break;
                                        case UID.A1:
                                            data[10, i] = fValueInt;
                                            break;
                                        case UID.FP2:
                                            data[11, i] = fValueInt;
                                            break;
                                        case UID.F4:
                                            data[12, i] = fValueInt;
                                            break;
                                        case UID.C4:
                                            data[13, i] = fValueInt;
                                            break;
                                        case UID.P4:
                                            data[14, i] = fValueInt;
                                            break;
                                        case UID.O2:
                                            data[15, i] = fValueInt;
                                            break;
                                        case UID.F8:
                                            data[16, i] = fValueInt;
                                            break;
                                        case UID.T4:
                                            data[17, i] = fValueInt;
                                            break;
                                        case UID.T6:
                                            data[18, i] = fValueInt;
                                            break;
                                        case UID.COMPLEX1:
                                            data[19, i] = fValueInt;
                                            break;
                                        case UID.CZ:
                                            data[20, i] = fValueInt;
                                            break;
                                        case UID.COMPLEX2:
                                            data[21, i] = fValueInt;
                                            break;
                                        case UID.BIO1:
                                            data[22, i] = fValueInt;
                                            data[23, i] = fValueInt;
                                            data[24, i] = fValueInt;
                                            data[25, i] = fValueInt;
                                            data[26, i] = fValueInt;
                                            data[27, i] = fValueInt;
                                            data[28, i] = fValueInt;
                                            break;
                                    }
                                }
                            }
                            time_on_bus += TicksReceived;
                        }
                        ThreadSupportAD0Data(this, ad0Dictionary);
                        ThreadSupportTransformData?.Invoke(data);
                        ThreadSupportReseiveData?.Invoke(this, new EEGReseiveDataEventArgs((int[,])data.Clone(), time_on_bus));
                    }
                }
                else
                    Thread.Sleep(1);
            }
            Terminated = true;
        }
        

    }

}


