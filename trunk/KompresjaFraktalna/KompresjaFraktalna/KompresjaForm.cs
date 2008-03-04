using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

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
			fc = new FractalCompressor();
		}		

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog()== DialogResult.OK) {
                string file = this.openFileDialog1.FileName;

				Bitmap bmp = new Bitmap(file);
				fc.Input = bmp;

				this.input.Image = fc.Input;
				this.input.Refresh();
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
					break;
				case Operation.Decompression:
					MessageBox.Show("Dekompresja zakoñczona");
					output.Image = fc.Output;
					break;
				default:
					break;
			}
		}

		void backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
			Operation op = (Operation)e.Argument;

			Console.WriteLine("Starting: " + op.ToString());

			switch (op) {
				case Operation.Compression:
					fc.Compress();
					e.Result = op;
					break;
				case Operation.Decompression:
					fc.Decompress();
					e.Result = op;
					output.Image = fc.Output;

					break;
				default:
					break;
			}
		}
    }
}
