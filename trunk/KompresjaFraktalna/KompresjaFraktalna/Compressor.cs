using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna {
    class Compressor {

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


            /*
             * punkt 2. create two queues, one named squeue and put all the regions inside as well 
             * as a queue named iqueue and put all initial interpolation points inside.
             * In addition create two empty queues named cqueue and aqueue (we store contractivity factors in
             * the first and the addresses in the latter). set the depth d = 1 and create queue named squeue2.
             */

            LinkedList<Region> squeue, squeue2;
            LinkedList<Point> iqueue;
            LinkedList<double> cqueue;
            LinkedList<Address> aqueue;
            int _d;

            squeue = GenerateRegions();
            iqueue = GenerateInterpolationPoints();
            cqueue = new LinkedList<double>();
            aqueue = new LinkedList<Address>();
            _d = 1;
            squeue2 = new LinkedList<Region>();

            do {

                /*
                 * Punkt 3. 
                 * while squeue is not empty do:
                 */


                LinkedList<Domain> domains = GenerateDomains();
                while (squeue.Count != 0) {
                    /*
                     * 3.a: 
                     * get one region from squeue
                     */
                    Region region = squeue.First.Value;
                    squeue.RemoveFirst();


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
                         */


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

                        squeue2.AddLast(r1);
                        squeue2.AddLast(r2);
                        squeue2.AddLast(r3);
                        squeue2.AddLast(r4);
                        foreach (Point p in r1.Points) {
                            iqueue.AddLast(p);
                        }
                        foreach (Point p in r2.Points) {
                            iqueue.AddLast(p);
                        }
                        foreach (Point p in r3.Points) {
                            iqueue.AddLast(p);
                        }
                        foreach (Point p in r4.Points) {
                            iqueue.AddLast(p);
                        }
                    } else {
                        aqueue.AddLast(minimum);
                        cqueue.AddLast(minimum.ContractivityFactor);
                    }
                }
                /*
                 * punkt 4. if squeue2 is not empty, then set squeue = squeue2, d = d+1 ant go to 3.
                 */

                if (squeue2.Count != 0) {
                    squeue = squeue2;
                    squeue2 = new LinkedList<Region>(); // ? nie jestem pewien, ale raczej trzeba wyczyscic
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

        private LinkedList<Domain> GenerateDomains() {
            throw new Exception("The method or operation is not implemented.");
        }

        private LinkedList<Point> GenerateInterpolationPoints() {
            throw new Exception("The method or operation is not implemented.");
        }

        private LinkedList<Region> GenerateRegions() {
            throw new Exception("The method or operation is not implemented.");
        }

    }
}
