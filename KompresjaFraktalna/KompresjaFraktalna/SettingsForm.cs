using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KompresjaFraktalna.Properties;

namespace KompresjaFraktalna {
	public partial class SettingsForm : Form {
		public SettingsForm() {
			InitializeComponent();
		}

		private void SettingsForm_Load(object sender, EventArgs e) {
			this.nudBigDelta.Value = Settings.Default.BigDelta;
			this.nudEps.Value = (Decimal)Settings.Default.Epsilon;
			this.nudEpsHij.Value = (Decimal)Settings.Default.EpsilonHij;
			this.nudMaxDepth.Value = Settings.Default.Dmax;
			this.nudSmallDelta.Value = Settings.Default.SmallDelta;
		}

		private void button1_Click(object sender, EventArgs e) {
			Settings.Default.BigDelta = (int)nudBigDelta.Value;
			Settings.Default.Dmax = (int)nudMaxDepth.Value;
			Settings.Default.Epsilon = (double)nudEps.Value;
			Settings.Default.EpsilonHij = (double)nudEpsHij.Value;
			Settings.Default.SmallDelta = (int)nudSmallDelta.Value;

			Settings.Default.Save();
		}

		private void nudSmallDelta_ValueChanged(object sender, EventArgs e) {

		}
	}
}
