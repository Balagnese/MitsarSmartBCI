﻿using System;
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
    public partial class MitsarSmartBCIBataryControl : UserControl
    {
        public MitsarSmartBCIBataryControl()
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
                    m_EEG.BatteryChanged -= EEG_Battery;
                }

                m_EEG = value;
                
                m_EEG.Started += EEG_Started;
                m_EEG.Stopped += EEG_Stopped;
                m_EEG.BatteryChanged += EEG_Battery;
            }
        }

        private void EEG_Started(object sender, EventArgs e)
        {
        }

        private void EEG_Stopped(object sender, EventArgs e)
        {
        }
        private void EEG_Battery(object sender, EventArgs e)
        {
            Dispatcher.Invoke(this, () =>
            {
                int result = m_EEG.Battery;
                batteryLabel.Text = "Заряд батареи: " + result.ToString() + "%";
                if (result < 10)
                    batteryLabel.ForeColor = Color.Red;
            });

        }
    }
}

