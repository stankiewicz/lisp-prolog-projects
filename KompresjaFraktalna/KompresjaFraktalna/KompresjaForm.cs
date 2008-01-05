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
            Compressor compressor = new Compressor();
            int[,] colorR = new int[bmLeft.Width, bmLeft.Height];
            for (int i = 0; i < bmLeft.Width; ++i) {
                for (int j = 0; j < bmLeft.Height; ++j) {
                    colorR[i, j] = (int)bmLeft.GetPixel(i, j).R;
                }
            }
            this.backgroundWorker1.ReportProgress(5);


            int[,] colorG = new int[bmLeft.Width, bmLeft.Height];
            for (int i = 0; i < bmLeft.Width; ++i) {
                for (int j = 0; j < bmLeft.Height; ++j) {
                    colorG[i, j] = (int)bmLeft.GetPixel(i, j).R;
                }
            }

            this.backgroundWorker1.ReportProgress(10);

            int[,] colorB = new int[bmLeft.Width, bmLeft.Height];
            for (int i = 0; i < bmLeft.Width; ++i) {
                for (int j = 0; j < bmLeft.Height; ++j) {
                    colorB[i, j] = (int)bmLeft.GetPixel(i, j).R;
                }
            }

            this.backgroundWorker1.ReportProgress(15);

            compressor.Compress(colorR, memoryStream);

            this.backgroundWorker1.ReportProgress(35);

            compressor.Compress(colorG, memoryStream);

            this.backgroundWorker1.ReportProgress(50);

            compressor.Compress(colorB, memoryStream);

            this.backgroundWorker1.ReportProgress(70);

            memoryStream.Flush();
            compressedImage = memoryStream.ToArray();
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

        }

        private void przerToolStripMenuItem_Click(object sender, EventArgs e) {
            if(backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e) {
            this.toolStripProgressBar1.Value = e.ProgressPercentage;
            this.toolStripStatusLabel1.Text = e.ProgressPercentage.ToString() + " %";
            this.Validate();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            this.statusStrip1.Visible = false;
            this.Validate();
        }




    }
}