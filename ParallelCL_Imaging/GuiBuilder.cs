using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace ParallelCL_Imaging
{
	public class GuiBuilder
	{
		// ----- ----- ----- ATTRIBUTES ----- ----- ----- \\
		public MainView Win;

		public Panel ParamsPanel;

		public Label[] ParamsLabels = [];
		public NumericUpDown[] ParamsNumerics = [];
		public ToolTip[] ParamsTips = [];



		// ----- ----- ----- CONSTRUCTOR ----- ----- ----- \\
		public GuiBuilder(MainView window)
		{
			// Set attributes
			Win = window;

			// Find panel for params input
			ParamsPanel = (GetControlByName("panel_kernelParams") as Panel) ?? new Panel();
		}








		// ----- ----- ----- METHODS ----- ----- ----- \\
		public Control? GetControlByName(string name)
		{
			return Win.Controls.Find(name, true).FirstOrDefault();
		}

		public void FillParams(string[] parameters, int margin = 5, int height = 23)
		{
			ParamsPanel.Controls.Clear();

			int pointers = 0;
			int lengths = 0;
			this.ParamsLabels = new Label[parameters.Length];
			this.ParamsNumerics = new NumericUpDown[parameters.Length];
			this.ParamsTips = new ToolTip[parameters.Length];

			// For each parameter: Generate label, numeric and tooltip on panel
			for (int i = 0; i < parameters.Length; i++)
			{
				// Skip if contains __global & * (pointer)
				if (parameters[i].Contains("__global") && parameters[i].Contains("*"))
				{
					pointers++;
					continue;
				}

				// Skip first int or if contains int and less than pointers
				if ((parameters[i].Contains("int") || parameters[i].Contains("long")) && (lengths < pointers || lengths == 0))
				{
					lengths++;
					continue;
				}

				int index = i - pointers - lengths;

				// Label
				ParamsLabels[index] = new Label();
				ParamsLabels[index].Text = parameters[i];
				ParamsLabels[index].Location = new Point(margin, margin + index * height);
				ParamsLabels[index].Size = new Size(100, height);
				ParamsPanel.Controls.Add(ParamsLabels[index]);

				// Numeric
				ParamsNumerics[index] = new NumericUpDown();
				ParamsNumerics[index].Location = new Point(100 + margin, margin + index * height);
				ParamsNumerics[index].Size = new Size(160, height);
				ParamsPanel.Controls.Add(ParamsNumerics[index]);

				// Tooltip
				ParamsTips[index] = new ToolTip();
			}

			// Add panel to window
			Win.Controls.Add(ParamsPanel);
		}





	}
}
