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
        protected List<Region> GenerateRegions(int side, int width, int height) {
            List<Region> regions = new List<Region>();

            for (int y = 0; y < height; y += side)
                for (int x = 0; x < width; x += side) {
                    Region r = new Region(x, y, side, side, 0, 0, 0, 0);
                    regions.Add(r);
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
        protected List<Domain> GenerateDomains(int side, int width, int height) {
            List<Domain> domains = new List<Domain>();
            int j = 0;
            for (int y = 0; y < height; y += side)
                for (int x = 0; x < width; x += side) {
                    Domain d = new Domain(++j,x, y, side, side, 0, 0, 0, 0);
                    domains.Add(d);
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
        protected List<Point> GenerateInterpolationPoints(int side, int width, int height) {
            List<Point> points = new List<Point>();
            for (int y = 0; y < height; y += side)
                for (int x = 0; x < width; x += side) {
                    Point p = new Point(x, y);
                    points.Add(p);
                }
            return points;
        }
    }
}
