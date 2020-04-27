using System.Drawing;

namespace EEG.MitsarSmartBCI
{
    /// <summary>
    /// Элемент управления для визуализации ЭЭГ Нейро-Спектр 4ВП
    /// </summary>
    public partial class MitsarSmartBCIViewControl : EEGViewControl
    {
        private static readonly ViewChannel[] m_ViewChannels = { new ViewChannel("1. FP1", Color.FromArgb(255, 128, 128)),
            new ViewChannel("2. F3", Color.FromArgb(128, 255, 128)), new ViewChannel("3. C3", Color.FromArgb(128, 128, 255)),
            new ViewChannel("4. P3", Color.FromArgb(128, 128, 128)), new ViewChannel("5. O1", Color.FromArgb(255, 255, 128)),
            new ViewChannel("6. F7", Color.FromArgb(255, 128, 255)), new ViewChannel("7. T3", Color.FromArgb(128, 255, 255)),
            new ViewChannel("8. T5", Color.FromArgb(0, 255, 255)), new ViewChannel("9. FZ", Color.FromArgb(255, 0, 255)),
            new ViewChannel("10. PZ", Color.FromArgb(255, 255, 0)), new ViewChannel("11. A1", Color.FromArgb(255, 128, 0)),
            new ViewChannel("12. FP2", Color.FromArgb(255, 0, 128)), new ViewChannel("13. F4", Color.FromArgb(0, 255, 128)),
            new ViewChannel("14. C4", Color.FromArgb(0, 128, 255)), new ViewChannel("15. P4",Color.FromArgb(128, 0, 255)),
            new ViewChannel("16. O2", Color.FromArgb(128, 255, 0)), new ViewChannel("17. F8", Color.FromArgb(0, 255, 0)),
            new ViewChannel("18. T4", Color.FromArgb(255, 0, 0)), new ViewChannel("19. T6", Color.FromArgb(0, 0, 255)),
            new ViewChannel("20. FPZ",Color.FromArgb(0, 0, 128)), new ViewChannel("21. CZ", Color.FromArgb(0, 128, 0)),
            new ViewChannel("22. OZ", Color.FromArgb(128, 0, 0)), new ViewChannel("23. Не используется", Color.FromArgb(128, 192, 0)),
            new ViewChannel("24. Не используется", Color.FromArgb(128, 0, 192)), new ViewChannel("25. Не используется", Color.FromArgb(192, 128, 0)),
            new ViewChannel("26. Не используется", Color.FromArgb(192, 0, 128)), new ViewChannel("27. Не используется", Color.FromArgb(0, 192, 128)),
            new ViewChannel("28. Не используется", Color.FromArgb(0, 128, 192)), new ViewChannel("29. Ref", Color.FromArgb(0, 128, 192))
        };
        //new ViewChannel("24. AD0", Color.FromArgb(192, 128, 0)),
        //new ViewChannel("25. AD1", Color.FromArgb(192, 0, 128)), 
        /// <summary>
        /// Конструктор
        /// </summary>
        public MitsarSmartBCIViewControl()
        {
            InitializeComponent();

            for (int i = 0; i < m_ViewChannels.Length; i++)
                ViewChannels.Add(m_ViewChannels[i]);
        }
    }
}