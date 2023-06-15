namespace Fiber_Optic_System_Designer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            splitContainer1 = new SplitContainer();
            pictureBox2 = new PictureBox();
            aboutus_button = new Panel();
            aboutus_button_label = new Label();
            pictureBox1 = new PictureBox();
            settings_button = new Panel();
            settings_button_label = new Label();
            home_button = new Panel();
            home_button_label = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            aboutus_button.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            settings_button.SuspendLayout();
            home_button.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel1;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Margin = new Padding(0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(pictureBox2);
            splitContainer1.Panel1.Controls.Add(aboutus_button);
            splitContainer1.Panel1.Controls.Add(pictureBox1);
            splitContainer1.Panel1.Controls.Add(settings_button);
            splitContainer1.Panel1.Controls.Add(home_button);
            splitContainer1.Panel1.Paint += splitContainer1_Panel1_Paint_1;
            splitContainer1.Panel1MinSize = 110;
            splitContainer1.Panel2MinSize = 650;
            splitContainer1.Size = new Size(908, 691);
            splitContainer1.SplitterDistance = 144;
            splitContainer1.SplitterWidth = 3;
            splitContainer1.TabIndex = 0;
            // 
            // pictureBox2
            // 
            pictureBox2.Dock = DockStyle.Bottom;
            pictureBox2.Image = Properties.Resources.pngegg;
            pictureBox2.Location = new Point(0, 547);
            pictureBox2.Margin = new Padding(0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(144, 144);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // aboutus_button
            // 
            aboutus_button.Controls.Add(aboutus_button_label);
            aboutus_button.Location = new Point(2, 320);
            aboutus_button.Margin = new Padding(0);
            aboutus_button.Name = "aboutus_button";
            aboutus_button.Size = new Size(140, 49);
            aboutus_button.TabIndex = 0;
            // 
            // aboutus_button_label
            // 
            aboutus_button_label.AutoSize = true;
            aboutus_button_label.Location = new Point(43, 17);
            aboutus_button_label.Margin = new Padding(0);
            aboutus_button_label.Name = "aboutus_button_label";
            aboutus_button_label.Size = new Size(60, 15);
            aboutus_button_label.TabIndex = 0;
            aboutus_button_label.Text = "About me";
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Top;
            pictureBox1.Image = Properties.Resources.lightblue_18;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Margin = new Padding(0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(144, 154);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // settings_button
            // 
            settings_button.Controls.Add(settings_button_label);
            settings_button.Location = new Point(2, 249);
            settings_button.Margin = new Padding(0);
            settings_button.Name = "settings_button";
            settings_button.Size = new Size(140, 49);
            settings_button.TabIndex = 0;
            // 
            // settings_button_label
            // 
            settings_button_label.AutoSize = true;
            settings_button_label.Location = new Point(43, 18);
            settings_button_label.Margin = new Padding(0);
            settings_button_label.Name = "settings_button_label";
            settings_button_label.Size = new Size(49, 15);
            settings_button_label.TabIndex = 0;
            settings_button_label.Text = "Settings";
            // 
            // home_button
            // 
            home_button.Controls.Add(home_button_label);
            home_button.Location = new Point(0, 178);
            home_button.Margin = new Padding(0);
            home_button.Name = "home_button";
            home_button.Size = new Size(140, 49);
            home_button.TabIndex = 0;
            // 
            // home_button_label
            // 
            home_button_label.AutoSize = true;
            home_button_label.Location = new Point(25, 17);
            home_button_label.Margin = new Padding(0);
            home_button_label.Name = "home_button_label";
            home_button_label.Size = new Size(94, 15);
            home_button_label.TabIndex = 0;
            home_button_label.Text = "System Designer";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(908, 691);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(900, 700);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Fiber Optic System Design";
            Load += Form1_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            aboutus_button.ResumeLayout(false);
            aboutus_button.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            settings_button.ResumeLayout(false);
            settings_button.PerformLayout();
            home_button.ResumeLayout(false);
            home_button.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Panel home_button;
        private Label home_button_label;
        private Panel settings_button;
        private Label settings_button_label;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Panel aboutus_button;
        private Label aboutus_button_label;
    }
}