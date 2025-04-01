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



		// ----- ----- ----- LAMBDA ----- ----- ----- \\






		// ----- ----- ----- CONSTRUCTOR ----- ----- ----- \\
		public OpenClKernelHandling(OpenClContextHandling contextH)
		{
			this.ContextH = contextH;

		}





		// ----- ----- ----- METHODS ----- ----- ----- \\








	}
}
