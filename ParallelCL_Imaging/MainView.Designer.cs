namespace ParallelCL_Imaging
{
    partial class MainView
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
			this.listBox_log = new ListBox();
			this.comboBox_devices = new ComboBox();
			this.listBox_images = new ListBox();
			this.pictureBox_view = new PictureBox();
			this.panel_main = new Panel();
			this.listBox_kernels = new ListBox();
			this.panel_kernelParams = new Panel();
			((System.ComponentModel.ISupportInitialize) this.pictureBox_view).BeginInit();
			this.panel_main.SuspendLayout();
			this.SuspendLayout();
			// 
			// listBox_log
			// 
			this.listBox_log.FormattingEnabled = true;
			this.listBox_log.ItemHeight = 15;
			this.listBox_log.Location = new Point(12, 810);
			this.listBox_log.Name = "listBox_log";
			this.listBox_log.Size = new Size(810, 139);
			this.listBox_log.TabIndex = 0;
			// 
			// comboBox_devices
			// 
			this.comboBox_devices.FormattingEnabled = true;
			this.comboBox_devices.Location = new Point(12, 12);
			this.comboBox_devices.Name = "comboBox_devices";
			this.comboBox_devices.Size = new Size(292, 23);
			this.comboBox_devices.TabIndex = 1;
			// 
			// listBox_images
			// 
			this.listBox_images.FormattingEnabled = true;
			this.listBox_images.ItemHeight = 15;
			this.listBox_images.Location = new Point(12, 680);
			this.listBox_images.Name = "listBox_images";
			this.listBox_images.Size = new Size(292, 124);
			this.listBox_images.TabIndex = 2;
			this.listBox_images.SelectedIndexChanged += this.listBox_images_SelectedIndexChanged;
			// 
			// pictureBox_view
			// 
			this.pictureBox_view.Location = new Point(0, 0);
			this.pictureBox_view.Name = "pictureBox_view";
			this.pictureBox_view.Size = new Size(512, 512);
			this.pictureBox_view.TabIndex = 3;
			this.pictureBox_view.TabStop = false;
			// 
			// panel_main
			// 
			this.panel_main.Controls.Add(this.pictureBox_view);
			this.panel_main.Location = new Point(310, 12);
			this.panel_main.Name = "panel_main";
			this.panel_main.Size = new Size(512, 512);
			this.panel_main.TabIndex = 4;
			// 
			// listBox_kernels
			// 
			this.listBox_kernels.FormattingEnabled = true;
			this.listBox_kernels.ItemHeight = 15;
			this.listBox_kernels.Location = new Point(310, 530);
			this.listBox_kernels.Name = "listBox_kernels";
			this.listBox_kernels.Size = new Size(120, 274);
			this.listBox_kernels.TabIndex = 5;
			// 
			// panel_kernelParams
			// 
			this.panel_kernelParams.Location = new Point(436, 530);
			this.panel_kernelParams.Name = "panel_kernelParams";
			this.panel_kernelParams.Size = new Size(386, 274);
			this.panel_kernelParams.TabIndex = 6;
			// 
			// MainView
			// 
			this.AutoScaleDimensions = new SizeF(7F, 15F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(834, 961);
			this.Controls.Add(this.panel_kernelParams);
			this.Controls.Add(this.listBox_kernels);
			this.Controls.Add(this.panel_main);
			this.Controls.Add(this.listBox_images);
			this.Controls.Add(this.comboBox_devices);
			this.Controls.Add(this.listBox_log);
			this.MaximumSize = new Size(850, 1000);
			this.MinimumSize = new Size(850, 1000);
			this.Name = "MainView";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize) this.pictureBox_view).EndInit();
			this.panel_main.ResumeLayout(false);
			this.ResumeLayout(false);
		}

		#endregion

		private ListBox listBox_log;
		private ComboBox comboBox_devices;
		private ListBox listBox_images;
		private PictureBox pictureBox_view;
		private Panel panel_main;
		private ListBox listBox_kernels;
		private Panel panel_kernelParams;
	}
}
