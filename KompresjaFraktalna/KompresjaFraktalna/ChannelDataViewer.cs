using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace KompresjaFraktalna {
	public partial class ChannelDataViewer : UserControl {
		int state = 0;

		public ChannelDataViewer() {
			InitializeComponent();
		}

		private void ChannelDataViewer_Load(object sender, EventArgs e) {

		}

		private ChannelData red;

		public ChannelData Red {
			get { return red; }
			set {
				red = value;
				this.Invalidate();
			}
		}

		private ChannelData green;

		public ChannelData Green {
			get { return green; }
			set {
				green = value;
				this.Invalidate();
			}
		}

		private ChannelData blue;

		public ChannelData Blue {
			get { return blue; }
			set {
				blue = value;
				this.Invalidate();
			}
		}

		void nextState() {
			state++;
			state %= 3;

			this.Invalidate();
		}

		private void ChannelDataViewer_Click(object sender, EventArgs e) {
			nextState();
		}

		private void ChannelDataViewer_Paint(object sender, PaintEventArgs e) {
			ChannelData cd = null;
			Pen pen = Pens.Black;

			if (state == 0) {
				cd = red;
				pen = Pens.Red;
			} else if (state == 1) {
				cd = green;
				pen = Pens.Green;
			} else {
				cd = blue;
				pen = Pens.Blue;
			}

			if (cd == null) {
				return;
			}

			Graphics g = e.Graphics;

			int xDomains = (cd.Width - 1) / cd.BigDelta;
			int yHeight = (cd.Height - 1) / cd.BigDelta;
			for (int x = 0; x < xDomains; x++) {
				for (int y = 0; y < yHeight; y++) {
					g.DrawRectangle(Pens.Black, x * cd.BigDelta, y * cd.BigDelta, cd.BigDelta + 2, cd.BigDelta + 2);
				}
			}

			foreach (Region r in cd.Regions) {
				g.DrawRectangle(pen, r.Left, r.Bottom, r.Width + 1, r.Height + 1);
			}
		}
	}
}
