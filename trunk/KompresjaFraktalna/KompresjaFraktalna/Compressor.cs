using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna {
    class Compressor : Common {

        int[,] bitmap;
        
		double _epsilonHIJ = 50;
		int _delta;
		int _Delta;
		int _a;
		double _epsilon;
		int _dMax;

		public Compressor() {
			#region Step 1)
			/*
             * Punkt 1. choose values for delta and Delta, such that Delta = a * delta. 
             * Choose also, error tolerance Epsilon and maximum depth Dmax.
             */
			#endregion

			_epsilonHIJ = 50;
			_delta = Compression.Default.SmallDelta;
			_Delta = Compression.Default.BigDelta;
			_a = _Delta / _delta;
			_epsilon = Compression.Default.Epsilon;
			_dMax = Compression.Default.Dmax;
		}

		/// <summary>
        /// Kompresja jednej skladowej koloru.
        /// </summary>
        /// <param name="bitmap">tablica z wartosciami skladowej koloru</param>
        /// <param name="outputStream">strumien do ktorego zostanie zapisane wszystko</param>
        public void Compress(int[,] bitmap, System.IO.Stream outputStream) {
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
            Queue<Region> squeue = GenerateRegions(_delta, width, height);

			Console.WriteLine("Generowanie punktów interpolacji");
			Queue<Point> iqueue = GenerateInterpolationPoints(_delta, width, height);

			Console.WriteLine("Inicjowanie kolejek i tablicy regionów");
			Queue<double> cqueue = new Queue<double>();
			Queue<Address> aqueue = new Queue<Address>();
			Queue<Region> squeue2 = new Queue<Region>();
            Region[,] regions = new Region[width, height];
            Queue<Region> _regions = new Queue<Region>();
            int d = 1;
            foreach (Region reg in squeue) {
                regions[reg.Left, reg.Bottom] = reg;
            }

			Console.WriteLine("Generowanie domen");
			Queue<Domain> domains = GenerateDomains(_Delta, width, height);

			#endregion

			do {
				#region Step 3)
				/*
                 * Punkt 3. 
                 * while 'squeue' is not empty do:
                 */               

                while (squeue.Count != 0) {
                    /*
                     * 3.a: 
                     * get one region from squeue
                     */
					Region region = squeue.Dequeue();

                    Address minimum = new Address();
                    minimum.Hij = Double.MaxValue;

                    foreach (Domain domain in domains) {
                        /*
                         * 3.b.x. 
                         * compute the contractivity factor for the map acciociated with the y-th domain and the region.
                         */
                        double contractivityFactor = ComputeContractivityFactor(domain, region,bitmap);

                        /*
                         * 3.b.ii. 
                         * if |s| >= 1 go to 3.b.v . otherwise chec the condition of continuity. if it doesn't hold, go to 3.b.v
                         */
                        if (Math.Abs(contractivityFactor) >= 1) {
                            continue;
                        }
                        region.ContractivityFactor = contractivityFactor;
                        if (CheckConditionOfContinuity(domain, region,regions,_delta) == false) {
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

                        double[] parameters;
                        if (TryMapDomainToRegion(domain, region, contractivityFactor, bitmap, out parameters)) {
                            minimum.OtherParameters = parameters;

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
                        if (hij < minimum.Hij) {
                            minimum.Domain = domain;
                            minimum.Hij = hij;
                            minimum.ContractivityFactor = contractivityFactor;
                            minimum.OtherParameters = null; // 
                        }
                        /*
                         * 3.b.v.
                         * next y.
                         */
                    }

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
                    if (minimum.Hij > _epsilonHIJ && d < _dMax) {
                        Region r1, r2, r3, r4;
                        int newSize = 1 + region.Size / 2;
                        r1 = GenerateRegion(region.X, region.Y, newSize, newSize, bitmap);
                        regions[r1.Left, r1.Bottom] = r1;

                        r2 = GenerateRegion(region.X + newSize - 1, region.Y, newSize, newSize, bitmap);
                        regions[r2.Left, r2.Bottom] = r2;
                        r3 = GenerateRegion(region.X, region.Y + newSize - 1, newSize, newSize, bitmap);
                        regions[r3.Left, r3.Bottom] = r3;

                        r4 = GenerateRegion(region.X + newSize - 1, region.Y + newSize - 1, newSize, newSize, bitmap);
                        regions[r4.Left, r4.Bottom] = r4;
                        r1.Depth = r2.Depth = r3.Depth = r4.Depth = region.Depth + 1;
                        squeue2.Enqueue(r1);
						squeue2.Enqueue(r2);
						squeue2.Enqueue(r3);
						squeue2.Enqueue(r4);
                        foreach (Point p in r1.Points) {
							iqueue.Enqueue(p);
                        }
                        foreach (Point p in r2.Points) {
							iqueue.Enqueue(p);
                        }
                        foreach (Point p in r3.Points) {
							iqueue.Enqueue(p);
                        }
                        foreach (Point p in r4.Points) {
							iqueue.Enqueue(p);
                        }
						//aqueue.Enqueue(0); //??
                    } else {



						//aqueue.Enqueue(minimum);
						//cqueue.Enqueue(minimum.ContractivityFactor);

                        region.Domain = minimum.Domain;
                        region.ContractivityFactor = minimum.ContractivityFactor;
                        region.Parameters = minimum.OtherParameters;
                        _regions.Enqueue(region);
                    }
				}

				#endregion

				#region Step 4)
				//if squeue2 is not empty, then set squeue = squeue2, d = d+1 ant go to 3)

                if (squeue2.Count == 0) {
					break;
				}

                squeue = squeue2;
				squeue2 = new Queue<Region>(); // ? nie jestem pewien, ale raczej trzeba wyczyscic
				
                d++;

				#endregion
			} while (true);

			#region Step 5)
			//store dmax, delta, Delta, cqueue, iqueue, aqueue

			Console.WriteLine("Zapisywanie danych");

            SuperFajnaKlasa sfk = new SuperFajnaKlasa();
            sfk.Height = bitmap.GetLength(1);
            sfk.Width = bitmap.GetLength(0);
            sfk.InterpolationPoints = iqueue.ToArray();
            sfk.SmallDelta = _delta;
            sfk.BigDelta = _Delta;
            sfk.DMax = _dMax;
            sfk.Regions = _regions.ToArray();
			
            sfk.Save(outputStream);

			Console.WriteLine("Zapisywanie danych zakoñczone");
			#endregion
        }

        private double Distance(Domain domain, Region region, double[] parameters) {
            double hij = 0;
            int delta = domain.Width / region.Width;

            double[,] mappedRegion = new double[region.Width, region.Height];
            for (int x = domain.Left; x <= domain.Right; x+=delta) {
                for (int y = domain.Bottom; y <= domain.Top; y += delta) {
                    int z = bitmap[x, y];
                    double xm = parameters[(int)param.a] * x + parameters[(int)param.k];
                    double ym = parameters[(int)param.d] * y + parameters[(int)param.l];
                    double zm = parameters[(int)param.e] * x + parameters[(int)param.g] * y + parameters[(int)param.h] * x * y + region.ContractivityFactor * z + parameters[(int)param.m];

                    mappedRegion[(int)xm - region.Left, (int)ym - region.Bottom] = zm;
                }
            }

            for (int x = 0; x < region.Width; ++x) {
                for (int y = 0; y < region.Height; ++y) {
                    hij += Math.Abs(bitmap[region.X + x, region.Y + y] - mappedRegion[x, y]);
                }
            }

            return hij / (region.Width * region.Width);
        }
        private bool TryMapDomainToRegion(Domain domain, Region region, double s, int[,] bitmap, out double[] parameters) {

            parameters = new double[8];
            parameters[(int)param.a] = ((double)region.Size - 1.0) / ((double)domain.Size - 1);
            parameters[(int)param.k] = region.X - domain.X * parameters[(int)param.a];
            parameters[(int)param.d] = parameters[(int)param.a];
            parameters[(int)param.l] = region.Y - domain.Y * parameters[(int)param.a];
            double[] B = new double[4];
            double[,] A = new double[,] { 
                  { domain.Left, domain.Bottom, domain.Left * domain.Bottom, 1, bitmap[region.Left, region.Bottom] - s * bitmap[domain.Left, domain.Bottom] },
                  { domain.Right, domain.Bottom, (domain.Right)*(domain.Bottom), 1, bitmap[region.Right, region.Bottom] - s * bitmap[domain.Right, domain.Bottom]},
                  { domain.Left, domain.Top, domain.Left*(domain.Top), 1, bitmap[region.Left, region.Top] - s * bitmap[domain.Left, domain.Top]},
                  { domain.Right, domain.Top, (domain.Right)*(domain.Top), 1, bitmap[region.Right, region.Top] - s * bitmap[domain.Right, domain.Top]}
           };
            if (LinearEquationSolver.GaussianElimination(A, B)) {
                parameters[(int)param.e] = B[0];
                parameters[(int)param.g] = B[1];
                parameters[(int)param.h] = B[2];
                parameters[(int)param.m] = B[3];
                return true;
            } else
                throw new Exception("Macierz osobliwa");


            
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="region"></param>
        /// <param name="bitmap"></param>
        /// <param name="parameters"></param>
        /// <returns>[a,k,d,l,e,g,h,m]</returns>
        private bool TryMapDomainToRegion2(Domain domain, Region region, double s, int[,] bitmap, out double[] parameters) {

            /* w tym miejscu wydaje mi sie ze trzeba wyliczyc 8 rownan z 8 niewiadomymi. niewiadome to
             * aij, kij, dij, lij, eij,gij,hij,mij. - rownania  pod koniec 2giej strony.
             * 
             * 
             */
            double[,] matrix = new double[9, 8];

            for (int i = 0; i < 9; ++i)
                for (int j = 0; j < 8; ++j)
                    matrix[i, j] = 0;

            //dla przypomnienia - matrix [kolumna, wiersz]

            // lewy gorny rog 
            matrix[0, 0] = domain.X;
            matrix[1, 0] = 1;
            matrix[8, 0] = region.X;

            matrix[2, 1] = domain.Y;
            matrix[3, 1] = 1;
            matrix[8, 1] = region.Y;

            matrix[4, 2] = domain.X;
            matrix[5, 2] = domain.Y;
            matrix[6, 2] = domain.X * domain.Y;
            matrix[7, 2] = 1;
            matrix[8, 2] = (double)bitmap[region.X, region.Y] - s * (double)bitmap[domain.X, domain.Y];

            matrix[0, 3] = domain.X + domain.Width;
            matrix[1, 3] = 1;
            matrix[8, 3] = region.X + region.Width;

            matrix[2, 4] = domain.Y + domain.Height;
            matrix[3, 4] = 1;
            matrix[8, 4] = region.Y + region.Height;

            // prawy gorny
            matrix[4, 5] = domain.X + domain.Width;
            matrix[5, 5] = domain.Y;
            matrix[6, 5] = (domain.X + domain.Width) * domain.Y;
            matrix[7, 5] = 1;
            matrix[8, 5] = (double)bitmap[region.X + region.Width, region.Y] - s * (double)bitmap[domain.X + domain.Width, domain.Y];

            //lewy dolny
            matrix[4, 6] = domain.X;
            matrix[5, 6] = domain.Y + domain.Height;
            matrix[6, 6] = domain.X * (domain.Y + domain.Height);
            matrix[7, 6] = 1;
            matrix[8, 6] = (double)bitmap[region.X, region.Y + region.Height] - s * (double)bitmap[domain.X, domain.Y + domain.Height];

            // prawy dolny
            matrix[4, 7] = domain.X + domain.Width;
            matrix[5, 7] = domain.Y + domain.Height;
            matrix[6, 7] = (domain.X + domain.Width) * (domain.Y + domain.Height);
            matrix[7, 7] = 1;
            matrix[8, 7] = (double)bitmap[region.X + region.Width, region.Y + region.Height] - s * (double)bitmap[domain.X + domain.Width, domain.Y + domain.Height];

            parameters = new double[8];
			for (int i = 0; i < 8; ++i) {
				parameters[i] = 0;
			}
            return LinearEquationSolver.GaussianElimination(matrix, parameters);
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
        private Region GenerateRegion(int x, int y, int width, int height, int[,] bitmap) {
            Region r = new Region(x, y, width, height, bitmap[x, y], bitmap[x + width - 1, y], bitmap[x, y + height - 1], bitmap[x + width -1 , y + height - 1]);
            return r;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        private bool CheckConditionOfContinuity(Domain domain, Region region, Region[,] regions, int delta) {

            if(region.Bottom == 0 && region.Left ==0) return true;

            do {

                if (region.Left != 0) {
                    // sprawdzamy lewy region
                    Region left = regions[region.X - region.Width + 1, region.Y];
                    if (left == null) break;

                    // hack me
                    if (left.Depth != region.Depth) {
                        break;
                    }

                    // liczymy condition of continuity
                    Domain leftDomain = left.Domain;
                    if (leftDomain == null) break;
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
                        if (Math.Abs(diff.Value) > this._epsilon) {
                            return false;
                        }
                    }
                }
            } while (false);

            if (region.Bottom != 0) {
                // sprawdzamy lewy region
                Region down = regions[region.Left, region.Bottom - region.Height + 1];
                if (down == null) return true;

                // hack me
                if (down.Depth != region.Depth) {
                    return true;
                }

                // liczymy condition of continuity
                Domain downDomain = down.Domain;
                if (downDomain == null) return true;
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
                    int valUp = bitmap[domain.X + i,  domain.Bottom];

                    double tmp1 = distanceFromLine(downDomain.Left,
                        bitmap[downDomain.Left, downDomain.Top], downDomain.Right,
                        bitmap[downDomain.Right, downDomain.Top],
                        downDomain.Left + i, bitmap[downDomain.Left + i, downDomain.Top]);
                    double tmp2 = distanceFromLine(domain.Left,
                        bitmap[domain.Left, domain.Bottom], domain.Right,
                        bitmap[domain.Right, domain.Bottom],
                        domain.Left + i, bitmap[domain.Left + i, domain.Bottom]);

                    double diff = contrFactorDown * tmp1 - contrFactorUp * tmp2;
                    if (Math.Abs(diff) > this._epsilon) {
                        return false;
                    }
                }
            }
               


            return true;
        }

        private double distanceFromLine(double x1, double z1, double x2, double z2, double xt, double zt) {
            double a = (z2 - z1) / (x2-x1);
            return Math.Abs(a * xt + z1 - zt);
        }

        private double ComputeMeanDistance(Rectangle rect, int [,] bitmap) {

            double a;
            double distances = 0;


            for (int row = rect.Y ; row < rect.Y + rect.Height; ++row) {
                
                a = (bitmap[rect.Right, row] - bitmap[rect.X, row]) / rect.Width;

                double aIter = a * rect.X;

                for (int x = rect.X; x < rect.X + rect.Width; ++x) {

                    aIter += a;
                    distances += Math.Abs(aIter - bitmap[x,row]);
                    // aIter wartosc w punkcie

                }


            }

            distances/=rect.Height*rect.Width;
            
            return distances;
        }

        private double ComputeContractivityFactor(Domain domain, Region region, int [,] bitmap) {
            double mi = 1;
            double ni = 1;

            // mi - domena
            mi = ComputeMeanDistance(domain,bitmap);



            // ni - region
            ni = ComputeMeanDistance(region,bitmap);

            return ni / mi;
        }


    }
}
