using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna {
    [Serializable]
    class Domain:Rectangle {
        public Domain(int j, int x, int y, int width, int height, int x00, int x10, int x01, int x11)
            : base(x, y, width, height,x00,x10,x01,x11) {
            _j = j;
        }
        
        private int _j;
        public int J {
            get { return _j; }
        }

    }
}
