using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KompresjaFraktalna {
    public partial class KompresjaForm : Form {

        Bitmap bmLeft;

        Bitmap tmp;

        byte[] compressedImage;

        public KompresjaForm() {
            InitializeComponent();
            this.splitContainer1.Panel1.AutoScrollMinSize = new Size(this.splitContainer1.Panel1.Width, this.splitContainer1.Panel1.Height);
            this.splitContainer1.Panel2.AutoScrollMinSize = new Size(this.splitContainer1.Panel2.Width, this.splitContainer1.Panel2.Height);
        }

        private void wyjdŸToolStripMenuItem_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void otwórzPlikToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.openFileDialog1.ShowDialog()== DialogResult.OK) {
                string file = this.openFileDialog1.FileName;

                bmLeft = new Bitmap(Bitmap.FromFile(file));
                this.pictureBox1.Image = bmLeft;
                this.Validate();
            }
        }

        private void zapiszPlikToolStripMenuItem_Click(object sender, EventArgs e) {

        }

        private void kompresujToolStripMenuItem_Click(object sender, EventArgs e) {
            if (bmLeft != null) {
                tmp = new Bitmap(bmLeft);
                statusStrip1.Visible = true;
                toolStripStatusLabel1.Text = "0 %";
                toolStripProgressBar1.Value = 0;
                backgroundWorker1.DoWork -= new DoWorkEventHandler(backgroundWorker1_DoWork);
                backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
                backgroundWorker1.RunWorkerAsync();
            }
        }

        void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e) {

            System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();

            // dzielimy na 3 kolory

            UnsafeBitmap bm = new UnsafeBitmap(tmp);
            bm.LockBitmap();
            Compressor compressor = new Compressor();

            
            int[,] colorR = new int[tmp.Width, tmp.Height];
            int[,] colorG = new int[tmp.Width, tmp.Height];
            int[,] colorB = new int[tmp.Width, tmp.Height];
            PixelData pd;
            for (int i = 0; i < bm.Width; ++i) {
                for (int j = 0; j < bm.Height; ++j) {
                    pd = bm.GetPixel(i, j);
                    colorB[i, j] = (int)pd.B;
                    colorG[i, j] = (int)pd.G;
                    colorR[i, j] = (int)pd.R;
                }
            }
            bm.UnlockBitmap();

            //this.backgroundWorker1.ReportProgress(15);

            compressor.Compress(colorR, memoryStream);

            //this.backgroundWorker1.ReportProgress(35);

            compressor.Compress(colorG, memoryStream);

            //this.backgroundWorker1.ReportProgress(50);

            compressor.Compress(colorB, memoryStream);

            //this.backgroundWorker1.ReportProgress(70);

            memoryStream.Flush();
            compressedImage = memoryStream.ToArray();

            System.Console.Out.Write(compressedImage);

            memoryStream.Position = 0;
            
            // a teraz odtwarzamy
            

            Decompressor decompressor = new Decompressor();

            colorR = decompressor.Decompress(memoryStream);

            this.backgroundWorker1.ReportProgress(75);

            colorG = decompressor.Decompress(memoryStream);

            this.backgroundWorker1.ReportProgress(80);

            colorB = decompressor.Decompress(memoryStream);

            this.backgroundWorker1.ReportProgress(85);

            Bitmap bmRight = new Bitmap(colorB.GetLength(0), colorB.GetLength(1));

            for (int i = 0; i < bmRight.Width; ++i) {
                for (int j = 0; j < bmRight.Height; ++j) {
                    bmRight.SetPixel(i, j, Color.FromArgb(colorR[i, j], colorG[i, j], colorB[i, j]));
                }
            }

            pictureBox2.Image = bmRight;
            this.Validate();

        }

        private void przerToolStripMenuItem_Click(object sender, EventArgs e) {
            //if(backgroundWorker1.IsBusy)
            //    backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            this.toolStripProgressBar1.Value = e.ProgressPercentage;
            this.toolStripStatusLabel1.Text = e.ProgressPercentage.ToString() + " %";
            this.Validate();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Error != null) {
                MessageBox.Show(e.Error.Message);
                System.Console.Out.WriteLine(e.Error.Message);
            }
            System.Console.Out.WriteLine("dupa");
            this.statusStrip1.Visible = false;
            this.Validate();
        }




    }
}