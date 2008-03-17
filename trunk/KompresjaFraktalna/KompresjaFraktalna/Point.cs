using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

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

		internal void serialize(System.IO.BinaryWriter bw) {
			//bw.Write(_x);
			//bw.Write(_y);
			//bw.Write(_z);
			bw.Write((ushort)_x);
			bw.Write((ushort)_y);
			bw.Write((ushort)_z);
		}

		public static Point deserialize(BinaryReader br) {
			//int x = br.ReadInt32();
			//int y = br.ReadInt32();
			//int z = br.ReadInt32();
			int x = br.ReadUInt16();
			int y = br.ReadUInt16();
			int z = br.ReadUInt16();
			return new Point(x, y, z);
		}
	}
}
