using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace KompresjaFraktalna {
    [Serializable]
    class Rectangle {
    
        private int _x;
        private int _y;
        private int _height;
        private int _width;

        int _x00, _x10, _x01, _x11;

        public Rectangle(int x, int y, int width, int height, int x00,int x10,int x01,int x11) {
            this._x = x;
            this._y = y;
            this._width = width;
            this._height = height;
            _x00 = x00;
            _x10 = x10;
            _x01 = x01;
            _x11 = x11;
        }

        public Point[] Points {
            get {
                return new Point[] {
                    new Point(X,Y,_x00),
                    new Point(X+Width,Y,_x01),
                    new Point(X,Y+Height,_x10),
                    new Point(X+Width,Y+Height,_x11)
                };
            }
        }
        
        public int X {
            get { return _x; }
        }
        
        public int Y {
            get { return _y; }
        }
        
        public int Width {
            get { return _width; }
        }
        
        public int Height {
            get { return _height; }
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context) {
            
        }

        #endregion
    }
}
