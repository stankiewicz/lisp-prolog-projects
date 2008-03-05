using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna {
    [Serializable]
    public class ChannelData {
        private Region[] regions;

        public Region [] Regions {
            get { return regions; }
            set { regions = value; }
        }
        private Point [] ip;

        public Point [] InterpolationPoints {
            get { return ip; }
            set { ip = value; }
        }

        private int smallDelta;

        public int SmallDelta {
            get { return smallDelta; }
            set { smallDelta = value; }
        }

        private int bigDelta;

        public int BigDelta {
            get { return bigDelta; }
            set { bigDelta = value; }
        }

        private int width;

        public int Width {
            get { return width; }
            set { width = value; }
        }

        private int height;

        public int Height {
            get { return height; }
            set { height = value; }
        }

        private int dMax;

        public int DMax {
            get { return dMax; }
            set { dMax = value; }
        }
    }
}
