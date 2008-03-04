using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace KompresjaFraktalna {
    [Serializable]
    public class Rectangle {
    
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
                    new Point(Left,Bottom,_x00),
                    new Point(Right,Bottom,_x01),
                    new Point(Left,Top,_x10),
                    new Point(Right,Top,_x11)
                };
            }
        }

        public int Left {
            get {
                return _x;
            }
        }

        public int Right {
            get {
                return _x + _width - 1;
            }
        }

        public int Top {
            get {
                return _y + _height -1;
            }
        }

        public int Bottom {
            get {
                return _y;
            }
        }
        
        public int X {
            get { return _x; }
        }
        
        public int Y {
            get { return _y; }
        }

        public int Size {
            get { return _width; }
        }
        
        public int Width {
            get { return _width; }
        }
        
        public int Height {
            get { return _height; }
        }
    }
}
