using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace KompresjaFraktalna {
    [Serializable]
    public class Domain:Rectangle {
		public Domain(int j, int x, int y, int width, int height)
			: base(x, y, width, height) {
			_j = j;
		}
        
        private int _j;
        public int J {
            get { return _j; }
        }

		/// <summary>
		/// generuje domeny. wierszowo. zakladam ze width = k* side +1 x height = l * side +1 , k,l Naturalne.
		/// </summary>
		/// <param name="side">Delta</param>
		/// <param name="width">szerokosc obszaru</param>
		/// <param name="height">wysokosc obszaru</param>
		/// <returns>podzial na domeny</returns>
		public static Queue<Domain> GenerateDomains(int bigDelta, int width, int height) {
			Queue<Domain> domains = new Queue<Domain>();

			int j = 0;
			for (int y = 0; y < height - 1; y += bigDelta) {
				for (int x = 0; x < width - 1; x += bigDelta) {
					Debug.Assert(y + bigDelta < height && x + bigDelta < width, "Niepoprawne generowanie domen");

					Domain d = new Domain(j++, x, y, bigDelta + 1, bigDelta + 1);
					domains.Enqueue(d);
				}
			}
			return domains;
		}

		public static Domain fromIndex(int idx, int bigDelta, int bitmapWidth, int bitmapHeight) {
			int xCount = (bitmapWidth - 1) / bigDelta;
			int yCount = (bitmapHeight - 1) / bigDelta;
			int xIdx = idx % xCount;
			int yIdx = idx / xCount;
			Domain d = new Domain(idx, xIdx * bigDelta, yIdx * bigDelta, bigDelta + 1, bigDelta + 1);
			return d;
		}
    }
}
