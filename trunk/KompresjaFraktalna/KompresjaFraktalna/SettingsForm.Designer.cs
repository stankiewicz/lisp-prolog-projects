namespace KompresjaFraktalna {
	partial class SettingsForm {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.button1 = new System.Windows.Forms.Button();
			this.nudEps = new System.Windows.Forms.NumericUpDown();
			this.nudEpsHij = new System.Windows.Forms.NumericUpDown();
			this.nudBigDelta = new System.Windows.Forms.NumericUpDown();
			this.nudSmallDelta = new System.Windows.Forms.NumericUpDown();
			this.nudMaxDepth = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.nudEps)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudEpsHij)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudBigDelta)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSmallDelta)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxDepth)).BeginInit();
			this.SuspendLayout();
			// 
			// button1
			// 
			this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.button1.Location = new System.Drawing.Point(12, 166);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 0;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// nudEps
			// 
			this.nudEps.Location = new System.Drawing.Point(74, 7);
			this.nudEps.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nudEps.Name = "nudEps";
			this.nudEps.Size = new System.Drawing.Size(54, 20);
			this.nudEps.TabIndex = 1;
			// 
			// nudEpsHij
			// 
			this.nudEpsHij.Location = new System.Drawing.Point(74, 37);
			this.nudEpsHij.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nudEpsHij.Name = "nudEpsHij";
			this.nudEpsHij.Size = new System.Drawing.Size(54, 20);
			this.nudEpsHij.TabIndex = 2;
			// 
			// nudBigDelta
			// 
			this.nudBigDelta.Location = new System.Drawing.Point(74, 69);
			this.nudBigDelta.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nudBigDelta.Name = "nudBigDelta";
			this.nudBigDelta.Size = new System.Drawing.Size(54, 20);
			this.nudBigDelta.TabIndex = 3;
			// 
			// nudSmallDelta
			// 
			this.nudSmallDelta.Location = new System.Drawing.Point(74, 106);
			this.nudSmallDelta.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.nudSmallDelta.Name = "nudSmallDelta";
			this.nudSmallDelta.Size = new System.Drawing.Size(54, 20);
			this.nudSmallDelta.TabIndex = 4;
			this.nudSmallDelta.ValueChanged += new System.EventHandler(this.nudSmallDelta_ValueChanged);
			// 
			// nudMaxDepth
			// 
			this.nudMaxDepth.Location = new System.Drawing.Point(74, 140);
			this.nudMaxDepth.Name = "nudMaxDepth";
			this.nudMaxDepth.Size = new System.Drawing.Size(54, 20);
			this.nudMaxDepth.TabIndex = 5;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(44, 13);
			this.label1.TabIndex = 6;
			this.label1.Text = "Epsilon:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 39);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 13);
			this.label2.TabIndex = 7;
			this.label2.Text = "EpsilonHij";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 71);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 13);
			this.label3.TabIndex = 8;
			this.label3.Text = "Big delta:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 108);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(61, 13);
			this.label4.TabIndex = 9;
			this.label4.Text = "Small delta:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 142);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(60, 13);
			this.label5.TabIndex = 10;
			this.label5.Text = "Max depth:";
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(139, 201);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.nudMaxDepth);
			this.Controls.Add(this.nudSmallDelta);
			this.Controls.Add(this.nudBigDelta);
			this.Controls.Add(this.nudEpsHij);
			this.Controls.Add(this.nudEps);
			this.Controls.Add(this.button1);
			this.Name = "SettingsForm";
			this.Text = "SettingsForm";
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.nudEps)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudEpsHij)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudBigDelta)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudSmallDelta)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxDepth)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.NumericUpDown nudEps;
		private System.Windows.Forms.NumericUpDown nudEpsHij;
		private System.Windows.Forms.NumericUpDown nudBigDelta;
		private System.Windows.Forms.NumericUpDown nudSmallDelta;
		private System.Windows.Forms.NumericUpDown nudMaxDepth;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
	}
}