using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

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

		public void serialize(BinaryWriter bw) {
			//bw.Write(this.BigDelta);
			//bw.Write(this.DMax);
			//bw.Write(this.Height);
			//bw.Write(this.SmallDelta);
			//bw.Write(this.Width);

			//bw.Write(this.InterpolationPoints.Length);
			//foreach (Point p in InterpolationPoints) {
			//    p.serialize(bw);
			//}

			//bw.Write(this.Regions.Length);
			//foreach (Region r in Regions) {
			//    r.serialize(bw);
			//}

			bw.Write((ushort)this.BigDelta);
			bw.Write((byte)this.DMax);
			bw.Write((ushort)this.Height);
			bw.Write((ushort)this.SmallDelta);
			bw.Write((ushort)this.Width);

			bw.Write((ushort)this.InterpolationPoints.Length);
			foreach (Point p in InterpolationPoints) {
				p.serialize(bw);
			}

			Debug.WriteLine("ChannelData:InterpolationPoint.Length=" + InterpolationPoints.Length);

			bw.Write((ushort)this.Regions.Length);

			Debug.WriteLine("ChannelData:Regions.Length=" + regions.Length);

			foreach (Region r in Regions) {
				r.serialize(bw);
			}
		}

		public static ChannelData deserialize(BinaryReader br) {
			ChannelData cd = new ChannelData();
			//cd.BigDelta = br.ReadInt32();
			//cd.DMax = br.ReadInt32();
			//cd.height = br.ReadInt32();
			//cd.smallDelta = br.ReadInt32();
			//cd.width = br.ReadInt32();
			cd.BigDelta = br.ReadUInt16();
			cd.DMax = br.ReadByte();
			cd.height = br.ReadUInt16();
			cd.smallDelta = br.ReadUInt16();
			cd.width = br.ReadUInt16();

			int ipLen = br.ReadUInt16();

			Point[] ips = new Point[ipLen];
			for (int i = 0; i < ipLen; i++) {
				ips[i] = Point.deserialize(br);
			}
			cd.InterpolationPoints = ips;

			int regLen = br.ReadUInt16();
			Region[] regs = new Region[regLen];
			for (int i = 0; i < regLen; i++) {
				regs[i] = Region.deserialize(br);
			}
			cd.Regions = regs;
			return cd;
		}
    }
}
