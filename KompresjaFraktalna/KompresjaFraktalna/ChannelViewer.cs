using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KompresjaFraktalna {
	partial class ChannelViewer : Form {

		ChannelData cdR, cdG, cdB;
		UnsafeBitmap bmp = null;

		public ChannelViewer() {
			InitializeComponent();
		}

		private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e) {
			Graphics g = e.Graphics;

			if (bmp == null) {
				g.Clear(Color.Black);
				return;
			}

			g.DrawImageUnscaled(bmp.Bitmap, 0, 0);			
		}

		public ChannelData ChannelDataR {
			set {
				this.cdR = value;
			}
		}

		public ChannelData ChannelDataG {
			set {
				this.cdG = value;
			}
		}

		public ChannelData ChannelDataB {
			set {
				this.cdB = value;
			}
		}

		void channelHelper(Point[] interpolationPoints, int[,] channel) {
			foreach (Point p in interpolationPoints) {
				channel[p.X, p.Y] = p.Z;
			}
		}

		public void redrawBitmap() {
			if (cdR == null || cdG == null || cdB == null) return;

			int width = cdR.Width;
			int height = cdR.Height;

			int[,] r = new int[width, height], g = new int[width, height], b = new int[width, height];

			channelHelper(cdR.InterpolationPoints, r);
			channelHelper(cdG.InterpolationPoints, g);
			channelHelper(cdB.InterpolationPoints, b);

			bmp = new UnsafeBitmap(width, height);
			bmp.LockBitmap();
			PixelData pd;
			for (int x = 0; x < width; x++) {
				for (int y = 0; y < height; y++) {
					pd.R = (byte)r[x, y];
					pd.G = (byte)g[x, y];
					pd.B = (byte)b[x, y];
					bmp.SetPixel(x, y, pd);
				}
			}
			bmp.UnlockBitmap();
		}
	}
}
