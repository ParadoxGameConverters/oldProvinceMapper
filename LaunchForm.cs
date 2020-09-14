using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;

namespace ProvinceMapper
{
	public delegate void StatusUpdate(double amount);

	public partial class LaunchForm : Form
	{
		public LaunchForm()
		{
			InitializeComponent();

			// load settings
			tbSourceMapFolder.Text = Properties.Settings.Default.srcMapFolder;
			tbDestMapFolder.Text = Properties.Settings.Default.destMapFolder;
			tbSourceTag.Text = Properties.Settings.Default.srcTag;
			tbDestTag.Text = Properties.Settings.Default.destTag;
			tbMappingsFile.Text = Properties.Settings.Default.mappingFile;
			cbScale.Checked = Properties.Settings.Default.fitMaps;
			cbRivers.Checked = Properties.Settings.Default.showRivers;
			cbNamesFrom.SelectedItem = Properties.Settings.Default.namesFrom;
			ckInvertSource.Checked = Properties.Settings.Default.invertSource;
			ckInvertDest.Checked = Properties.Settings.Default.invertDest;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// read definitions and create province lists
			lblStatus.Text = "Load Source Definitions";
			Application.DoEvents();
			string sourceDefPath = Path.Combine(tbSourceMapFolder.Text, "Definition.csv");
			Program.sourceDef = new DefinitionReader(sourceDefPath, PushStatusUpdate);

			lblStatus.Text = "Load Target Definitions";
			Application.DoEvents();
			string targetDefPath = Path.Combine(tbDestMapFolder.Text, "Definition.csv");
			Program.targetDef = new DefinitionReader(targetDefPath, PushStatusUpdate);

			// pre-scale maps
			lblStatus.Text = "Scale Maps";
			PushStatusUpdate(0.0);
			Application.DoEvents();
			string sourceMapPath = Path.Combine(tbSourceMapFolder.Text, "Provinces.bmp");
			Bitmap srcMapNoRivers = (Bitmap)Image.FromFile(sourceMapPath);
			string sourceRiversMapPath = Path.Combine(tbSourceMapFolder.Text, "rivers.bmp");
			Bitmap srcRiversMap = (Bitmap)Image.FromFile(sourceRiversMapPath);
			Bitmap srcMap = new Bitmap(srcMapNoRivers.Width, srcMapNoRivers.Height, PixelFormat.Format32bppArgb);
			// add the rivers to the source map 
			if (cbRivers.Checked)
			{
				AddRiversToMap(srcMap, srcMapNoRivers, srcRiversMap);
			}
			else
			{
				srcMap = srcMapNoRivers;
			}
			PushStatusUpdate(33.0);

			string targetMapPath = Path.Combine(tbDestMapFolder.Text, "Provinces.bmp");
			Bitmap targetMapNoRivers = (Bitmap)Image.FromFile(targetMapPath);
			string targetRiversMapPath = Path.Combine(tbDestMapFolder.Text, "rivers.bmp");
			Bitmap targetRiversMap = (Bitmap)Image.FromFile(targetRiversMapPath);
			Bitmap targetMap = new Bitmap(targetMapNoRivers.Width, targetMapNoRivers.Height, PixelFormat.Format32bppArgb);
			// add the rivers to the target map 
			if (cbRivers.Checked)
			{
				AddRiversToMap(targetMap, targetMapNoRivers, targetRiversMap);
			}
			else
			{
				targetMap = targetMapNoRivers;
			}
			PushStatusUpdate(67.0);
			if (cbScale.Checked)
			{
				int h = Math.Max(srcMap.Height, targetMap.Height);
				int w = Math.Max(srcMap.Width, targetMap.Width);
				if (srcMap.Height < h || srcMap.Width < w)
				{
					srcMap = Program.CleanResizeBitmap(srcMap, w, h);
				}
				if (targetMap.Height < h || targetMap.Width < w)
				{
					targetMap = Program.CleanResizeBitmap(targetMap, w, h);
				}
			}
			PushStatusUpdate(100.0);
			srcMap.Tag = sourceMapPath;
			targetMap.Tag = targetMapPath;

			// add geo data to province lists
			lblStatus.Text = "Load Source Map";
			Application.DoEvents();
			Program.sourceMap = new MapReader(srcMap, Program.sourceDef.provinces, ckInvertSource.Checked, PushStatusUpdate);

			lblStatus.Text = "Load Target Map";
			Application.DoEvents();
			Program.targetMap = new MapReader(targetMap, Program.targetDef.provinces, ckInvertDest.Checked, PushStatusUpdate);

			// load localizations, if desired
			if (cbNamesFrom.SelectedItem.ToString() == "Localization")
			{
				lblStatus.Text = "Load Source Localization";
				Application.DoEvents();
				LocalizationReader lr = new LocalizationReader(tbSourceMapFolder.Text, Program.sourceDef.provinces, PushStatusUpdate);

				lblStatus.Text = "Load Target Localization";
				Application.DoEvents();
				lr = new LocalizationReader(tbDestMapFolder.Text, Program.targetDef.provinces, PushStatusUpdate);
			}

			// read existing mappings (if any)
			string mappingFile = tbMappingsFile.Text.Trim();
			if (mappingFile != String.Empty && File.Exists(mappingFile))
			{
				lblStatus.Text = "Parse Existing Mappings";
				Application.DoEvents();
				Program.mappings = new MappingReader(mappingFile, tbSourceTag.Text,
					 tbDestTag.Text, Program.sourceDef.provinces, Program.targetDef.provinces, PushStatusUpdate);
			}
			else
			{
				Program.mappings = new MappingReader(mappingFile, tbSourceTag.Text, tbDestTag.Text);
			}

			// save settings
			Properties.Settings.Default.srcMapFolder = tbSourceMapFolder.Text;
			Properties.Settings.Default.destMapFolder = tbDestMapFolder.Text;
			Properties.Settings.Default.srcTag = tbSourceTag.Text;
			Properties.Settings.Default.destTag = tbDestTag.Text;
			Properties.Settings.Default.mappingFile = tbMappingsFile.Text;
			Properties.Settings.Default.fitMaps = cbScale.Checked;
			Properties.Settings.Default.namesFrom = cbNamesFrom.SelectedItem.ToString();
			Properties.Settings.Default.invertSource = ckInvertSource.Checked;
			Properties.Settings.Default.invertDest = ckInvertDest.Checked;
			Properties.Settings.Default.Save();
		}

		private void btnExit_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		public void PushStatusUpdate(double amount)
		{
			int actualAmt = (int)amount;
			if (actualAmt != progressBar1.Value)
			{
				// ProgressBar updates much more reliably going backwards
				if (actualAmt < 100)
				{
					progressBar1.Value = actualAmt + 1;
				}
				progressBar1.Value = actualAmt;
				Application.DoEvents();
			}
		}

		public void AddRiversToMap(Bitmap outputMap, Bitmap inputMap, Bitmap riversMap)
        {
			var rect = new Rectangle(0, 0, inputMap.Width, inputMap.Height);
			var bitsRiver = riversMap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			var bitsInput = inputMap.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
			var bitsOutput = outputMap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
			unsafe
			{
				for (int y = 0; y < outputMap.Height; y++)
				{
					byte* lineRiver = (byte*)bitsRiver.Scan0 + y * bitsRiver.Stride;
					byte* lineInput = (byte*)bitsInput.Scan0 + y * bitsInput.Stride;
					byte* lineOutput = (byte*)bitsOutput.Scan0 + y * bitsOutput.Stride;
					for (int x = 0; x < outputMap.Width; x++)
					{
						lineOutput[4 * x + 2] = lineInput[4 * x + 2];   // red
						lineOutput[4 * x + 1] = lineInput[4 * x + 1];   // green
						lineOutput[4 * x] = lineInput[4 * x];           // blue
						lineOutput[4 * x + 3] = lineInput[4 * x + 3];      // alpha

						if (!IsPink(lineRiver, x) && !IsGrey(lineRiver, x) && !IsWhite(lineRiver, x))
						{ // pixel on rivers map is not pink, not grey, not white
							lineOutput[4 * x + 3] = 0;        // make the pixel on main map fully transparent
						}
					}
				}
			}
			riversMap.UnlockBits(bitsRiver);
			inputMap.UnlockBits(bitsInput);
			outputMap.UnlockBits(bitsOutput);
		}

		unsafe public bool IsPink(byte* lineRiver, int x)
		{
			if (lineRiver[4 * x + 2] == 255 && lineRiver[4 * x + 1] == 0 && lineRiver[4 * x] == 128) { return true; }
            else { return false; }
        }
		unsafe public bool IsGrey(byte* lineRiver, int x)
		{
			if (lineRiver[4 * x + 2] == 122 && lineRiver[4 * x + 1] == 122 && lineRiver[4 * x] == 122) { return true; }
			else { return false; }
		}
		unsafe public bool IsWhite(byte* lineRiver, int x)
		{
			if (lineRiver[4 * x + 2] == 255 && lineRiver[4 * x + 1] == 255 && lineRiver[4 * x] == 255) { return true; }
			else { return false; }
		}
	}
}
