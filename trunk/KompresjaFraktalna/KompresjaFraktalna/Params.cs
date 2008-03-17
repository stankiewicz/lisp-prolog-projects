using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace KompresjaFraktalna {
	[Serializable]
	public class Params {
		double a, k, d, l, e, g, h, m;

		public double A {
			get { return a; }
			set { a = value; }
		}

		public double K {
			get { return k; }
			set { k = value; }
		}

		public double D {
			get { return d; }
			set { d = value; }
		}

		public double L {
			get { return l; }
			set { l = value; }
		}

		public double E {
			get { return e; }
			set { e = value; }
		}

		public double G {
			get { return g; }
			set { g = value; }
		}

		public double H {
			get { return h; }
			set { h = value; }
		}

		public double M {
			get { return m; }
			set { m = value; }
		}

		internal void serialize(System.IO.BinaryWriter bw) {
			bw.Write(this.a);
			bw.Write(this.d);
			bw.Write(this.e);
			bw.Write(this.g);
			bw.Write(this.h);
			bw.Write(this.k);
			bw.Write(this.l);
			bw.Write(this.m);
		}

		public static Params deserialize(BinaryReader br) {
			Params p = new Params();
			p.a = br.ReadDouble();
			p.d = br.ReadDouble();
			p.e = br.ReadDouble();
			p.g = br.ReadDouble();
			p.h = br.ReadDouble();
			p.k = br.ReadDouble();
			p.l = br.ReadDouble();
			p.m = br.ReadDouble();
			return p;
		}
	}
}
