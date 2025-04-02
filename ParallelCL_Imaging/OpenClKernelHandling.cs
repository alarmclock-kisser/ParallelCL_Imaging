using OpenTK.Compute.OpenCL;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace ParallelCL_Imaging
{
	public class OpenClKernelHandling
	{
		// ----- ----- ----- ATTRIBUTES ----- ----- ----- \\
		private ListBox LogBox => this.ContextH.LogBox;

		public OpenClContextHandling ContextH;
		public OpenClMemoryHandling? MemH => this.ContextH.MemH;
		private CLContext? Ctx => this.ContextH.Ctx;
		private CLDevice? Dev => this.ContextH.Dev;


		public CLKernel? Kernel = null;
		public string? KernelName = null;
		public string? KernelSource = null;

		public List<string> KernelPaths => this.GetKernelPaths();


		public string KernelsDir => ContextH.Repopath + "\\Resources\\Kernels\\";
		// ----- ----- ----- LAMBDA ----- ----- ----- \\






		// ----- ----- ----- CONSTRUCTOR ----- ----- ----- \\
		public OpenClKernelHandling(OpenClContextHandling contextH)
		{
			this.ContextH = contextH;

		}





		// ----- ----- ----- METHODS ----- ----- ----- \\
		public void Log(string message, string inner = "", int layer = 1, bool update = false)
		{
			string msg = "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "] ";
			msg += "<Kernel>";
			for (int i = 0; i <= layer; i++)
			{
				msg += " - ";
			}
			msg += message;
			if (inner != "")
			{
				msg += "  (" + inner + ")";
			}
			if (update)
			{
				this.LogBox.Items[^1] = msg;
			}
			else
			{
				this.LogBox.Items.Add(msg);
				this.LogBox.SelectedIndex = this.LogBox.Items.Count - 1;
			}
		}

		public List<string> GetKernelPaths()
		{
			// Get all files in directory
			string[] files = Directory.GetFiles(this.KernelsDir, "*.cl");

			// Return list
			return files.ToList();
		}

		public void Dispose()
		{
			// Release kernel
		}

		public string? ReadKernelFile(string filePath)
		{
			// If file does not exist: Try find path in Repopath\Kernels
			if (!File.Exists(filePath))
			{
				filePath = Path.Combine(this.ContextH.Repopath, "Kernels", Path.GetFileName(filePath));
				if (!File.Exists(filePath))
				{
					this.Log("Error reading kernel file", "File not found: " + filePath);
					return null;
				}
			}

			// Try read file
			string? kernelString = null;
			try
			{
				kernelString = File.ReadAllText(filePath);
			}
			catch (Exception e)
			{
				this.Log("Error reading kernel file", e.Message);
			}

			// Return
			return kernelString;
		}

		public string? PrecompileKernelString(string? kernelString)
		{
			// Abort if kernelString null
			if (kernelString == null)
			{
				this.Log("Error precompiling kernel string", "Kernel string is null");
				return null;
			}

			// Read file if path
			if (File.Exists(kernelString))
			{
				kernelString = this.ReadKernelFile(kernelString) ?? "";
			}

			string? kernelName = null;

			// Check if contains "__kernel " and " void "
			if (!kernelString.Contains("__kernel ") || !kernelString.Contains(" void "))
			{
				this.Log("Error precompiling kernel string", "Kernel string does not contain '__kernel ' and ' void '");
				return null;
			}

			// Check every bracked paired ()
			int openCount = kernelString.Count(c => c == '(');
			int closeCount = kernelString.Count(c => c == ')');
			foreach (char c in kernelString)
				if (closeCount != openCount || closeCount == 0)
				{
					this.Log("Error precompiling kernel string: ()-brackets not paired or 0", openCount + " open, " + closeCount + " close");
					return null;
				}

			// Check every bracked paired []
			openCount = kernelString.Count(c => c == '[');
			closeCount = kernelString.Count(c => c == ']');
			if (closeCount != openCount || closeCount == 0)
			{
				this.Log("Error precompiling kernel string: []-brackets not paired or 0", openCount + " open, " + closeCount + " close");
				return null;
			}


			// Check brackets paired {}
			openCount = kernelString.Count(c => c == '{');
			closeCount = kernelString.Count(c => c == '}');
			if (closeCount != openCount || closeCount == 0)
			{
				this.Log("Error precompiling kernel string: {}-brackets not paired or 0", openCount + " open, " + closeCount + " close");
				return null;
			}

			// Check if contains " int " (mandatory for input array length)
			if (!kernelString.Contains(" int "))
			{
				this.Log("Error precompiling kernel string", "Kernel string does not contain ' int '");
				return null;
			}

			// Get kernel name (start after " void ", end before "(")
			int start = kernelString.IndexOf(" void ") + 6;
			int end = kernelString.IndexOf("(", start);
			kernelName = kernelString.Substring(start, end - start).Trim();
			if (string.IsNullOrEmpty(kernelName))
			{
				this.Log("Error precompiling kernel string", "Kernel name not found");
				return null;
			}

			// Return
			return kernelName;
		}

		public CLKernel? CompileKernel(string? kernelString)
		{
			string? kernelName;

			// Read file if path
			if (File.Exists(kernelString) && Path.GetExtension(kernelString) == ".cl")
			{
				kernelString = this.ReadKernelFile(kernelString);

				// Warn kernelString was file path
				this.Log("Kernel string was a file path", Path.GetFileName(kernelString ?? ""));
			}

			// Abort if kernelString null
			if (kernelString == null || kernelString == "" || kernelString.Length == 0)
			{
				this.Log("Error reading kernel file", "KernelString was null or empty");
				return null;
			}

			// Abort if no Ctx
			if (this.Ctx == null || this.Dev == null)
			{
				this.Log("No context to compile kernel");
				return null;
			}

			// Precompile -> Get kernel name
			kernelName = this.PrecompileKernelString(kernelString);
			if (kernelName == null)
			{
				this.Log("Error precompiling kernel string");
				return null;
			}

			// Try compile kernel
			CLKernel? kernel = null;

			// Get program
			CLProgram? program = CL.CreateProgramWithSource(this.Ctx.Value, kernelString, out CLResultCode err);
			if (err != CLResultCode.Success || program == null)
			{
				this.Log("Error creating program (1)", err.ToString());
				return null;
			}

			// Create callback
			CL.ClEventCallback callback = (ev, evStatus) =>
			{
				this.Log("Callback", "Event status: " + evStatus.ToString());
			};

			// Build program
			err = CL.BuildProgram(program.Value, [this.Dev.Value], "", callback);
			if (err != CLResultCode.Success)
			{
				this.Log("Error building program (2)", err.ToString());
				return null;
			}

			// Build info options (CLOptions)
			err = CL.GetProgramBuildInfo(program.Value, this.Dev.Value, ProgramBuildInfo.Options, out byte[]? buildOptions);
			if (err != CLResultCode.Success || buildOptions == null)
			{
				this.Log("Error getting program build info (3.1)", err.ToString());
				return null;
			}
			if (buildOptions.Length > 0)
			{
				this.Log("Program build options", Encoding.UTF8.GetString(buildOptions).Trim('\0'));
			}
			else
			{
				this.Log("No build options available");
			}

			// Build info status
			err = CL.GetProgramBuildInfo(program.Value, this.Dev.Value, ProgramBuildInfo.Status, out byte[]? buildStatus);
			if (err != CLResultCode.Success || buildStatus == null)
			{
				this.Log("Error getting program build info (3.2)", err.ToString());
				return null;
			}
			if (buildStatus.Length > 0)
			{
				this.Log("Program build status", BitConverter.ToInt32(buildStatus, 0).ToString().Trim('\0'));
			}
			else
			{
				this.Log("No build status available");
			}

			// Build info log
			err = CL.GetProgramBuildInfo(program.Value, this.Dev.Value, ProgramBuildInfo.Log, out byte[]? log);
			if (err != CLResultCode.Success || log == null)
			{
				this.Log("Error getting program build info (3.3)", err.ToString());
				return null;
			}
			if (log.Length > 0)
			{
				this.Log("Program build log", Encoding.UTF8.GetString(log).Trim('\0'));
			}
			else
			{
				this.Log("No build log available");
			}

			// Build kernel
			kernel = CL.CreateKernel(program.Value, kernelName, out err);
			if (err != CLResultCode.Success || kernel == null)
			{
				this.Log("Error creating kernel (4)", err.ToString());
				return null;
			}

			// Return
			return kernel;
		}

		public void SetKernel(string kernelName)
		{
			// Set kernel name
			this.KernelName = kernelName;

			// Load kernel source
			this.KernelSource = File.ReadAllText(this.KernelsDir + kernelName + ".cl");

			// Create kernel
			this.Kernel = CompileKernel(this.KernelSource);
		}

		public void FillKernelsListBox(ListBox listBox)
		{
			// Clear
			listBox.Items.Clear();

			// Get all files in directory
			string[] files = Directory.GetFiles(this.KernelsDir, "*.cl");

			// Add to listbox
			foreach (string file in files)
			{
				listBox.Items.Add(Path.GetFileNameWithoutExtension(file));
			}
		}

		public string[] GetKernelParams(string? kernelString = null)
		{
			kernelString ??= this.KernelSource;
			if (kernelString == null)
			{
				this.Log("Error getting kernel params", "Kernel string is null");
				return [];
			}

			// Split into lines
			string[] lines = kernelString.Split(Environment.NewLine);

			// Get params line between "(" and ")" and contains __global
			string? paramsLine = lines.FirstOrDefault(x => x.Contains("__global") && x.Contains("(") && x.Contains(")"));
			if (paramsLine == null)
			{
				this.Log("Error getting kernel params", "Params line not found");
				return [];
			}

			// Get params between "(" and ")", split and trim each
			string[] parameters = paramsLine.Substring(paramsLine.IndexOf("(") + 1, paramsLine.IndexOf(")") - paramsLine.IndexOf("(") - 1).Split(",").Select(p => p.Trim()).ToArray();

			// Return
			return parameters;
		}

		public long ExecuteKernel(long indexPointer, object[] args)
		{
			// Check if Kernel and Memory Handler exist with Que
			if (this.Kernel == null || this.MemH == null || this.MemH.Que == null || indexPointer == 0)
			{
				this.Log("Error executing kernel", "Kernel or MemH or Pointer is null");
				return 0;
			}

			// Get Kernel parameter order
			string[] parameters = this.GetKernelParams();

			// Get Buffers and their lengths
			CLBuffer[] buffers = this.MemH.FindBuffers(indexPointer);
			long[] lengths = this.MemH.FindLengths(indexPointer).Select(s => (long) s).ToArray();

			if (buffers.Length == 0 || lengths.Length == 0)
			{
				this.Log("Error executing kernel", "No buffers or lengths found for indexPointer");
				return 0;
			}

			// Identify input buffer and length parameter positions
			int inputBufferIndex = -1;
			int lengthIndex = -1;

			for (int i = 0; i < parameters.Length; i++)
			{
				if (parameters[i].Contains("__global"))
				{
					inputBufferIndex = i;
				}
				else if (parameters[i].Contains("int") || parameters[i].Contains("long"))
				{
					lengthIndex = i;
				}
			}

			if (inputBufferIndex == -1 || lengthIndex == -1)
			{
				this.Log("Error executing kernel", "Could not find required parameters (__global buffer, int/long length)");
				return 0;
			}

			// Stopwatch start
			Stopwatch sw = Stopwatch.StartNew();

			// Apply Kernel on all found buffers (IN-PLACE!)
			for (int i = 0; i < buffers.Length; i++)
			{
				// Set input buffer
				CL.SetKernelArg(this.Kernel.Value, (uint) inputBufferIndex, (IntPtr) buffers[i].Handle);

				// Set length parameter (int or long)
				if (parameters[lengthIndex].Contains("int"))
				{
					int len = (int) lengths[i];
					CL.SetKernelArg(this.Kernel.Value, (uint) lengthIndex, len);
				}
				else
				{
					CL.SetKernelArg(this.Kernel.Value, (uint) lengthIndex, lengths[i]);
				}

				// Set additional arguments
				int argsArrayIndex = 0;
				for (int j = 0; j < parameters.Length; j++)
				{
					if (j != inputBufferIndex && j != lengthIndex)
					{
						if (argsArrayIndex < args.Length)
						{
							this.Log($"Type of args[{argsArrayIndex}]:", args[argsArrayIndex]?.GetType().ToString());
							if (parameters[j].ToLower().Contains("float") && args[argsArrayIndex] is float floatValue)
							{
								CL.SetKernelArg(this.Kernel.Value, (uint) j, floatValue);
							}
							else if (parameters[j].Contains("double") && args[argsArrayIndex] is double doubleValue)
							{
								CL.SetKernelArg(this.Kernel.Value, (uint) j, doubleValue);
							}
							else if (parameters[j].Contains("int") && args[argsArrayIndex] is int intValue)
							{
								CL.SetKernelArg(this.Kernel.Value, (uint) j, intValue);
							}
							else if (parameters[j].Contains("long") && args[argsArrayIndex] is long longValue)
							{
								CL.SetKernelArg(this.Kernel.Value, (uint) j, longValue);
							}
							else if (parameters[j].Contains("float") && args[argsArrayIndex] is decimal decimalValue)
							{
								CL.SetKernelArg(this.Kernel.Value, (uint) j, decimalValue);
							}
							else
							{
								this.Log($"Error setting kernel argument at index {j} ({parameters[j]})", $"Type mismatch or value not provided in args at index {argsArrayIndex}");
								return indexPointer;
							}
							argsArrayIndex++;
						}
						else
						{
							this.Log($"Error setting kernel argument at index {j} ({parameters[j]})", "No corresponding value found in args");
							return indexPointer;
						}
					}
				}

				// Execute Kernel (In-Place) for this buffer
				CL.EnqueueNDRangeKernel(this.MemH.Que.Value, this.Kernel.Value, 1, null, [(nuint) lengths[i] / 4], null, 0, null, out CLEvent _);
				CL.Finish(this.MemH.Que.Value);

				// Log
				this.Log("Kernel executed", "Buffer #" + i + " (" + lengths[i].ToString("N0") + " elements, args = {" + string.Join(',', args) + "})", 1);
			}

			// Post log
			sw.Stop();
			this.Log("Kernel execution time: " + sw.ElapsedMilliseconds + " ms", buffers.Length.ToString("N0") + " buffer(s)", 1);

			// Return original indexPointer (since processing was in-place)
			return indexPointer;
		}


	}
}
