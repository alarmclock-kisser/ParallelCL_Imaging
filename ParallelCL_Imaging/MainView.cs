using OpenTK.Graphics.ES11;

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
			// PictureBox image
			pictureBox_view.Image = ImageH.CurrentImage?.Img;

			// Fill kernels listBox
			KernelH?.FillKernelsListBox(listBox_kernels);
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
		}
	}
}
