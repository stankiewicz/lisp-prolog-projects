using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace KompresjaFraktalna {
	class FractalCompressor {

		int orgWidth, orgHeight;
		Bitmap input, output;

		byte[,] redChannel, greenChannel, blueChannel;

		ChannelData redChannelData, blueChannelData, greenChannelData;

		/// <summary>
		/// Jakiœ obiekt
		/// </summary>
		Object compressedData;

		public Bitmap Input {
			set {
				//zapamiêtujemy rozmiar oryginalnej bitmapy
				orgWidth = value.Width;
				orgHeight = value.Height;

				//tworzymy now¹ bitmapê dla tego por¹banego algorytmu
				int newWidth = getNewSize(orgWidth);
				int newHeight = getNewSize(orgHeight);
				input = new Bitmap(newWidth, newHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

				Graphics g = Graphics.FromImage(input);
				g.Clear(Color.White);
				g.DrawImageUnscaled(value, 0, 0);
				g.Dispose();
			}
		}

		public Bitmap Output {
			get {
				return output;
			}
		}

		public void Compress() {
			if (input == null) {
				throw new InvalidOperationException("Nie podano bitmapy");
			}

			prepareChannels();

			Compressor compressor = new Compressor();
			
			Console.WriteLine("Compressing red channel");
			redChannelData = compressor.Compress(redChannel);
			Console.WriteLine("Red channel compressed");

			Console.WriteLine("Compressing green channel");
			greenChannelData = compressor.Compress(greenChannel);
			Console.WriteLine("Green channel compressed");

			Console.WriteLine("Compressing blue channel");
			blueChannelData = compressor.Compress(blueChannel);
			Console.WriteLine("Blue channel compressed");

			ChannelViewer cv = new ChannelViewer();
			cv.ChannelDataR = redChannelData;
			cv.ChannelDataG = greenChannelData;
			cv.ChannelDataB = blueChannelData;
			cv.redrawBitmap();
			cv.ShowDialog();
		}

		private void prepareChannels() {
			UnsafeBitmap bm = new UnsafeBitmap(input);
			bm.LockBitmap();
			
			redChannel = new byte[bm.Width, bm.Height];
			greenChannel = new byte[bm.Width, bm.Height];
			blueChannel = new byte[bm.Width, bm.Height];

			for (int i = 0; i < bm.Width; ++i) {
				for (int j = 0; j < bm.Height; ++j) {
					PixelData pd = bm.GetPixel(i, j);
					blueChannel[i, j] = pd.B;
					greenChannel[i, j] = pd.G;
					redChannel[i, j] = pd.R;
				}
			}
			bm.UnlockBitmap();
		}

		public void Decompress() {
			Console.WriteLine("Decompressing image data");

			Decompressor decompressor = new Decompressor();

			redChannel = decompressor.Decompress(redChannelData);

			greenChannel= decompressor.Decompress(greenChannelData);

			blueChannel= decompressor.Decompress(blueChannelData);

			Console.WriteLine("Tworzenie koñcowej bitmapy");
			Bitmap bmp = new Bitmap(blueChannel.GetLength(0), blueChannel.GetLength(1), System.Drawing.Imaging.PixelFormat.Format24bppRgb);
			UnsafeBitmap unsafeBitmap = new UnsafeBitmap(bmp);
			unsafeBitmap.LockBitmap();

			for (int i = 0; i < unsafeBitmap.Width; ++i) {
				for (int j = 0; j < unsafeBitmap.Height; ++j) {
					PixelData pd = new PixelData((byte)redChannel[i, j], (byte)greenChannel[i, j], (byte)blueChannel[i, j]);
					unsafeBitmap.SetPixel(i, j, pd);
				}
			}
			unsafeBitmap.UnlockBitmap();

			Console.WriteLine("Tworzenie koñcowej bitmapy zakoñczone");
			output = bmp;
		}

		public Object CompressedData {
			get {
				return compressedData;
			}

			set {
				compressedData = value;
			}
		}

		int getNewSize(int oldSize) {
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
