using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEG.MitsarSmartBCI
{
    public partial class MitsarSmartBCIAccelerometerControl : UserControl
    {
        public MitsarSmartBCIAccelerometerControl()
        {
            InitializeComponent();
        }
        MitsarSmartBCI m_EEG;
        /// <summary>
        /// Устройство ЭЭГ
        /// </summary>
        [Browsable(false), ReadOnly(true)]
        public MitsarSmartBCI EEG
        {
            get
            {
                return m_EEG;
            }
            set
            {
                if (EEG == value)
                    return;

                if (value == null)
                    throw new ArgumentNullException();

                if ((EEG != null && EEG.IsRunning) || value.IsRunning)
                    throw new Exception("Нельзя задать генератор ЭЭГ когда текущий или новый работает");

                if (m_EEG != null)
                {
                    m_EEG.Started -= EEG_Started;
                    m_EEG.Stopped -= EEG_Stopped;
                    m_EEG.AccelerometerData -= EEG_Accelerometer;
                }

                m_EEG = value;

                m_EEG.Started += EEG_Started;
                m_EEG.Stopped += EEG_Stopped;
                m_EEG.AccelerometerData += EEG_Accelerometer;
            }
        }

        private void EEG_Started(object sender, EventArgs e)
        {
        }

        private void EEG_Stopped(object sender, EventArgs e)
        {
            accelerometerdgv.Rows.Clear();
        }
        private void EEG_Accelerometer(object sender, int[] e)
        {
            Dispatcher.Invoke(this, () =>
            {
                accelerometerdgv.Rows.Clear();
                DataGridViewRow row = new DataGridViewRow();
                row.Cells.Add(new DataGridViewTextBoxCell { Value = e[0] });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = e[1] });
                row.Cells.Add(new DataGridViewTextBoxCell { Value = e[2] });
                accelerometerdgv.Rows.Add(row);
            });

        }
    }
}

