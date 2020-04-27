namespace EEG.MitsarSmartBCI
{
    partial class MitsarSmartBCIAccelerometerControl
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.accelerometerdgv = new System.Windows.Forms.DataGridView();
            this.x = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.y = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.z = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.accelerometerdgv)).BeginInit();
            this.SuspendLayout();
            // 
            // accelerometerdgv
            // 
            this.accelerometerdgv.AllowUserToAddRows = false;
            this.accelerometerdgv.AllowUserToDeleteRows = false;
            this.accelerometerdgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.accelerometerdgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.x,
            this.y,
            this.z});
            this.accelerometerdgv.Location = new System.Drawing.Point(7, 20);
            this.accelerometerdgv.Name = "accelerometerdgv";
            this.accelerometerdgv.ReadOnly = true;
            this.accelerometerdgv.RowHeadersVisible = false;
            this.accelerometerdgv.RowTemplate.Height = 24;
            this.accelerometerdgv.Size = new System.Drawing.Size(164, 55);
            this.accelerometerdgv.TabIndex = 0;
            // 
            // x
            // 
            this.x.HeaderText = "x";
            this.x.Name = "x";
            this.x.ReadOnly = true;
            this.x.Width = 40;
            // 
            // y
            // 
            this.y.HeaderText = "y";
            this.y.Name = "y";
            this.y.ReadOnly = true;
            this.y.Width = 40;
            // 
            // z
            // 
            this.z.HeaderText = "z";
            this.z.Name = "z";
            this.z.ReadOnly = true;
            this.z.Width = 40;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Акселерометр";
            // 
            // MitsarSmartBCIAccelerometerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.accelerometerdgv);
            this.Name = "MitsarSmartBCIAccelerometerControl";
            this.Size = new System.Drawing.Size(177, 83);
            ((System.ComponentModel.ISupportInitialize)(this.accelerometerdgv)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView accelerometerdgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn x;
        private System.Windows.Forms.DataGridViewTextBoxColumn y;
        private System.Windows.Forms.DataGridViewTextBoxColumn z;
        private System.Windows.Forms.Label label1;
    }
}
