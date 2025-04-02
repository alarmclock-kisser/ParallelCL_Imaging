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
			List<Label> labels = new();
			List<NumericUpDown> numerics = new();
			List<ToolTip> tips = new();

			// **Erst alle Parameter verarbeiten und Listen füllen**
			for (int i = 0; i < parameters.Length; i++)
			{
				// **Pointer (__global *): Überspringen**
				if (parameters[i].Contains("__global") && parameters[i].Contains("*"))
				{
					pointers++;
					continue;
				}

				// **Erste int/long oder wenn weniger als Pointers: Überspringen**
				if ((parameters[i].Contains("int") || parameters[i].Contains("long")) && (lengths < pointers || lengths == 0))
				{
					lengths++;
					continue;
				}

				// **NumericUpDown-Werte setzen**
				decimal min = -1000, max = 1000, inc = 1, val = 0;
				int dec = 0;

				if (parameters[i].Contains("float"))
				{
					inc = 0.01M; dec = 6;
				}
				else if (parameters[i].Contains("int"))
				{
					inc = 1; dec = 0;
				}
				else if (parameters[i].Contains("long"))
				{
					min = -999999999; max = 999999999; inc = 1; dec = 0;
				}
				else if (parameters[i].Contains("double"))
				{
					inc = 0.001M; dec = 15;
				}
				else if (parameters[i].Contains("decimal"))
				{
					inc = 0.000001M; dec = 24;
				}

				int index = labels.Count;  // **Neuer Index anhand der Listenlänge**

				// **Label erstellen**
				Label lbl = new()
				{
					Text = parameters[i],
					Location = new Point(margin, margin + index * height),
					Size = new Size(100, height)
				};
				labels.Add(lbl);

				// **NumericUpDown erstellen**
				NumericUpDown num = new()
				{
					Location = new Point(100 + margin, margin + index * height),
					Size = new Size(160, height),
					Minimum = min,
					Maximum = max,
					Increment = inc,
					DecimalPlaces = dec,
					Value = val
				};
				numerics.Add(num);

				// **ToolTip hinzufügen**
				tips.Add(new ToolTip());
			}

			// **Jetzt erst die Attribut-Arrays setzen**
			ParamsLabels = labels.ToArray();
			ParamsNumerics = numerics.ToArray();
			ParamsTips = tips.ToArray();

			// **Alles ins Panel packen**
			for (int i = 0; i < labels.Count; i++)
			{
				ParamsPanel.Controls.Add(ParamsLabels[i]);
				ParamsPanel.Controls.Add(ParamsNumerics[i]);
			}

			// **Panel ins Fenster hinzufügen**
			Win.Controls.Add(ParamsPanel);
		}

		public object[] GetParams()
		{
			if (ParamsNumerics.Length == 0)
			{
				return [];
			}

			object[] parameters = new object[ParamsNumerics.Length];
			for (int i = 0; i < ParamsNumerics.Length; i++)
			{
				if (ParamsNumerics[i] != null)
				{
					parameters[i] = ParamsNumerics[i].Value;
				}
			}
			return parameters;
		}





	}
}
