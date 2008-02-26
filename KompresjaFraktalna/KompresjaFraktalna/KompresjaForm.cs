using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KompresjaFraktalna {
    public partial class KompresjaForm : Form {

		Bitmap imageToCompress;

        public KompresjaForm() {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e) {
            if (openFileDialog1.ShowDialog()== DialogResult.OK) {
                string file = this.openFileDialog1.FileName;

				//utworzenie bitmapy o odpowiednim rozmiarze (n*delta+1)
				Bitmap bmp = new Bitmap(file);
				int newWidth = getNewSize(bmp.Width);
				int newHeight = getNewSize(bmp.Height);
				imageToCompress = new Bitmap(newWidth, newHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				Graphics g = Graphics.FromImage(imageToCompress);
				g.Clear(Color.White);
				g.DrawImageUnscaled(bmp, 0, 0);
				g.Dispose();
				//

                this.pictureBox1.Image = imageToCompress;
				this.pictureBox1.Refresh();
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e) {
			if (pictureBox2.Image == null) {
				MessageBox.Show("Brak skompresowanego obrazka");
				return;
			}

			if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
				pictureBox2.Image.Save(saveFileDialog1.FileName);
				MessageBox.Show("Plik zapisany");
			}
        }

        private void compressToolStripMenuItem_Click(object sender, EventArgs e) {
			if (!compressToolStripMenuItem.Enabled) {
				return;
			}
			compressToolStripMenuItem.Enabled = false;

			if (imageToCompress == null) {
				MessageBox.Show("Nie wczytano bitmapy");
				return;
			}

            statusStrip1.Visible = true;
            toolStripProgressBar1.Value = 0;
            backgroundWorker1.RunWorkerAsync();
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {

			Console.WriteLine("Begin compression");
			
            // dzielimy na 3 kolory
            UnsafeBitmap bm = new UnsafeBitmap(imageToCompress);
            bm.LockBitmap();
            Compressor compressor = new Compressor();

			int[,] colorR = new int[bm.Width, bm.Height];
			int[,] colorG = new int[bm.Width, bm.Height];
			int[,] colorB = new int[bm.Width, bm.Height];

			Console.WriteLine("Prepare image data");

			for (int i = 0; i < bm.Width; ++i) {
				for (int j = 0; j < bm.Height; ++j) {
                    PixelData pd = bm.GetPixel(i, j);
                    colorB[i, j] = pd.B;
                    colorG[i, j] = pd.G;
                    colorR[i, j] = pd.R;
                }
            }
            bm.UnlockBitmap();

			Console.WriteLine("Image data ready");

			ChannelData redChannel, blueChannel, greenChannel;

			Console.WriteLine("Compressing red channel");
            redChannel = compressor.Compress(colorR);
			Console.WriteLine("Red channel compressed");

			Console.WriteLine("Compressing green channel");
			greenChannel = compressor.Compress(colorG);
			Console.WriteLine("Green channel compressed");

			Console.WriteLine("Compressing blue channel");
			blueChannel = compressor.Compress(colorB);
			Console.WriteLine("Blue channel compressed");

			
			Console.WriteLine("Decompressing image data");
            
			Decompressor decompressor = new Decompressor();

			Console.WriteLine("Decompressing red channel");
            colorR = decompressor.Decompress(redChannel);
			Console.WriteLine("Red channel decompressed");

            this.backgroundWorker1.ReportProgress(75);

			Console.WriteLine("Decompressing green channel");
            colorG = decompressor.Decompress(greenChannel);
			Console.WriteLine("Green channel decompressed");

            this.backgroundWorker1.ReportProgress(80);

			Console.WriteLine("Decompressing blue channel");
            colorB = decompressor.Decompress(blueChannel);
			Console.WriteLine("Blue channel decompressed");

            this.backgroundWorker1.ReportProgress(85);

			Console.WriteLine("Tworzenie koñcowej bitmapy");
			Bitmap bmp = new Bitmap(colorB.GetLength(0), colorB.GetLength(1), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			UnsafeBitmap unsafeBitmap = new UnsafeBitmap(bmp);
			unsafeBitmap.LockBitmap();

			for (int i = 0; i < unsafeBitmap.Width; ++i) {
				for (int j = 0; j < unsafeBitmap.Height; ++j) {
					PixelData pd = new PixelData((byte)colorR[i, j], (byte)colorG[i, j], (byte)colorB[i, j]);
					unsafeBitmap.SetPixel(i, j, pd);
                }
            }
			unsafeBitmap.UnlockBitmap();

			Console.WriteLine("Tworzenie koñcowej bitmapy zakoñczone");

			pictureBox2.Image = unsafeBitmap.Bitmap;
        }

        private void przerToolStripMenuItem_Click(object sender, EventArgs e) {
            //if(backgroundWorker1.IsBusy)
            //    backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            this.toolStripProgressBar1.Value = e.ProgressPercentage;
            this.toolStripStatusLabel1.Text = e.ProgressPercentage.ToString() + " %";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
			Console.WriteLine("Kompresja zakoñczona");
            if (e.Error != null) {
                MessageBox.Show(e.Error.Message);
                System.Console.Out.WriteLine(e.Error.Message);
            }
            this.statusStrip1.Visible = false;
			pictureBox2.Refresh();
        }

		private int getNewSize(int oldSize) {
			int bigDelta = Compression.Default.BigDelta;
			int newSize = 0;
			while (newSize < oldSize) {
				newSize += bigDelta;
			}
			newSize++;
			return newSize;
		}
    }
}
