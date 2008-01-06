using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna {
    class Compressor:Common {

        /// <summary>
        /// kompresja jednej skladowej koloru
        /// </summary>
        /// <param name="bitmap">tablica z wartosciami skladowej koloru</param>
        /// <param name="outputStream">strumien do ktorego zostanie zapisane wszystko</param>
        public  void Compress(int [,] bitmap, System.IO.Stream outputStream) {
            /*
             * realizacja algorytmu ze strony 6.
             * 
             */

            /*
             * Punkt 1. choose values for delta and Delta, such that Delta = a * delta. 
             * Choose also, error tolerance Epsilon and maximum depth Dmax.
             */

            int _delta = Compression.Default.SmallDelta;
            int _Delta = Compression.Default.BigDelta;
            double _epsilon = Compression.Default.Epsilon;
            int _dMax = Compression.Default.Dmax;

            int width = bitmap.GetLength(0);
            int height = bitmap.GetLength(1);
            /*
             * punkt 2. create two queues, one named squeue and put all the regions inside as well 
             * as a queue named iqueue and put all initial interpolation points inside.
             * In addition create two empty queues named cqueue and aqueue (we store contractivity factors in
             * the first and the addresses in the latter). set the depth d = 1 and create queue named squeue2.
             */

            List<Region> squeue, squeue2;
            List<Point> iqueue;
            List<double> cqueue;
            List<Address> aqueue;
            int _d;

            squeue = GenerateRegions(_delta,width,height);
            iqueue = GenerateInterpolationPoints(_delta, width, height);
            

            cqueue = new List<double>();
            aqueue = new List<Address>();
            _d = 1;
            squeue2 = new List<Region>();

            do {

                /*
                 * Punkt 3. 
                 * while squeue is not empty do:
                 */


                List<Domain> domains = GenerateDomains(_Delta,width,height);
                while (squeue.Count != 0) {
                    /*
                     * 3.a: 
                     * get one region from squeue
                     */
                    Region region = squeue[0];
                    squeue.RemoveAt(0);


                    Address minimum = new Address();
                    minimum.Hij = Double.MaxValue;

                    foreach (Domain domain in domains) {
                        /*
                         * 3.b.i. 
                         * compute the contractivity factor for the map acciociated with the j-th domain and the region.
                         */
                        double contractivityFactor = ComputeContractivityFactor(domain, region);

                        /*
                         * 3.b.ii. 
                         * if |s| >= 1 go to 3.b.v . otherwise chec the condition of continuity. if it doesn't hold, go to 3.b.v
                         */
                        if (Math.Abs(contractivityFactor) >= 1) {
                            continue;
                        }

                        if (CheckConditionOfContinuity(domain, region) == false) {
                            continue;
                        }

                        /*
                         * 3.b.iii.
                         * Compute the other parameters and map the points of the j-th domain (say Dj) through 'w' according
                         * to the 'mapping algorithm'. say w(Dj) the emerging set.
                         */


                        /* w tym miejscu wydaje mi sie ze trzeba wyliczyc 8 rownan z 8 niewiadomymi. niewiadome to
                         * aij, kij, dij, lij, eij,gij,hij,mij. - rownania  pod koniec 2giej strony.
                         * 
                         * aczkolwiek 'Mapping Algorithm opisany na gorze 7 strony jest dziwny'
                         */

                        double [] parameters;
                        if (TryMapDomainToRegion(domain, region,contractivityFactor, bitmap, out parameters)) {
                            minimum.OtherParameters = parameters;

                        } else {
                            throw new Exception("moja glowa!! brak rozwiazania");
                        }

                        /*
                         * 3.b.iv.
                         * compute (with a proper distance measure) the distance hij between w(Dj) and the points of region i.
                         */

                        /*
                         * jak liczyc taka odleglosc?
                         * po tym bedzie minimum.
                         */
                        double hij = 0;
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
                         * next j.
                         */
                    }

                    /*
                     * Punkt 3.c
                     * Find the j for which hij is a minimum
                     */

                    //minimum - juz mamy

                    /*
                     * Punkt 3.d
                     * If hij > Epsilon and d < dmax 
                     * then create four new regions, add them to squeue2, add the vertices of each new region to iqueue (as additional interpolation points) 
                     * and add 0 (?dlaczego 0?) to aqueue.
                     * 
                     * else 
                     * store j with the minimum distance inside aqueue and s inside aqueue
                     */
                    if (minimum.Hij > _epsilon && _d < _dMax) {
                        Region r1, r2, r3, r4;
                        r1 = GenerateRegion(region.X, region.Y, region.Width / 2, region.Height / 2, bitmap);
                        r1 = GenerateRegion(region.X, region.Y, region.Width / 2, region.Height / 2, bitmap);
                        r2 = GenerateRegion(region.X + r1.Width, region.Y, region.Width - region.Width / 2, region.Height - region.Height / 2, bitmap);
                        r3 = GenerateRegion(region.X, region.Y + r1.Height, region.Width / 2, region.Height - region.Height / 2, bitmap);
                        r4 = GenerateRegion(region.X + r1.Width, region.Y + r1.Height, region.Width - region.Width / 2, region.Height - region.Height / 2, bitmap);

                        squeue2.Add(r1);
                        squeue2.Add(r2);
                        squeue2.Add(r3);
                        squeue2.Add(r4);
                        foreach (Point p in r1.Points) {
                            iqueue.Add(p);
                        }
                        foreach (Point p in r2.Points) {
                            iqueue.Add(p);
                        }
                        foreach (Point p in r3.Points) {
                            iqueue.Add(p);
                        }
                        foreach (Point p in r4.Points) {
                            iqueue.Add(p);
                        }
                    } else {
                        aqueue.Add(minimum);
                        cqueue.Add(minimum.ContractivityFactor);
                    }
                }
                /*
                 * punkt 4. if squeue2 is not empty, then set squeue = squeue2, d = d+1 ant go to 3.
                 */

                if (squeue2.Count != 0) {
                    squeue = squeue2;
                    squeue2 = new List<Region>(); // ? nie jestem pewien, ale raczej trzeba wyczyscic
                    _d += 1;
                } else {
                    break;
                }
            } while (true);


            /*
             * punkt 5.
             * store dmax, delta, Delta, cqueue, iqueue, aqueue
             * 
             */
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();
            formatter.Serialize(outputStream,bitmap.GetLength(0));
            formatter.Serialize(outputStream, bitmap.GetLength(1));
            formatter.Serialize(outputStream, _dMax);
            formatter.Serialize(outputStream, _delta);
            formatter.Serialize(outputStream, _Delta);
            formatter.Serialize(outputStream, cqueue);
            formatter.Serialize(outputStream, iqueue);
            formatter.Serialize(outputStream, aqueue);

            /*
             * punkt 6. koniec
             */
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="region"></param>
        /// <param name="bitmap"></param>
        /// <param name="parameters"></param>
        /// <returns>[a,k,d,l,e,g,h,m]</returns>
        private bool TryMapDomainToRegion(Domain domain, Region region, double s, int[,] bitmap, out double[] parameters) {
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
            matrix[8, 2] = (double)bitmap[region.X, region.Y] - s*(double)bitmap[domain.X,domain.Y];

            matrix[0, 3] = domain.X+domain.Width;
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
            matrix[8, 6] = (double)bitmap[region.X, region.Y+region.Height] - s * (double)bitmap[domain.X, domain.Y+domain.Height];

            // prawy dolny
            matrix[4, 7] = domain.X+domain.Width;
            matrix[5, 7] = domain.Y+domain.Height;
            matrix[6, 7] = (domain.X +domain.Width)* (domain.Y+domain.Height);
            matrix[7, 7] = 1;
            matrix[8, 7] = (double)bitmap[region.X+region.Width, region.Y+region.Height] - s * (double)bitmap[domain.X+domain.Width, domain.Y+domain.Height];

            


            parameters = new double[8];
            return LinearEquationSolver.GaussianElimination(matrix, parameters);
        }

        private bool TryMapDomainToRegion(Domain domain, Region region) {
            throw new Exception("The method or operation is not implemented.");
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
            Region r = new Region(x, y, width, height,bitmap[x,y],bitmap[x+width,y],bitmap[x,y+height],bitmap[x+width,y+height]);
            return r;
        }


        private bool CheckConditionOfContinuity(Domain domain, Region region) {
            throw new Exception("The method or operation is not implemented.");
        }

        private double ComputeContractivityFactor(Domain domain, Region region) {
            throw new Exception("The method or operation is not implemented.");
        }

        
    }
}
