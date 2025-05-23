﻿namespace ParallelCL_Imaging
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
			this.button_move = new Button();
			this.button_execute = new Button();
			this.button_export = new Button();
			this.numericUpDown_zoom = new NumericUpDown();
			this.button_recenter = new Button();
			this.label_currentKernel = new Label();
			this.textBox_kernelString = new TextBox();
			this.button_kernelCompile = new Button();
			((System.ComponentModel.ISupportInitialize) this.pictureBox_view).BeginInit();
			this.panel_main.SuspendLayout();
			((System.ComponentModel.ISupportInitialize) this.numericUpDown_zoom).BeginInit();
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
			this.listBox_kernels.Location = new Point(310, 545);
			this.listBox_kernels.Name = "listBox_kernels";
			this.listBox_kernels.Size = new Size(163, 259);
			this.listBox_kernels.TabIndex = 5;
			this.listBox_kernels.SelectedIndexChanged += this.listBox_kernels_SelectedIndexChanged;
			// 
			// panel_kernelParams
			// 
			this.panel_kernelParams.Location = new Point(479, 559);
			this.panel_kernelParams.Name = "panel_kernelParams";
			this.panel_kernelParams.Size = new Size(343, 216);
			this.panel_kernelParams.TabIndex = 6;
			// 
			// button_move
			// 
			this.button_move.Location = new Point(229, 651);
			this.button_move.Name = "button_move";
			this.button_move.Size = new Size(75, 23);
			this.button_move.TabIndex = 7;
			this.button_move.Text = "Move";
			this.button_move.UseVisualStyleBackColor = true;
			this.button_move.Click += this.button_move_Click;
			// 
			// button_execute
			// 
			this.button_execute.Location = new Point(479, 781);
			this.button_execute.Name = "button_execute";
			this.button_execute.Size = new Size(75, 23);
			this.button_execute.TabIndex = 8;
			this.button_execute.Text = "Execute";
			this.button_execute.UseVisualStyleBackColor = true;
			this.button_execute.Click += this.button_execute_Click;
			// 
			// button_export
			// 
			this.button_export.Location = new Point(12, 651);
			this.button_export.Name = "button_export";
			this.button_export.Size = new Size(75, 23);
			this.button_export.TabIndex = 9;
			this.button_export.Text = "Export";
			this.button_export.UseVisualStyleBackColor = true;
			this.button_export.Click += this.button_export_Click;
			// 
			// numericUpDown_zoom
			// 
			this.numericUpDown_zoom.Location = new Point(747, 530);
			this.numericUpDown_zoom.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
			this.numericUpDown_zoom.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
			this.numericUpDown_zoom.Name = "numericUpDown_zoom";
			this.numericUpDown_zoom.Size = new Size(75, 23);
			this.numericUpDown_zoom.TabIndex = 10;
			this.numericUpDown_zoom.Value = new decimal(new int[] { 100, 0, 0, 0 });
			this.numericUpDown_zoom.ValueChanged += this.numericUpDown_zoom_ValueChanged;
			// 
			// button_recenter
			// 
			this.button_recenter.Location = new Point(666, 530);
			this.button_recenter.Name = "button_recenter";
			this.button_recenter.Size = new Size(75, 23);
			this.button_recenter.TabIndex = 11;
			this.button_recenter.Text = "Re-center";
			this.button_recenter.UseVisualStyleBackColor = true;
			this.button_recenter.Click += this.button_recenter_Click;
			// 
			// label_currentKernel
			// 
			this.label_currentKernel.AutoSize = true;
			this.label_currentKernel.Location = new Point(310, 527);
			this.label_currentKernel.Name = "label_currentKernel";
			this.label_currentKernel.Size = new Size(79, 15);
			this.label_currentKernel.TabIndex = 12;
			this.label_currentKernel.Text = "Kernel loaded";
			// 
			// textBox_kernelString
			// 
			this.textBox_kernelString.AcceptsReturn = true;
			this.textBox_kernelString.AcceptsTab = true;
			this.textBox_kernelString.Location = new Point(12, 108);
			this.textBox_kernelString.MaxLength = 9999999;
			this.textBox_kernelString.Multiline = true;
			this.textBox_kernelString.Name = "textBox_kernelString";
			this.textBox_kernelString.PlaceholderText = "Kernel string here";
			this.textBox_kernelString.Size = new Size(292, 416);
			this.textBox_kernelString.TabIndex = 13;
			this.textBox_kernelString.KeyPress += this.textBox_kernelString_KeyPress;
			// 
			// button_kernelCompile
			// 
			this.button_kernelCompile.Location = new Point(12, 530);
			this.button_kernelCompile.Name = "button_kernelCompile";
			this.button_kernelCompile.Size = new Size(75, 23);
			this.button_kernelCompile.TabIndex = 14;
			this.button_kernelCompile.Text = "Compile";
			this.button_kernelCompile.UseVisualStyleBackColor = true;
			this.button_kernelCompile.Click += this.button_kernelCompile_Click;
			// 
			// MainView
			// 
			this.AutoScaleDimensions = new SizeF(7F, 15F);
			this.AutoScaleMode = AutoScaleMode.Font;
			this.ClientSize = new Size(834, 961);
			this.Controls.Add(this.button_kernelCompile);
			this.Controls.Add(this.textBox_kernelString);
			this.Controls.Add(this.label_currentKernel);
			this.Controls.Add(this.button_recenter);
			this.Controls.Add(this.numericUpDown_zoom);
			this.Controls.Add(this.button_export);
			this.Controls.Add(this.button_execute);
			this.Controls.Add(this.button_move);
			this.Controls.Add(this.panel_kernelParams);
			this.Controls.Add(this.listBox_kernels);
			this.Controls.Add(this.panel_main);
			this.Controls.Add(this.listBox_images);
			this.Controls.Add(this.comboBox_devices);
			this.Controls.Add(this.listBox_log);
			this.MaximumSize = new Size(850, 1000);
			this.MinimumSize = new Size(850, 1000);
			this.Name = "MainView";
			this.Text = "OpenCL Imaging Kernels";
			((System.ComponentModel.ISupportInitialize) this.pictureBox_view).EndInit();
			this.panel_main.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize) this.numericUpDown_zoom).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private ListBox listBox_log;
		private ComboBox comboBox_devices;
		private ListBox listBox_images;
		private PictureBox pictureBox_view;
		private Panel panel_main;
		private ListBox listBox_kernels;
		private Panel panel_kernelParams;
		private Button button_move;
		private Button button_execute;
		private Button button_export;
		private NumericUpDown numericUpDown_zoom;
		private Button button_recenter;
		private Label label_currentKernel;
		private TextBox textBox_kernelString;
		private Button button_kernelCompile;
	}
}
