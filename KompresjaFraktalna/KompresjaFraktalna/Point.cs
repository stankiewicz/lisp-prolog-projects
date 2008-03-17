using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace KompresjaFraktalna {
    [Serializable]
    public class Point {

        private int _x;
        private int _y;
        private int _z;

        public Point(int x, int y, int z) {
            _x = x;
            _y = y;
            _z = z;
        }
        
        public int X {
            get { return _x; }
        }
        
        public int Y {
            get { return _y; }
        }

        public int Z {
            get { return _z; }
            set { _z = value; }
        }
    }
}
