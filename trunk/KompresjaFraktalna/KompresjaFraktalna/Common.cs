using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna {
    class Common {

        /// <summary>
        /// generuje regiony. wierszowo. 
        /// zakladam ze width = k* side + 1 i height = l * side +1 , k,l Naturalne.
        /// </summary>
        /// <param name="side">dlugosc boku</param>
        /// <param name="width">szerokosc obszaru</param>
        /// <param name="height">wysokosc obszaru</param>
        /// <returns>podzial na regiony</returns>
        protected Queue<Region> GenerateRegions(int side, int width, int height) {
            Queue<Region> regions = new Queue<Region>();

			for (int y = 0; y < height; y += side) {
				for (int x = 0; x < width; x += side) {
					Region r = new Region(x, y, side, side, 0, 0, 0, 0);
					regions.Enqueue(r);
				}
			}
            return regions;
        }

        /// <summary>
        /// generuje domeny. wierszowo. zakladam ze width = k* side +1 i height = l * side +1 , k,l Naturalne.
        /// </summary>
        /// <param name="side">dlugosc boku</param>
        /// <param name="width">szerokosc obszaru</param>
        /// <param name="height">wysokosc obszaru</param>
        /// <returns>podzial na domeny</returns>
		protected Queue<Domain> GenerateDomains(int side, int width, int height) {
			Queue<Domain> domains = new Queue<Domain>();

            int j = 0;
			for (int y = 0; y < height; y += side) {
				for (int x = 0; x < width; x += side) {
					Domain d = new Domain(++j, x, y, side, side, 0, 0, 0, 0);
					domains.Enqueue(d);
				}
			}
            return domains;
        }

        /// <summary>
        /// generuje punkty interpolacji. zakladam ze width = k* side +1 i height = l * side +1 , k,l Naturalne.
        /// </summary>
        /// <param name="side">dlugosc boku</param>
        /// <param name="width">szerokosc obszaru</param>
        /// <param name="height">wysokosc obszaru</param>
        /// <returns>podzial na domeny</returns>
        protected Queue<Point> GenerateInterpolationPoints(int side, int width, int height) {
            Queue<Point> points = new Queue<Point>();
            for (int y = 0; y < height; y += side)
                for (int x = 0; x < width; x += side) {
                    Point p = new Point(x, y);
                    points.Enqueue(p);
                }
            return points;
        }
    }
}
