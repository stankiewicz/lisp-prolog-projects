using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO.Compression;
using System.Diagnostics;

namespace KompresjaFraktalna {
    public partial class KompresjaForm : Form {
		FractalCompressor fc = null;

		enum Operation {
			Compression, Decompression
		};

        public KompresjaForm() {
            InitializeComponent();
        }

		private void KompresjaForm_Load(object sender, EventArgs e) {
			int c = System.Diagnostics.Debug.Listeners.Add(new ConsoleTraceListener());

			fc = new FractalCompressor();
		}		

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog()== DialogResult.OK) {
                string file = this.openFileDialog1.FileName;

				Bitmap bmp = new Bitmap(file);
				bmp = (Bitmap)bmp.Clone();

				fc.Input = bmp;

				this.input.Image = fc.Input;
				this.input.Invalidate();

				this.channelDataViewer1.Size = fc.Input.Size;
				this.channelDataViewer1.Invalidate();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
			if (output.Image == null) {
				MessageBox.Show("Brak skompresowanego obrazka");
				return;
			}

			if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
				output.Image.Save(saveFileDialog1.FileName);
				MessageBox.Show("Plik zapisany");
			}
        }

        private void compressToolStripMenuItem_Click(object sender, EventArgs e) {
			if (backgroundWorker.IsBusy) {
				MessageBox.Show("Trwa kompresja. Proszê czekaæ.");
				return;
			}

			backgroundWorker.RunWorkerAsync(Operation.Compression);
        }

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			About a = new About();
			a.ShowDialog();
		}

		private void decompressToolStripMenuItem_Click(object sender, EventArgs e) {
			if (backgroundWorker.IsBusy) {
				MessageBox.Show("Poczekaj na zakoñczenie aktualnej operacji");
				return;
			}

			backgroundWorker.RunWorkerAsync(Operation.Decompression);
		}

		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e) {
			toolStripProgressBar1.Value = e.ProgressPercentage;
		}

		private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			Operation op = (Operation)e.Result;

			switch (op) {
				case Operation.Compression:
					MessageBox.Show("Kompresja zakoñczona");
					channelDataViewer1.Red = fc.RedChannel;
					channelDataViewer1.Green = fc.GreenChannel;
					channelDataViewer1.Blue = fc.BlueChannel;
					break;
				case Operation.Decompression:
					if (result) {
						MessageBox.Show("Dekompresja zakoñczona");
						output.Image = fc.Output;
					} else {
						MessageBox.Show("Dekompresja nieudana. Mo¿e z³y format pliku?");
					}
					break;
				default:
					break;
			}
		}

		bool result = true;
		void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
			Operation op = (Operation)e.Argument;

			Console.WriteLine("Starting: " + op.ToString());

			switch (op) {
				case Operation.Compression:
					fc.Compress();
					e.Result = op;
					break;
				case Operation.Decompression:
					result = fc.Decompress();
					e.Result = op;
					break;
				default:
					break;
			}
		}

		private void saveCompressedDataToolStripMenuItem_Click(object sender, EventArgs e) {
			if (fc == null) {
				MessageBox.Show("Nie zainicjalizowano kompresji");
				return;
			}
			if (fc.BlueChannel == null || fc.GreenChannel == null || fc.RedChannel == null) {
				MessageBox.Show("Brak skompresowanych danych");
				return;
			}

			if (saveFileDialog1.ShowDialog() != DialogResult.OK) {
				return;
			}

			MemoryStream ms = new MemoryStream();
			BinaryWriter bw = new BinaryWriter(ms);
			fc.RedChannel.serialize(bw);
			fc.GreenChannel.serialize(bw);
			fc.BlueChannel.serialize(bw);

			byte[] input = new byte[ms.Length];
			ms.Position = 0;
			ms.Read(input, 0, input.Length);

			bw.Close();
			ms.Close();

			FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
			GZipStream zip = new GZipStream(fs, CompressionMode.Compress);
			zip.Write(input, 0, input.Length);
			zip.Close();
			fs.Close();

			MessageBox.Show("File saved");
		}

		private void loadCompressedDataToolStripMenuItem_Click(object sender, EventArgs e) {
			if (openFileDialog1.ShowDialog() != DialogResult.OK) {
				return;
			}

			FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open);
			GZipStream zip = new GZipStream(fs, CompressionMode.Decompress, true);
			MemoryStream ms = new MemoryStream();
			byte[] buffer = new byte[(int)Math.Pow(2, 16)];
			int bytesRead;
			bool continueLoop = true;
			while (continueLoop) {
				bytesRead = zip.Read(buffer, 0, buffer.Length);
				if (bytesRead == 0)
					break;
				ms.Write(buffer, 0, bytesRead);
			}
			zip.Close();
			fs.Close();

			ms.Position = 0;

			BinaryReader br = new BinaryReader(ms);
			fc.RedChannel = ChannelData.deserialize(br);
			fc.GreenChannel = ChannelData.deserialize(br);
			fc.BlueChannel = ChannelData.deserialize(br);
			br.Close();

			MessageBox.Show("Data loaded");
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
			SettingsForm sf = new SettingsForm();
			sf.ShowDialog();
		}
    }
}
