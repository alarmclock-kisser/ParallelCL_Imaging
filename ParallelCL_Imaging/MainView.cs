using OpenTK.Graphics.ES11;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace ParallelCL_Imaging
{
	public partial class MainView : Form
	{
		// ----- ----- ----- ATTRIBUTES ----- ----- ----- \\
		public string Repopath;

		public ImageHandling ImageH;
		public GuiBuilder GuiB;
		public OpenClContextHandling ContextH;

		public ImageObject? IMG => ImageH.CurrentImage;
		public OpenClMemoryHandling? MemH => ContextH.MemH;
		public OpenClKernelHandling? KernelH => ContextH.KernelH;


		private int oldZoom = 100;
		private Point mouseDownLocation;
		private bool isDragging = false;

		// ----- ----- ----- CONSTRUCTORS ----- ----- ----- \\
		public MainView()
		{
			InitializeComponent();

			// Set repopath
			Repopath = GetRepopath(true);

			// Window position
			this.StartPosition = FormStartPosition.Manual;
			this.Location = new Point(0, 0);

			// Init. objects
			ImageH = new ImageHandling(Repopath, listBox_images);
			GuiB = new GuiBuilder(this);
			ContextH = new OpenClContextHandling(Repopath, listBox_log, comboBox_devices);

			// Register events
			listBox_log.DoubleClick += (s, e) => CopyLogLine();
			this.pictureBox_view.MouseWheel += this.pictureBox_view_MouseWheel;
			this.pictureBox_view.MouseDown += this.pictureBox_view_MouseDown;
			this.pictureBox_view.MouseMove += this.pictureBox_view_MouseMove;
			this.pictureBox_view.MouseUp += this.pictureBox_view_MouseUp;


			// Select device (Intel CPU)
			SelectDeviceLike("Core");

			// Load images resources
			LoadResourcesImages();
		}






		// ----- ----- ----- METHODS ----- ----- ----- \\
		private string GetRepopath(bool root)
		{
			string repo = AppDomain.CurrentDomain.BaseDirectory;

			if (root)
			{
				repo += @"..\..\..\";
			}

			repo = Path.GetFullPath(repo);
			return repo;
		}

		private void SelectDeviceLike(string name)
		{
			// Get devices from combo
			string[] entries = new string[comboBox_devices.Items.Count];
			for (int i = 0; i < comboBox_devices.Items.Count; i++)
			{
				entries[i] = comboBox_devices.Items[i]?.ToString() ?? "";
			}

			// Find device
			int index = Array.FindIndex(entries, x => x.ToLower().Contains(name.ToLower()));

			// Select device
			if (index >= 0 && index < comboBox_devices.Items.Count)
			{
				comboBox_devices.SelectedIndex = index;
			}
			else
			{
				MessageBox.Show("Device not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				comboBox_devices.SelectedIndex = -1;
			}
		}

		private void LoadResourcesImages()
		{
			// Get every file in Resources/Images
			string path = Repopath + @"Resources\Images\";

			// Get all files
			string[] files = Directory.GetFiles(path);

			// Add images
			foreach (string file in files)
			{
				ImageH.AddImage(file);
			}

			// Select first image
			if (ImageH.ImageCount > 0)
			{
				listBox_images.SelectedIndex = 0;
			}
		}

		public void ToggleUI()
		{
			// PictureBox
			this.pictureBox_view.Image = this.ImageH.CurrentImage?.Img;
			this.pictureBox_view.SizeMode = PictureBoxSizeMode.Zoom;

			// Zoom (change size of PictureBox))
			this.oldZoom = (int) this.numericUpDown_zoom.Value;
			this.pictureBox_view.Width = (int) (this.ImageH.CurrentImage?.Img?.Width * this.oldZoom / 100.0 ?? 1);
			this.pictureBox_view.Height = (int) (this.ImageH.CurrentImage?.Img?.Height * this.oldZoom / 100.0 ?? 1);

			// Fill kernels listBox
			KernelH?.FillKernelsListBox(listBox_kernels);

			// Set label current kernel
			label_currentKernel.Text = KernelH?.KernelName ?? "No kernel selected";
			label_currentKernel.ForeColor = KernelH?.KernelName == null ? Color.Black : Color.DarkGreen;

			// Execute button
			button_execute.Enabled = KernelH != null && IMG != null && (IMG.OnHost || IMG.OnDevice);

			// Move button
			button_move.Enabled = IMG != null;
			button_move.Text = IMG?.OnHost ?? false ? "-> Device" : "Host <-";
		}

		private void CopyLogLine()
		{
			// Get selected line
			string line = listBox_log.SelectedItem?.ToString() ?? "";

			// Copy to clipboard
			Clipboard.SetText(line);

			// MsgBox
			MessageBox.Show("Log-line: \n\n" + line, "Copied to Clipboard", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}






		// ----- ----- ----- EVENTS ----- ----- ----- \\
		private void listBox_images_SelectedIndexChanged(object sender, EventArgs e)
		{
			ToggleUI();
		}

		private void button_move_Click(object sender, EventArgs e)
		{
			// Abort if no IMG or no MemH
			if (IMG == null || MemH == null)
			{
				MessageBox.Show("No image or memory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Move image between host and device
			if (IMG.OnHost)
			{
				// Move to device
				var bytes = IMG.GetPixelsAsBytes();

				IMG.Ptr = MemH.PushChunks([bytes]);
				IMG.ClearImage();

				// Log with MemH
				MemH.Log("Image moved to device", (bytes.LongLength / 4).ToString("N0") + " pixels", 1);
			}
			else
			{
				// Move to host
				var bytes = MemH.PullChunks<byte>(IMG.Ptr).FirstOrDefault();

				IMG.SetImageFromBytes(bytes ?? []);
				IMG.Ptr = 0;

				// Log with MemH
				MemH.Log("Image moved to host", ((bytes?.LongLength ?? 4) / 4).ToString("N0") + " pixels", 1);

			}

			// Update UI
			ToggleUI();
		}

		private void listBox_kernels_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Set kernel
			KernelH?.SetKernel(listBox_kernels.SelectedItem?.ToString() ?? "");

			// Get params from kernel + log count
			string[]? parameters = KernelH?.GetKernelParams();
			KernelH?.Log("Kernel parameters", parameters?.Length.ToString() ?? "0", 1);

			// Fill params panel
			GuiB.FillParams(parameters ?? []);

			// Update UI
			ToggleUI();
		}

		private void button_execute_Click(object sender, EventArgs e)
		{
			// Abort if no IMG or no KernelH
			if (IMG == null || KernelH == null || MemH == null)
			{
				MessageBox.Show("No image or kernel or memory", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// Get params from panel + pointer + length
			long ptr = IMG.Ptr;
			object[] parameters = GuiB.GetParams();

			// If no pointer but img -> move to device
			bool pushBack = false;
			if (ptr == 0 && IMG.OnHost)
			{
				pushBack = true;

				// Move to device
				var bytes = IMG.GetPixelsAsBytes();
				IMG.Ptr = MemH.PushChunks([bytes]);
				ptr = IMG.Ptr;
				IMG.ClearImage();

				// Log with MemH
				MemH.Log("Image moved to device", (bytes.LongLength / 4).ToString("N0") + " pixels", 1);
			}

			// Execute kernel
			IMG.Ptr = KernelH.Execute(ptr, parameters);

			// Push back if moved
			if (pushBack)
			{
				// Move to host
				var bytes = MemH.PullChunks<byte>(IMG.Ptr).FirstOrDefault();
				IMG.SetImageFromBytes(bytes ?? []);
				IMG.Ptr = 0;

				// Log with MemH
				MemH.Log("Image moved to host", ((bytes?.LongLength ?? 4) / 4).ToString("N0") + " pixels", 1);
			}

			// Toggle UI
			ToggleUI();
		}

		private void button_export_Click(object sender, EventArgs e)
		{
			// Abort if no img o img on device
			if (IMG == null || IMG.OnDevice)
			{
				MessageBox.Show("No image or image on device", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// SFD at MyPictures for bitmap
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.FileName = IMG.Name;
			sfd.Filter = "Bitmap|*.bmp";
			sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
			sfd.OverwritePrompt = true;

			// Show dialog
			if (sfd.ShowDialog() == DialogResult.OK)
			{
				// Save image
				IMG.Img?.Save(sfd.FileName, ImageFormat.Bmp);
			}
		}

		private void numericUpDown_zoom_ValueChanged(object sender, EventArgs e)
		{
			this.ToggleUI();
		}

		// Dragging & zoom
		private void pictureBox_view_MouseDown(object? sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.isDragging = true;
				this.mouseDownLocation = e.Location;
				this.pictureBox_view.Cursor = Cursors.Hand;
			}
		}

		private void pictureBox_view_MouseMove(object? sender, MouseEventArgs e)
		{
			if (this.isDragging)
			{
				// Neue Position berechnen
				int dx = e.X - this.mouseDownLocation.X;
				int dy = e.Y - this.mouseDownLocation.Y;
				this.pictureBox_view.Left += dx;
				this.pictureBox_view.Top += dy;
			}
		}

		private void pictureBox_view_MouseUp(object? sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.isDragging = false;
				this.pictureBox_view.Cursor = Cursors.Default;
			}
		}

		private void pictureBox_view_MouseWheel(object? sender, MouseEventArgs e)
		{
			int zoomChange = e.Delta > 0 ? 5 : -5;
			int newZoom = Math.Max((int) this.numericUpDown_zoom.Minimum, Math.Min((int) this.numericUpDown_zoom.Maximum, this.oldZoom + zoomChange));

			this.numericUpDown_zoom.Value = newZoom;
		}

		private void button_recenter_Click(object sender, EventArgs e)
		{
			// Center image
			this.pictureBox_view.Left = (this.panel_main.Width - this.pictureBox_view.Width) / 2;
			this.pictureBox_view.Top = (this.panel_main.Height - this.pictureBox_view.Height) / 2;

			// Reset zoom if CTRL pressed
			if (Control.ModifierKeys == Keys.Control)
			{
				this.numericUpDown_zoom.Value = 100;
			}
		}
	}
}
