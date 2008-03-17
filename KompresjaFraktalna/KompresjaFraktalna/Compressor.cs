using System;
using System.Collections.Generic;
using System.Text;
using KompresjaFraktalna.utils;
using KompresjaFraktalna.Properties;
using System.Diagnostics;

#region Step 1)
/*
* Punkt 1. choose values for delta and Delta, such that Delta = a * delta. 
* Choose also, error tolerance Epsilon and maximum depth Dmax.
*/
#endregion

namespace KompresjaFraktalna {
	class Compressor {

		byte[,] bitmap;

		/// <summary>
		/// generuje regiony. wierszowo. 
		/// zakladam ze width = k* side + 1 x height = l * side +1 , k,l Naturalne.
		/// </summary>
		/// <param name="side">delta</param>
		/// <param name="width">szerokosc obszaru</param>
		/// <param name="height">wysokosc obszaru</param>
		/// <returns>podzial na regiony</returns>
		protected LinkedList<Region> GenerateRegions(int side, int width, int height) {
			LinkedList<Region> regions = new LinkedList<Region>();

			for (int y = 0; y < height - 1; y += side) {
				for (int x = 0; x < width - 1; x += side) {
					Debug.Assert(y + side < height && x + side < width, "Niepoprawne generowanie regionów");

					//Region r = new Region(x, y, side + 1, side + 1, 0, 0, 0, 0);
					Region r = new Region(x, y, side + 1, side + 1);
					r.Depth = 1;
					regions.AddLast(r);
				}
			}
			return regions;
		}

		/// <summary>
		/// generuje domeny. wierszowo. zakladam ze width = k* side +1 x height = l * side +1 , k,l Naturalne.
		/// </summary>
		/// <param name="side">Delta</param>
		/// <param name="width">szerokosc obszaru</param>
		/// <param name="height">wysokosc obszaru</param>
		/// <returns>podzial na domeny</returns>
		protected Queue<Domain> GenerateDomains(int side, int width, int height) {
			Queue<Domain> domains = new Queue<Domain>();

			int j = 0;
			for (int y = 0; y < height - 1; y += side) {
				for (int x = 0; x < width - 1; x += side) {
					Debug.Assert(y + side < height && x + side < width, "Niepoprawne generowanie domen");

					//Domain d = new Domain(j++, x, y, side + 1, side + 1, 0, 0, 0, 0);
					Domain d = new Domain(j++, x, y, side + 1, side + 1);
					domains.Enqueue(d);
				}
			}
			return domains;
		}


		/// <summary>
		/// generuje punkty interpolacji. zakladam ze width = k* side +1 x height = l * side +1 , k,l Naturalne.
		/// </summary>
		/// <param name="side">dlugosc boku</param>
		/// <param name="width">szerokosc obszaru</param>
		/// <param name="height">wysokosc obszaru</param>
		/// <returns>podzial na domeny</returns>
		protected Queue<Point> GenerateInterpolationPoints(int width, int height, byte[,] bitmap) {
			Queue<Point> points = new Queue<Point>();

			for (int y = 0; y < height; y += Settings.Default.SmallDelta)
				for (int x = 0; x < width; x += Settings.Default.SmallDelta) {
					Point p = new Point(x, y, bitmap[x, y]);
					points.Enqueue(p);
				}
			return points;
		}

		private double Distance(Domain domain, Region region, Params parameters) {
			double hij = 0;
			int delta = (domain.Width - 1) / (region.Width - 1);

			byte[,] mappedRegion = new byte[region.Width, region.Height];

			double xm = parameters.A * domain.Left + parameters.K;
			double xmDelta = delta * parameters.A;

			for (int x = domain.Left; x <= domain.Right; x += delta, xm += xmDelta) {
				double ym = parameters.D * domain.Bottom + parameters.L;
				double ymDelta = delta * parameters.D;

				for (int y = domain.Bottom; y <= domain.Top; y += delta, ym += ymDelta) {
					double z = bitmap[x, y];

					double zm = parameters.E * x + parameters.G * y + parameters.H * x * y + region.ContractivityFactor * z + parameters.M;

					if (zm < 0) {
						zm = 0;
					} else if (zm > 255) {
						zm = 255;
					}

					mappedRegion[(int)Math.Round(xm) - region.Left, (int)Math.Round(ym) - region.Bottom] = (byte)zm;
				}
			}

			for (int x = 0; x < region.Width; ++x) {
				for (int y = 0; y < region.Height; ++y) {
					hij += Math.Abs(bitmap[region.X + x, region.Y + y] - mappedRegion[x, y]);
				}
			}

			return hij / ((double)region.Width * (double)region.Width);
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
		private Region GenerateRegion(int x, int y, int width, int height, byte[,] bitmap) {
			//Region r = new Region(x, y, width, height, bitmap[x, y], bitmap[x + width - 1, y], bitmap[x, y + height - 1], bitmap[x + width - 1, y + height - 1]);
			Region r = new Region(x, y, width, height);
			return r;
		}


		/// <summary>
		/// //TODO: poprawiæ
		/// </summary>
		/// <param name="domain"></param>
		/// <param name="region"></param>
		/// <returns></returns>
		private bool CheckConditionOfContinuity(Domain domain, Region region, Region[,] regions, int delta) {

			if (region.Bottom == 0 && region.Left == 0) {
				return true;
			}

			do {

				if (region.Left != 0) {
					// sprawdzamy lewy region
					Region left = regions[region.X - region.Width + 1, region.Y];
					if (left == null) {
						break;
					}

					// hack me
					if (left.Depth != region.Depth) {
						break;
					}

					// liczymy condition of continuity
					Domain leftDomain = left.Domain;
					if (leftDomain == null) {
						break;
					}
					double contrFactorLeft = left.ContractivityFactor;
					if (contrFactorLeft == Double.MaxValue) {
						break;
					}

					Double contrFactorRight = region.ContractivityFactor;

					// sprawdzamy prawa krawedz lewej domeny z lewa krawedzia domeny
					for (int i = delta; i < leftDomain.Height; i += delta) {
						// contrleft

						// contrRitfht

						// valDown
						int valLeft = bitmap[leftDomain.Right, i + leftDomain.Y];

						//valUp
						int valRight = bitmap[domain.X, i + domain.Y];

						double tmp1 = distanceFromLine(leftDomain.Bottom,
							bitmap[leftDomain.Right, leftDomain.Bottom], leftDomain.Top,
							bitmap[leftDomain.Right, leftDomain.Top],
							leftDomain.Bottom + i, bitmap[leftDomain.Right, leftDomain.Bottom + i]);
						double tmp2 = distanceFromLine(domain.Bottom,
							bitmap[domain.Left, domain.Bottom], domain.Top,
							bitmap[domain.Left, domain.Top],
							domain.Bottom + i, bitmap[domain.Left, domain.Bottom + i]);

						double? diff = contrFactorLeft * tmp1 - contrFactorRight * tmp2;
						if (Math.Abs(diff.Value) > Settings.Default.Epsilon) {
							return false;
						}
					}
				}
			} while (false);

			if (region.Bottom != 0) {
				// sprawdzamy lewy region
				Region down = regions[region.Left, region.Bottom - region.Height + 1];
				if (down == null) {
					return true;
				}

				// hack me
				if (down.Depth != region.Depth) {
					return true;
				}

				// liczymy condition of continuity
				Domain downDomain = down.Domain;
				if (downDomain == null) {
					return true;
				}
				double contrFactorDown = down.ContractivityFactor;
				if (contrFactorDown == Double.MaxValue) {
					return true;
				}
				Double contrFactorUp = region.ContractivityFactor;

				// sprawdzamy prawa krawedz lewej domeny z lewa krawedzia domeny
				for (int i = delta; i < downDomain.Width; i += delta) {


					// valDown
					int valDown = bitmap[downDomain.Left + i, downDomain.Top];

					//valUp
					int valUp = bitmap[domain.X + i, domain.Bottom];

					double tmp1 = distanceFromLine(downDomain.Left,
						bitmap[downDomain.Left, downDomain.Top], downDomain.Right,
						bitmap[downDomain.Right, downDomain.Top],
						downDomain.Left + i, bitmap[downDomain.Left + i, downDomain.Top]);
					double tmp2 = distanceFromLine(domain.Left,
						bitmap[domain.Left, domain.Bottom], domain.Right,
						bitmap[domain.Right, domain.Bottom],
						domain.Left + i, bitmap[domain.Left + i, domain.Bottom]);

					double diff = contrFactorDown * tmp1 - contrFactorUp * tmp2;
					if (Math.Abs(diff) > Settings.Default.Epsilon) {
						return false;
					}
				}
			}

			return true;
		}

		private double distanceFromLine(double x1, double z1, double x2, double z2, double xt, double zt) {
			double a = (z2 - z1) / (x2 - x1);
			return Math.Abs(a * xt + z1 - zt);
		}

		private double ComputeMeanDistance(Rectangle rect, byte[,] bitmap) {

			double a;
			double distances = 0;


			for (int row = rect.Y; row < rect.Y + rect.Height; ++row) {

				double dZ = bitmap[rect.Right, row] - bitmap[rect.X, row];
				a = dZ / (double)rect.Width;

				double aIter = a * rect.X;

				for (int x = rect.X; x < rect.X + rect.Width; ++x) {

					aIter += a;
					distances += Math.Abs(aIter - (double)bitmap[x, row]);
					// aIter wartosc w punkcie
				}
			}

			distances /= (double)(rect.Height * rect.Width);

			return distances;
		}

		private double ComputeContractivityFactor(Domain domain, Region region, byte[,] bitmap) {
			double mi, ni;

			// mi - domena
			mi = ComputeMeanDistance(domain, bitmap);

			// ni - region
			ni = ComputeMeanDistance(region, bitmap);

			return ni / mi;
		}

		/// <summary>
		/// Kompresja jednej skladowej koloru.
		/// </summary>
		/// <param name="bitmap">tablica z wartosciami skladowej koloru</param>
		/// <param name="outputStream">strumien do ktorego zostanie zapisane wszystko</param>
		public ChannelData Compress(byte[,] bitmap) {
			/*
			 * realizacja algorytmu ze strony 6.
			 */
			this.bitmap = bitmap;
			int width = bitmap.GetLength(0);
			int height = bitmap.GetLength(1);

			#region Step 2)
			/*
             * punkt 2. create two queues, one named 'squeue' and put all the regions inside as well 
             * as a queue named 'iqueue' and put all initial interpolation points inside.
             * In addition create two empty queues named 'cqueue' and 'aqueue' (we store contractivity factors in
             * the first and the addresses in the latter). set the depth d = 1 and create queue named 'squeue2'.
             */

			Console.WriteLine("Generowanie regionów");
			LinkedList<Region> regionsToProcess = GenerateRegions(Settings.Default.SmallDelta, width, height);

			Console.WriteLine("Generowanie punktów interpolacji");
			Queue<Point> interpolationPoints = GenerateInterpolationPoints(width, height, bitmap);

			Console.WriteLine("Inicjowanie kolejek i tablicy regionów");
			Region[,] regionsMap = new Region[width, height];
			Queue<Region> regionsProcessed = new Queue<Region>();

			foreach (Region reg in regionsToProcess) {
				regionsMap[reg.Left, reg.Bottom] = reg;
			}

			Console.WriteLine("Generowanie domen");
			Queue<Domain> domains = GenerateDomains(Settings.Default.BigDelta, width, height);

			#endregion

			Console.WriteLine("Start kompresji");
			#region Step 3)
			/*
                 * Punkt 3. 
                 * while 'squeue' is not empty do:
                 */

			while (regionsToProcess.Count != 0) {
				/*
				 * 3.a: 
				 * get one region from squeue
				 */
				Region region = regionsToProcess.First.Value;
				regionsToProcess.RemoveFirst();

				double minHij = Double.MaxValue;
				Params minParams = null;
				Domain minDomain = null;
				double minContrFactor = Double.MaxValue;

				bool passedContractivity = false;
				bool passedContinuity = false;

				//bool firstRun = true;
				bool alreadyFoundMatching = false;

				foreach (Domain domain in domains) {
					/*
					 * 3.b.x. 
					 * compute the contractivity factor for the map acciociated with the y-th domain and the region.
					 */
					double contractivityFactor = ComputeContractivityFactor(domain, region, bitmap);

					/*
					 * 3.b.ii. 
					 * if |s| >= 1 go to 3.b.v . otherwise chec the condition of continuity. if it doesn't hold, go to 3.b.v
					 */
					if (Math.Abs(contractivityFactor) < 1) {
						passedContractivity = true;
					} else if (alreadyFoundMatching) {
						continue;
					}

					region.ContractivityFactor = contractivityFactor;
					if (CheckConditionOfContinuity(domain, region, regionsMap, Settings.Default.SmallDelta) == true) {
						passedContinuity = true;
					} else if (alreadyFoundMatching) {
						continue;
					}

					/*
					 * 3.b.iii.
					 * Compute the other parameters and map the points of the y-th domain (say Dj) through 'w' according
					 * to the 'mapping algorithm'. say w(Dj) the emerging set.
					 */


					/*
					 * The Mapping Algorithm: We map the domain Dj to the region R.
					 * 
					 * MA:1. Put the endpoints of the region R to the new set w(Dj)
					 * 
					 * MA:2. Compute the other parameters. Let a = delta/Delta, where delta,Delta are the side of the region
					 * and the corresponding domain, respectively.
					 */

					Params parameters;
					if (Common.TryMapDomainToRegion(domain, region, contractivityFactor, bitmap, out parameters)) {
						minParams = parameters;
					} else {
						throw new Exception("moja glowa!! brak rozwiazania");
					}

					/*
					 * MA:3. Map the points of the first and last row (of Dj) as follows. The first and the last point of these
					 * rows of Dj are interpolation points and the have been already mapped. Map the (a+1)-th point of the domain
					 * to the 2nd point of the w(Dj). Continue by mapping the (2a +1), (3a + 1),.. point of the domain to the
					 * 3rd,rth ... point of the w(Dj)
					 * 
					 * MA:4. For all other rows: Map the (a+1),(2a+1),.. point of the domain to the 1st,2nd.. point of w(Dj)
					 * 
					 * to wszystko bedzie potrzebne do wyliczenia odleglosci hij. gdzies trzeba zapamietac te punkty.
					 */


					/*
					 * 3.b.iv.
					 * compute (with a proper distance measure) the distance hij between w(Dj) and the points of region x.
					 */
					double hij = Distance(domain, region, parameters);

					/*
					 * trzeba wszystko to zapamietac jesli hij jest mniejsze
					 */
					if (hij < minHij || !alreadyFoundMatching) {
						minDomain = domain;
						minHij = hij;
						minContrFactor = contractivityFactor;
						minParams = parameters; //?
					}
					/*
					 * 3.b.v.
					 * next y.
					 */

					//firstRun = false;
					alreadyFoundMatching = alreadyFoundMatching || (passedContinuity && passedContractivity);
				}

				Debug.Assert(minDomain != null, "Nie uda³o siê dobraæ domeny do regionu. Wtf?!");

				/*
				 * Punkt 3.c
				 * Find the y for which hij is a minimum
				 */

				//minimum - juz mamy

				/*
				 * Punkt 3.d
				 * If hij > Epsilon and d < dmax 
				 * then create four new regions, add them to squeue2, add the vertices of each new region to iqueue (as additional interpolation points) 
				 * and add 0 (?dlaczego 0?) to aqueue.
				 * 
				 * else 
				 * store y with the minimum distance inside aqueue and s inside aqueue
				 */
				if (minHij > Settings.Default.EpsilonHij && region.Depth < Settings.Default.Dmax) {
					Region r1, r2, r3, r4;

					int newSize = 1 + region.Size / 2;
					r1 = GenerateRegion(region.X, region.Y, newSize, newSize, bitmap);
					regionsMap[r1.Left, r1.Bottom] = r1;
					r2 = GenerateRegion(region.X + newSize - 1, region.Y, newSize, newSize, bitmap);
					regionsMap[r2.Left, r2.Bottom] = r2;
					r3 = GenerateRegion(region.X, region.Y + newSize - 1, newSize, newSize, bitmap);
					regionsMap[r3.Left, r3.Bottom] = r3;
					r4 = GenerateRegion(region.X + newSize - 1, region.Y + newSize - 1, newSize, newSize, bitmap);
					regionsMap[r4.Left, r4.Bottom] = r4;

					r1.Depth = r2.Depth = r3.Depth = r4.Depth = region.Depth + 1;

					regionsToProcess.AddFirst(r4);
					regionsToProcess.AddFirst(r3);
					regionsToProcess.AddFirst(r2);
					regionsToProcess.AddFirst(r1);

					/*
					foreach (Point p in r1.Points) {
						p.Z = bitmap[p.X, p.Y];
						interpolationPoints.Enqueue(p);
					}
					foreach (Point p in r2.Points) {
						p.Z = bitmap[p.X, p.Y];
						interpolationPoints.Enqueue(p);
					}
					foreach (Point p in r3.Points) {
						p.Z = bitmap[p.X, p.Y];
						interpolationPoints.Enqueue(p);
					}
					foreach (Point p in r4.Points) {
						p.Z = bitmap[p.X, p.Y];
						interpolationPoints.Enqueue(p);
					}
					za du¿o! :D
					dochodzi tylko 5 nowych punktów (a nie 16 ;))
					 */
					Point p1 = new Point(r1.Right, r1.Bottom, bitmap[r1.Right, r1.Bottom]);
					Point p2 = new Point(r1.Left, r1.Top, bitmap[r1.Left, r1.Top]);
					Point p3 = new Point(r1.Right, r1.Top, bitmap[r1.Right, r1.Top]);
					Point p4 = new Point(r4.Left, r4.Top, bitmap[r4.Left, r4.Top]);
					Point p5 = new Point(r4.Right, r4.Bottom, bitmap[r4.Right, r4.Bottom]);
					interpolationPoints.Enqueue(p1);
					interpolationPoints.Enqueue(p2);
					interpolationPoints.Enqueue(p3);
					interpolationPoints.Enqueue(p4);
					interpolationPoints.Enqueue(p5);					
				} else {
					region.Domain = minDomain;
					region.ContractivityFactor = minContrFactor;
					region.Parameters = minParams;
					regionsProcessed.Enqueue(region);
					region = null;
				}
			}

			#endregion

			Console.WriteLine("Koniec kompresji");

			#region Step 5)
			//store dmax, delta, Delta, cqueue, iqueue, aqueue

			ChannelData sfk = new ChannelData();
			sfk.Height = bitmap.GetLength(1);
			sfk.Width = bitmap.GetLength(0);
			sfk.InterpolationPoints = interpolationPoints.ToArray();
			sfk.SmallDelta = Settings.Default.SmallDelta;
			sfk.BigDelta = Settings.Default.BigDelta;
			sfk.DMax = Settings.Default.Dmax;
			sfk.Regions = regionsProcessed.ToArray();
			return sfk;

			#endregion
		}
	}
}
