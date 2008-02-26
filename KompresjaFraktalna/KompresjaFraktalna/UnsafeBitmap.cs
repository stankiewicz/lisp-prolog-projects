using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace KompresjaFraktalna {

    public unsafe class UnsafeBitmap {
        Bitmap bitmap;

        private int pixels;



        int width;
        BitmapData bitmapData = null;
        Byte* pBase = null;

        public UnsafeBitmap(Bitmap bitmap) {
            this.bitmap = bitmap;
            if (bitmap.PixelFormat == PixelFormat.Format24bppRgb) {
                pixels = 3;
            } else if (bitmap.PixelFormat == PixelFormat.Format32bppArgb) {
                pixels = 4;
			} else {
				throw new Exception("Nieobs³ugiwany format bitmapy: " + bitmap.PixelFormat);
            }
        }

        public UnsafeBitmap(int width, int height) {
            this.bitmap = new Bitmap(width, height);
        }

        public UnsafeBitmap(UnsafeBitmap ub) {
            this.bitmap = ub.Bitmap.Clone() as Bitmap;
        }

        public void Dispose() {
            bitmap.Dispose();
        }

        public Bitmap Bitmap {
            get {
                return (bitmap);
            }
        }

        public int Width {
            get { return this.Bitmap.Width; }
        }

        public int Height {
            get { return this.Bitmap.Height; }
        }

        private Point PixelSize {
            get {
                GraphicsUnit unit = GraphicsUnit.Pixel;
                RectangleF bounds = bitmap.GetBounds(ref unit);

                return new Point((int)bounds.Width, (int)bounds.Height);
            }
        }

        public void LockBitmap() {
            GraphicsUnit unit = GraphicsUnit.Pixel;
            RectangleF boundsF = bitmap.GetBounds(ref unit);
            System.Drawing.Rectangle bounds = new System.Drawing.Rectangle((int)boundsF.X, (int)boundsF.Y, (int)boundsF.Width, (int)boundsF.Height);

            bitmapData = bitmap.LockBits(bounds, ImageLockMode.ReadWrite, bitmap.PixelFormat);
            width = bitmapData.Stride;

            pBase = (Byte*)bitmapData.Scan0.ToPointer();
        }

        public PixelData GetPixel(int x, int y) {
            PixelData returnValue = *PixelAt(x, y);
            return returnValue;
        }

        public void SetPixel(int x, int y, PixelData colour) {
            PixelData* pixel = PixelAt(x, y);
            *pixel = colour;
        }

        public void UnlockBitmap() {
            bitmap.UnlockBits(bitmapData);
            bitmapData = null;
            pBase = null;
        }
        public PixelData* PixelAt(int x, int y) {
            return (PixelData*)(pBase + y * width + x * pixels * sizeof(byte));
        }
    }

    public struct PixelData {
        public byte B;
        public byte G;
        public byte R;

        public PixelData(byte r, byte g, byte b) {
            this.R = r;
            this.G = g;
            this.B = b;
        }

        public static bool operator ==(PixelData pd1, PixelData pd2) {
            return (pd1.B == pd2.B && pd1.G == pd2.G && pd1.R == pd2.R);
        }

        public static bool operator !=(PixelData pd1, PixelData pd2) {
            return !(pd1 == pd2);
        }

        public override bool Equals(object obj) {
            PixelData pd;
            try {
                pd = (PixelData)obj;
            } catch {
                return false;
            }

            if (pd.B == this.B && pd.G == this.G && pd.R == this.R) return true;
            else return false;
        }

        public override int GetHashCode() {
            return base.GetHashCode();
        }
    }
}
