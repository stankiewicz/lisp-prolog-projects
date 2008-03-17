using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using KompresjaFraktalna.Properties;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading;

namespace KompresjaFraktalna {
	class FractalCompressor {

		Bitmap _input, _output, _realInput, _realOutput;

		byte[,] _redChannel, _greenChannel, _blueChannel;

		ChannelData _redChannelData, _blueChannelData, _greenChannelData;

		public ChannelData RedChannel {
			get {
				return _redChannelData;
			}
			set { _redChannelData = value; }
		}

		public ChannelData GreenChannel {
			get {
				return _greenChannelData;
			}
			set { _greenChannelData = value; }
		}

		public ChannelData BlueChannel {
			get {
				return _blueChannelData;
			}
			set { _blueChannelData = value; }
		}

		public Bitmap Input {
			set {
				_realInput = value;

				//tworzymy now¹ bitmapê dla tego por¹banego algorytmu
				int newWidth = getNewSize(_realInput.Width);
				int newHeight = getNewSize(_realInput.Height);
				_input = new Bitmap(newWidth, newHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				Graphics g = Graphics.FromImage(_input);
				g.Clear(Color.White);
				g.DrawImageUnscaled(_realInput, 0, 0);
				g.Dispose();
			}
			get {
				return _input;
			}
		}

		public Bitmap Output {
			get {
				return _output;
			}
		}

		public Bitmap getRealInput() {
			return _realInput;
		}

		public Bitmap getRealOutput() {
			if (_output == null) {
				return null;
			}

			if (_realOutput == null) {
				Bitmap bmp = new Bitmap(_realInput.Width, _realInput.Height);
				Graphics g = Graphics.FromImage(bmp);
				g.DrawImageUnscaled(_output, 0, 0);
				g.Dispose();
			}

			return _realOutput;
		}

		Semaphore s = new Semaphore(0, 5);

		public void Compress() {
			if (_input == null) {
				throw new InvalidOperationException("Nie podano bitmapy");
			}

			prepareChannels();

			BackgroundWorker bw1 = new BackgroundWorker();
			bw1.DoWork += new DoWorkEventHandler(bw1_DoWork);
			bw1.RunWorkerAsync();

			BackgroundWorker bw2 = new BackgroundWorker();
			bw2.DoWork += new DoWorkEventHandler(bw2_DoWork);
			bw2.RunWorkerAsync();

			BackgroundWorker bw3 = new BackgroundWorker();
			bw3.DoWork += new DoWorkEventHandler(bw3_DoWork);
			bw3.RunWorkerAsync();

			s.WaitOne();
			s.WaitOne();
			s.WaitOne();
		}

		void bw1_DoWork(object sender, DoWorkEventArgs e) {
			Compressor compressor = new Compressor();
			Console.WriteLine("Compressing red channel");
			_redChannelData = compressor.Compress(_redChannel);
			Console.WriteLine("Red channel compressed");

			s.Release();
		}

		void bw2_DoWork(object sender, DoWorkEventArgs e) {
			Compressor compressor = new Compressor();
			Console.WriteLine("Compressing green channel");
			_greenChannelData = compressor.Compress(_greenChannel);
			Console.WriteLine("Green channel compressed");

			s.Release();
		}

		void bw3_DoWork(object sender, DoWorkEventArgs e) {
			Compressor compressor = new Compressor();
			Console.WriteLine("Compressing blue channel");
			_blueChannelData = compressor.Compress(_blueChannel);
			Console.WriteLine("Blue channel compressed");

			s.Release();
		}

		private void prepareChannels() {
			UnsafeBitmap bm = new UnsafeBitmap(_input);
			bm.LockBitmap();
			
			_redChannel = new byte[bm.Width, bm.Height];
			_greenChannel = new byte[bm.Width, bm.Height];
			_blueChannel = new byte[bm.Width, bm.Height];

			for (int i = 0; i < bm.Width; ++i) {
				for (int j = 0; j < bm.Height; ++j) {
					PixelData pd = bm.GetPixel(i, j);
					_blueChannel[i, j] = pd.B;
					_greenChannel[i, j] = pd.G;
					_redChannel[i, j] = pd.R;
				}
			}
			bm.UnlockBitmap();
		}

		public bool Decompress() {
			if (_redChannelData == null || _blueChannelData == null || _greenChannelData == null) {
				return false;
			}

			Console.WriteLine("Decompressing image data");

			Decompressor decompressor = new Decompressor();

			_redChannel = decompressor.Decompress(_redChannelData);

			_greenChannel= decompressor.Decompress(_greenChannelData);

			_blueChannel= decompressor.Decompress(_blueChannelData);

			Console.WriteLine("Tworzenie koñcowej bitmapy");
			Bitmap bmp = new Bitmap(_blueChannel.GetLength(0), _blueChannel.GetLength(1), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			UnsafeBitmap unsafeBitmap = new UnsafeBitmap(bmp);
			unsafeBitmap.LockBitmap();

			for (int i = 0; i < unsafeBitmap.Width; ++i) {
				for (int j = 0; j < unsafeBitmap.Height; ++j) {
					PixelData pd = new PixelData((byte)_redChannel[i, j], (byte)_greenChannel[i, j], (byte)_blueChannel[i, j]);
					unsafeBitmap.SetPixel(i, j, pd);
				}
			}
			unsafeBitmap.UnlockBitmap();

			Console.WriteLine("Tworzenie koñcowej bitmapy zakoñczone");
			_output = bmp;

			return true;
		}

		int getNewSize(int oldSize) {
			int bigDelta = Settings.Default.BigDelta;
			int newSize = 0;
			while (newSize < oldSize) {
				newSize += bigDelta;
			}
			newSize++;
			return newSize;
		}
	}
}
