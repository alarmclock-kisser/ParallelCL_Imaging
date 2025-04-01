using OpenTK.Compute.OpenCL;

namespace ParallelCL_Imaging
{
	public class OpenClKernelHandling
	{
		// ----- ----- ----- ATTRIBUTES ----- ----- ----- \\
		public OpenClContextHandling ContextH;

		public CLKernel? Kernel = null;
		public string? KernelName = null;
		public string? KernelSource = null;

		public List<string> KernelPaths = [];


		public string KernelsDir => ContextH.Repopath + "\\Resources\\Kernels\\";
		// ----- ----- ----- LAMBDA ----- ----- ----- \\






		// ----- ----- ----- CONSTRUCTOR ----- ----- ----- \\
		public OpenClKernelHandling(OpenClContextHandling contextH)
		{
			this.ContextH = contextH;

		}





		// ----- ----- ----- METHODS ----- ----- ----- \\
		public List<string> GetKernelPaths()
		{
			// Get all files in directory
			string[] files = Directory.GetFiles(this.KernelsDir, "*.cl");
			
			// Return list
			return files.ToList();
		}







	}
}
