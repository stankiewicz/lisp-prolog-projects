using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace KompresjaFraktalna {
	[Serializable]
	public class Region : Rectangle {

		private double contractivityFactor = Double.MaxValue;

		public double ContractivityFactor {
			get { return contractivityFactor; }
			set { contractivityFactor = value; }
		}

		private Params parameters;

		public Params Parameters {
			get { return parameters; }
			set { parameters = value; }
		}

		private int depth;

		/// <summary>
		/// g³êbokoœæ regionu
		/// </summary>
		public int Depth {
			get { return depth; }
			set { depth = value; }
		}

		[NonSerialized]
		private Domain domain;

		public Domain Domain {
			get { return domain; }
			set {
				domain = value;
				DomainIdx = domain.J;
			}
		}

		private int domainIdx;

		public int DomainIdx {
			get { return domainIdx; }
			set { domainIdx = value; }
		}

		public Region(int x, int y, int width, int height)
			: base(x, y, width, height) {
			Depth = 1;
		}

		/// <summary>
		/// generuje regiony. wierszowo. 
		/// zakladam ze width = k* side + 1 x height = l * side +1 , k,l Naturalne.
		/// </summary>
		/// <param name="side">delta</param>
		/// <param name="width">szerokosc obszaru</param>
		/// <param name="height">wysokosc obszaru</param>
		/// <returns>podzial na regiony</returns>
		public static LinkedList<Region> GenerateRegions(int smallDelta, int width, int height) {
			LinkedList<Region> regions = new LinkedList<Region>();

			for (int y = 0; y < height - 1; y += smallDelta) {
				for (int x = 0; x < width - 1; x += smallDelta) {
					Debug.Assert(y + smallDelta < height && x + smallDelta < width, "Niepoprawne generowanie regionów");

					Region r = new Region(x, y, smallDelta + 1, smallDelta + 1);
					regions.AddLast(r);
				}
			}
			return regions;
		}

		/// <summary>
		/// tworzy region z wypelnionymi danymi o kolorze
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <param name="bitmap"></param>
		/// <returns></returns>
		public static Region GenerateRegion(int x, int y, int width, int height) {
			Region r = new Region(x, y, width, height);
			return r;
		}

		internal void serialize(System.IO.BinaryWriter bw) {
			//bw.Write(this.X);
			//bw.Write(this.Y);
			//bw.Write(this.Width);
			//bw.Write(this.Height);
			//bw.Write(this.contractivityFactor);
			//bw.Write(this.domainIdx);
			//bw.Write(this.Depth);
			bw.Write((ushort)this.X);
			bw.Write((ushort)this.Y);
			//bw.Write((ushort)this.Width);
			//bw.Write((ushort)this.Height);
			bw.Write(this.contractivityFactor);
			bw.Write((ushort)this.domainIdx);
			bw.Write((byte)this.Depth);

			//parameters.serialize(bw);
		}

		public static Region deserialize(System.IO.BinaryReader br) {
			//int x = br.ReadInt32();
			//int y = br.ReadInt32();
			//int w = br.ReadInt32();
			//int h = br.ReadInt32();
			int x = br.ReadUInt16();
			int y = br.ReadUInt16();
			//int w = br.ReadUInt16(); // obliczane na podstawie smallDelta i Depth
			//int h = br.ReadUInt16(); // obliczane na podstawie smallDelta i Depth
			int w = 0, h = 0;
			Region r = new Region(x, y, w, h);
			r.ContractivityFactor = br.ReadDouble();
			r.DomainIdx = br.ReadUInt16();
			r.Depth = br.ReadByte();
			//r.Parameters = Params.deserialize(br);
			return r;
		}
	}
}
