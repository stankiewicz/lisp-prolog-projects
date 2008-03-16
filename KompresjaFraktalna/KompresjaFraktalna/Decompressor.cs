using System;
using System.Collections.Generic;
using System.Text;
using KompresjaFraktalna.utils;

namespace KompresjaFraktalna {
    class Decompressor {

        public byte[,] Decompress(ChannelData cd) {
            int _delta;
            int _Delta;
            int _dMax;
            Queue<Point> _IP;
            Queue<Region> _regions;

            int width, height;

            width = cd.Width;
            height = cd.Height;
            _dMax = cd.DMax;
            _delta = cd.SmallDelta;
            _Delta = cd.BigDelta;
            //_CON = (List<double>)formatter.Deserialize(inputStream);
            _IP = new Queue<Point>( cd.InterpolationPoints);
            //_AD = (List<Address>)formatter.Deserialize(inputStream);
            _regions = new Queue<Region>( cd.Regions);

            byte[,] image = new byte[width, height];

            foreach (Point interpolationPoint in _IP) {
                image[interpolationPoint.X, interpolationPoint.Y] = (byte)interpolationPoint.Z;
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
                    Params parameters = r.Parameters;
                    Domain domain = new Domain(-1, domainX, domainY, _Delta + 1, _Delta + 1, 0, 0, 0, 0);

                    double factor = r.ContractivityFactor;
                    int krok = (int)((double)_delta / (Math.Pow(a, -(step - 1))));
                    Map(krok, r.Domain, r, image);

                }
            }
            return image;
        }


        

        private void Map(int krok, Domain domain, Region region, byte[,] bitmap) {

            Params parameters;

            if (Common.TryMapDomainToRegion(domain, region, region.ContractivityFactor, bitmap, out parameters) == false) {
                throw new NotImplementedException("dupa bladaa");
            }

            for (int x = domain.Left; x <= domain.Right; x += krok) {
                for (int y = domain.Bottom; y <= domain.Top; y += krok) {

                    Point mapped = mapPoint(new Point(x, y, bitmap[x, y]), parameters, region.ContractivityFactor);
                    bitmap[mapped.X, mapped.Y] = (byte)mapped.Z;
                }
            }
        }

        private Point mapPoint(Point from, Params parameters, double factor) {

            int z = from.Z;
            double xm = parameters.A * from.X + parameters.K;
            double ym = parameters.D * from.Y + parameters.L;
            double zm = parameters.E * from.X + parameters.G * from.Y + parameters.H * from.X * from.Y + factor * from.Z + parameters.M;
            if (zm < 0) zm = 0;
            if (zm > 255) zm = 255;
            return new Point((int)xm, (int)ym, (int)zm);
        }
    }
}
