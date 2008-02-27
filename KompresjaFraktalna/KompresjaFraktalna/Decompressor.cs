using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna {
    class Decompressor : Common {

        public int[,] Decompress(ChannelData sfk) {
            int _delta;
            int _Delta;
            int _dMax;
            Queue<Point> _IP;
            Queue<Region> _regions;

            int width, height;

            width = sfk.Width;
            height = sfk.Height;
            _dMax = sfk.DMax;
            _delta = sfk.SmallDelta;
            _Delta = sfk.BigDelta;
            //_CON = (List<double>)formatter.Deserialize(inputStream);
            _IP = new Queue<Point>( sfk.InterpolationPoints);
            //_AD = (List<Address>)formatter.Deserialize(inputStream);
            _regions = new Queue<Region>( sfk.Regions);

            int[,] image = new int[width, height];

            foreach (Point interpolationPoint in _IP) {
                image[interpolationPoint.X, interpolationPoint.Y] = interpolationPoint.Z;
            }

            double a = (double)_delta / (double)_Delta;
            double logdelta = Math.Log(_delta);
            double logA = Math.Log(a);
            int steps = (int)Math.Truncate(logdelta / logA);
			steps = Math.Abs(steps);

            //posortowaæ kolejkê
            for (int step = 1; step <= steps; step++) {
                foreach (Region r in _regions) {
                    if (r.Depth > step) continue;
					int domainX = r.Domain.Left;
					int domainY = r.Domain.Bottom;
                    double[] parameters = r.Parameters;
                    Domain domain = new Domain(-1, domainX, domainY, _Delta + 1, _Delta + 1, 0, 0, 0, 0);

                    double factor = r.ContractivityFactor;
                    int krok = (int)(_delta / (Math.Pow(a, step - 1)));
                    Map(krok, domain, r, image);

                }
            }
            return image;
        }


        private bool TryMapDomainToRegion(Domain domain, Region region, double s, int[,] bitmap, out double[] parameters) {

            parameters = new double[8];
            parameters[(int)param.a] = ((double)region.Size - 1) / ((double)domain.Size - 1);
            parameters[(int)param.k] = region.X - domain.X * parameters[(int)param.a];
            parameters[(int)param.d] = parameters[(int)param.a];
            parameters[(int)param.l] = region.Y - domain.Y * parameters[(int)param.a];
            double[] B = new double[4];
            double[,] A = new double[,] { 
                  { domain.Left, domain.Bottom, domain.Left * domain.Bottom, 1, bitmap[region.Left, region.Bottom] - s * bitmap[domain.Left, domain.Bottom] },
                  { domain.Right, domain.Bottom, (domain.Right)*(domain.Bottom), 1, bitmap[region.Right, region.Bottom] - s * bitmap[domain.Right, domain.Bottom]},
                  { domain.Left, domain.Top, domain.Left*(domain.Top), 1, bitmap[region.Left, region.Top] - s * bitmap[domain.Left, domain.Top]},
                  { domain.Right, domain.Top, (domain.Right)*(domain.Top), 1, bitmap[region.Right, region.Top] - s * bitmap[domain.Right, domain.Top]}
           };
            if (LinearEquationSolver.GaussianElimination(A, B)) {
                parameters[(int)param.e] = B[0];
                parameters[(int)param.g] = B[1];
                parameters[(int)param.h] = B[2];
                parameters[(int)param.m] = B[3];
                return true;
            } else
                throw new Exception("Macierz osobliwa");




        }

        private void Map(int krok, Domain domain, Region region, int[,] bitmap) {

            double[] parameters;

            if (TryMapDomainToRegion(domain, region, region.ContractivityFactor, bitmap, out parameters) == false) {
                throw new NotImplementedException("dupa bladaa");
            }

            for (int x = domain.Left; x <= domain.Right; x += krok) {
                for (int y = domain.Bottom; y <= domain.Top; y += krok) {

                    Point mapped = mapPoint(new Point(x, y, bitmap[x, y]), parameters, region.ContractivityFactor);
                    bitmap[mapped.X, mapped.Y] = mapped.Z;
                }
            }
        }

        private Point mapPoint(Point from, double[] parameters, double factor) {

            int z = from.Z;
            double xm = parameters[(int)param.a] * from.X + parameters[(int)param.k];
            double ym = parameters[(int)param.d] * from.Y + parameters[(int)param.l];
            double zm = parameters[(int)param.e] * from.X + parameters[(int)param.g] * from.Y + parameters[(int)param.h] * from.X * from.Y + factor * from.Z + parameters[(int)param.m];

            return new Point((int)xm, (int)ym, (int)zm);
        }
    }
}
