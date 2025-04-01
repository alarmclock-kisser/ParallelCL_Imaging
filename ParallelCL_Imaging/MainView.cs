namespace ParallelCL_Imaging
{
	public partial class MainView : Form
	{
		// ----- ----- ----- ATTRIBUTES ----- ----- ----- \\
		public string Repopath;

		public ImageHandling ImageH;
		public OpenClContextHandling ContextH;


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
			ContextH = new OpenClContextHandling(Repopath, listBox_log, comboBox_devices);

			// Register events


			// Select device (Intel)
			SelectDeviceLike("Intel");

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
		}




		// ----- ----- ----- EVENTS ----- ----- ----- \\
		private void listBox_images_SelectedIndexChanged(object sender, EventArgs e)
		{
			ToggleUI();
		}




	}
}
