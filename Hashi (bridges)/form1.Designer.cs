namespace Hashi__bridges_
{
    partial class form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Header = new System.Windows.Forms.Label();
            this.EasyBtn = new System.Windows.Forms.Button();
            this.MediumBtn = new System.Windows.Forms.Button();
            this.HardBtn = new System.Windows.Forms.Button();
            this.HeavyBtn = new System.Windows.Forms.Button();
            this.InsaneBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Header
            // 
            this.Header.AutoSize = true;
            this.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Header.Location = new System.Drawing.Point(221, 9);
            this.Header.MaximumSize = new System.Drawing.Size(300, 300);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(213, 146);
            this.Header.TabIndex = 0;
            this.Header.Text = "Hashi Game";
            this.Header.Click += new System.EventHandler(this.label1_Click);
            // 
            // EasyBtn
            // 
            this.EasyBtn.Location = new System.Drawing.Point(274, 158);
            this.EasyBtn.Name = "EasyBtn";
            this.EasyBtn.Size = new System.Drawing.Size(101, 47);
            this.EasyBtn.TabIndex = 1;
            this.EasyBtn.Text = "Easy  6 x 9";
            this.EasyBtn.UseVisualStyleBackColor = true;
            this.EasyBtn.Click += new System.EventHandler(this.EasyBtn_Click);
            // 
            // MediumBtn
            // 
            this.MediumBtn.Location = new System.Drawing.Point(274, 211);
            this.MediumBtn.Name = "MediumBtn";
            this.MediumBtn.Size = new System.Drawing.Size(101, 47);
            this.MediumBtn.TabIndex = 2;
            this.MediumBtn.Text = "Medium 8 x 12";
            this.MediumBtn.UseVisualStyleBackColor = true;
            this.MediumBtn.Click += new System.EventHandler(this.MediumBtn_Click);
            // 
            // HardBtn
            // 
            this.HardBtn.Location = new System.Drawing.Point(274, 264);
            this.HardBtn.Name = "HardBtn";
            this.HardBtn.Size = new System.Drawing.Size(101, 47);
            this.HardBtn.TabIndex = 3;
            this.HardBtn.Text = "Hard 10 x 15";
            this.HardBtn.UseVisualStyleBackColor = true;
            this.HardBtn.Click += new System.EventHandler(this.HardBtn_Click);
            // 
            // HeavyBtn
            // 
            this.HeavyBtn.Location = new System.Drawing.Point(274, 317);
            this.HeavyBtn.Name = "HeavyBtn";
            this.HeavyBtn.Size = new System.Drawing.Size(101, 47);
            this.HeavyBtn.TabIndex = 4;
            this.HeavyBtn.Text = "Heavy 13 x 18";
            this.HeavyBtn.UseVisualStyleBackColor = true;
            this.HeavyBtn.Click += new System.EventHandler(this.HeavyBtn_Click);
            // 
            // InsaneBtn
            // 
            this.InsaneBtn.Location = new System.Drawing.Point(274, 370);
            this.InsaneBtn.Name = "InsaneBtn";
            this.InsaneBtn.Size = new System.Drawing.Size(101, 47);
            this.InsaneBtn.TabIndex = 5;
            this.InsaneBtn.Text = "Insane 15 x 22";
            this.InsaneBtn.UseVisualStyleBackColor = true;
            this.InsaneBtn.Click += new System.EventHandler(this.InsaneBtn_Click);
            // 
            // form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(651, 540);
            this.Controls.Add(this.InsaneBtn);
            this.Controls.Add(this.HeavyBtn);
            this.Controls.Add(this.HardBtn);
            this.Controls.Add(this.MediumBtn);
            this.Controls.Add(this.EasyBtn);
            this.Controls.Add(this.Header);
            this.Name = "form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Header;
        private System.Windows.Forms.Button EasyBtn;
        private System.Windows.Forms.Button MediumBtn;
        private System.Windows.Forms.Button HardBtn;
        private System.Windows.Forms.Button HeavyBtn;
        private System.Windows.Forms.Button InsaneBtn;
    }
}

