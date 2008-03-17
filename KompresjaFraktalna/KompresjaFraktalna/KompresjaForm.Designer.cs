namespace KompresjaFraktalna {
    partial class KompresjaForm {
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
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.compressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.decompressToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.saveCompressedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.loadCompressedDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.input = new System.Windows.Forms.PictureBox();
			this.output = new System.Windows.Forms.PictureBox();
			this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.channelDataViewer1 = new KompresjaFraktalna.ChannelDataViewer();
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.menuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.input)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.output)).BeginInit();
			this.statusStrip1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(775, 24);
			this.menuStrip1.TabIndex = 0;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.compressToolStripMenuItem,
            this.decompressToolStripMenuItem,
            this.toolStripSeparator1,
            this.saveCompressedDataToolStripMenuItem,
            this.loadCompressedDataToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+O";
			this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.openToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
			this.openToolStripMenuItem.Text = "Open bitmap";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+S";
			this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
			this.saveToolStripMenuItem.Text = "Save bitmap";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// compressToolStripMenuItem
			// 
			this.compressToolStripMenuItem.Name = "compressToolStripMenuItem";
			this.compressToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+C";
			this.compressToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.compressToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
			this.compressToolStripMenuItem.Text = "Compress";
			this.compressToolStripMenuItem.Click += new System.EventHandler(this.compressToolStripMenuItem_Click);
			// 
			// decompressToolStripMenuItem
			// 
			this.decompressToolStripMenuItem.Name = "decompressToolStripMenuItem";
			this.decompressToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+D";
			this.decompressToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
			this.decompressToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
			this.decompressToolStripMenuItem.Text = "Decompress";
			this.decompressToolStripMenuItem.Click += new System.EventHandler(this.decompressToolStripMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(180, 6);
			// 
			// saveCompressedDataToolStripMenuItem
			// 
			this.saveCompressedDataToolStripMenuItem.Name = "saveCompressedDataToolStripMenuItem";
			this.saveCompressedDataToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
			this.saveCompressedDataToolStripMenuItem.Text = "Save compressed data";
			this.saveCompressedDataToolStripMenuItem.Click += new System.EventHandler(this.saveCompressedDataToolStripMenuItem_Click);
			// 
			// loadCompressedDataToolStripMenuItem
			// 
			this.loadCompressedDataToolStripMenuItem.Name = "loadCompressedDataToolStripMenuItem";
			this.loadCompressedDataToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
			this.loadCompressedDataToolStripMenuItem.Text = "Load compressed data";
			this.loadCompressedDataToolStripMenuItem.Click += new System.EventHandler(this.loadCompressedDataToolStripMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(180, 6);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+X";
			this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+A";
			this.aboutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.FileName = "openFileDialog1";
			this.openFileDialog1.Filter = "all|*.*|pictures|*.jpg;*.png;*.bmp";
			// 
			// input
			// 
			this.input.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.input.Location = new System.Drawing.Point(3, 3);
			this.input.Name = "input";
			this.input.Size = new System.Drawing.Size(100, 100);
			this.input.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.input.TabIndex = 0;
			this.input.TabStop = false;
			// 
			// output
			// 
			this.output.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.output.Location = new System.Drawing.Point(0, 3);
			this.output.Name = "output";
			this.output.Size = new System.Drawing.Size(100, 100);
			this.output.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.output.TabIndex = 0;
			this.output.TabStop = false;
			// 
			// backgroundWorker
			// 
			this.backgroundWorker.WorkerReportsProgress = true;
			this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
			this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 666);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(775, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripProgressBar1
			// 
			this.toolStripProgressBar1.Name = "toolStripProgressBar1";
			this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.channelDataViewer1, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(775, 642);
			this.tableLayoutPanel1.TabIndex = 5;
			// 
			// channelDataViewer1
			// 
			this.channelDataViewer1.Blue = null;
			this.channelDataViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.channelDataViewer1.Green = null;
			this.channelDataViewer1.Location = new System.Drawing.Point(3, 324);
			this.channelDataViewer1.Name = "channelDataViewer1";
			this.channelDataViewer1.Red = null;
			this.channelDataViewer1.Size = new System.Drawing.Size(254, 234);
			this.channelDataViewer1.TabIndex = 5;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.input);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(381, 315);
			this.panel1.TabIndex = 6;
			// 
			// panel2
			// 
			this.panel2.AutoScroll = true;
			this.panel2.Controls.Add(this.output);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(390, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(382, 315);
			this.panel2.TabIndex = 7;
			// 
			// KompresjaForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(775, 688);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.statusStrip1);
			this.Controls.Add(this.menuStrip1);
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "KompresjaForm";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.KompresjaForm_Load);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.input)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.output)).EndInit();
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.PictureBox input;
        private System.Windows.Forms.PictureBox output;
		private System.Windows.Forms.ToolStripMenuItem compressToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem decompressToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem loadCompressedDataToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.ComponentModel.BackgroundWorker backgroundWorker;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private ChannelDataViewer channelDataViewer1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ToolStripMenuItem saveCompressedDataToolStripMenuItem;
    }
}

