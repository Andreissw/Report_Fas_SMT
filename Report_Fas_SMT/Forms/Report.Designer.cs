
namespace Report_Fas_SMT
{
    partial class Report
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.BT_Click = new System.Windows.Forms.Button();
            this.LBStatus = new System.Windows.Forms.Label();
            this.Grid = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.SMT = new System.Windows.Forms.CheckBox();
            this.FAS = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.LBStatusFAS = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // BT_Click
            // 
            this.BT_Click.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.BT_Click.Location = new System.Drawing.Point(12, 113);
            this.BT_Click.Name = "BT_Click";
            this.BT_Click.Size = new System.Drawing.Size(384, 181);
            this.BT_Click.TabIndex = 0;
            this.BT_Click.Text = "Запустить робота";
            this.BT_Click.UseVisualStyleBackColor = true;
            this.BT_Click.Click += new System.EventHandler(this.button1_Click);
            // 
            // LBStatus
            // 
            this.LBStatus.AutoSize = true;
            this.LBStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LBStatus.Location = new System.Drawing.Point(402, 300);
            this.LBStatus.Name = "LBStatus";
            this.LBStatus.Size = new System.Drawing.Size(90, 29);
            this.LBStatus.TabIndex = 1;
            this.LBStatus.Text = "Статус";
            // 
            // Grid
            // 
            this.Grid.AllowUserToAddRows = false;
            this.Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Grid.Location = new System.Drawing.Point(900, 179);
            this.Grid.Name = "Grid";
            this.Grid.Size = new System.Drawing.Size(47, 39);
            this.Grid.TabIndex = 2;
            this.Grid.Visible = false;
            // 
            // timer1
            // 
            this.timer1.Interval = 90;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(12, 300);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(384, 79);
            this.button1.TabIndex = 3;
            this.button1.Text = "Отправить карту SMT сейчас";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // SMT
            // 
            this.SMT.AutoSize = true;
            this.SMT.Checked = true;
            this.SMT.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SMT.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.SMT.Location = new System.Drawing.Point(12, 64);
            this.SMT.Name = "SMT";
            this.SMT.Size = new System.Drawing.Size(235, 43);
            this.SMT.TabIndex = 4;
            this.SMT.Text = "Карта - SMT";
            this.SMT.UseVisualStyleBackColor = true;
            // 
            // FAS
            // 
            this.FAS.AutoSize = true;
            this.FAS.Checked = true;
            this.FAS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.FAS.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FAS.Location = new System.Drawing.Point(18, 15);
            this.FAS.Name = "FAS";
            this.FAS.Size = new System.Drawing.Size(229, 43);
            this.FAS.TabIndex = 5;
            this.FAS.Text = "Карта - FAS";
            this.FAS.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(12, 397);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(384, 80);
            this.button2.TabIndex = 6;
            this.button2.Text = "Отправить карту FAS сейчас";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // LBStatusFAS
            // 
            this.LBStatusFAS.AutoSize = true;
            this.LBStatusFAS.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LBStatusFAS.Location = new System.Drawing.Point(402, 397);
            this.LBStatusFAS.Name = "LBStatusFAS";
            this.LBStatusFAS.Size = new System.Drawing.Size(90, 29);
            this.LBStatusFAS.TabIndex = 7;
            this.LBStatusFAS.Text = "Статус";
            // 
            // Report
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 489);
            this.Controls.Add(this.LBStatusFAS);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.FAS);
            this.Controls.Add(this.SMT);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Grid);
            this.Controls.Add(this.LBStatus);
            this.Controls.Add(this.BT_Click);
            this.Name = "Report";
            this.Text = "Report";
            this.Load += new System.EventHandler(this.Report_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BT_Click;
        private System.Windows.Forms.Label LBStatus;
        private System.Windows.Forms.DataGridView Grid;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox SMT;
        private System.Windows.Forms.CheckBox FAS;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label LBStatusFAS;
    }
}

