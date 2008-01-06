using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna {
    class Decompressor {

        public int[,] Decompress(System.IO.Stream inputStream) {


            /*
             * A decompression algorithm - Strona 7 (3.3)
             * 
             */ 


            /*
             * create the queues AD[i], CON[i], IP[i], i = 1,2...,dmax, which contain the addresses, contractivity factors
             * and interpolation points of the regions of side delta/a^(i-1).  
             * All these numbers were stored in aqueue, cqueue and iqueue respectively.
             */ 

            int _delta;
            int _Delta;
            int _dMax;
            List<Point> _IP;
            List<double> _CON;
            List<Address> _AD;
            
            int width,height;
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();

            width = (int)formatter.Deserialize(inputStream);
            height = (int)formatter.Deserialize(inputStream);
            _dMax = (int)formatter.Deserialize(inputStream);
            _delta = (int)formatter.Deserialize(inputStream);
            _Delta = (int)formatter.Deserialize(inputStream);
            _CON = (List<double>)formatter.Deserialize(inputStream);
            _IP =(List<Point>) formatter.Deserialize(inputStream);
            _AD = (List<Address>)formatter.Deserialize(inputStream);

            int[,] image = new int[width, height];

            /*
             * generowanie 
             */


            /*
             * punkt 2. 
             * put the interpolation points inside the picture
             */

            foreach (Point interpolationPoint in _IP) {
                image[interpolationPoint.X, interpolationPoint.Y] = interpolationPoint.Z;
            }

            /*
             * punkt 3. 
             * Compute steps = trunc(log(delta) / log(a))  [ a = delta/ Delta ] .
             */

            int a = _delta/_Delta;
            double logdelta = Math.Log(_delta);
            double logA = Math.Log(a);
            int steps = (int)Math.Floor(logdelta / logA);

            /*
             * Punkt 4. for t = 1: steps do
             *              for i =1 : min { steps, dmax } 
             */

            // zachowam numeracje jak w algorytmie
            for (int t = 1; t <= steps; ++t) {
                for (int i = 1; i <= Math.Min(steps, _dMax); ++i) {

                    /*
                     * Punkt 4.a
                     * For every region of side delta/a^(i-1) do
                     */

                    // tworzymy regiony o boku delta / a&(i - 1)
                    List<Region> regions = GenerateRegions((int)(_delta / Math.Pow(a, i - 1)));
                    foreach (Region region in regions) {
                        /*
                         * punkt 4.a.i
                         * Find the corresponding domain (j) (from AD[i]).
                         */

                        Address correspondingDomain = _AD[i];
                        int j = correspondingDomain.Domain.J;

                        /*
                         * punkt 4.a.ii
                         * if j!=0 then
                         */

                        if (j != 0) {
                            /*
                             * Punkt 4.a.ii.A
                             * Find the corresponding contractivity factor (from CON[i]) and compute the rest of the map parameters.
                             */


                            double contractivityFactor = _CON[i];

                            /*
                             * reszta parametrow? jakie?
                             * 
                             */


                            /*
                             * Punkt 4.a.ii.B
                             * Do the following until the last row of the domain is reached (a ^(t-1) times). Remember that
                             * the domain (as a part of the picture) is initially empty and gradually fills iteration
                             * after iteration. At this stage only pixels of distance (horizontal and vertical) delta/a^(t-1) are 
                             * mapped and, therefore, capable of been mapped again. So, to go from one line to the next,
                             * you must skip delta/a^(t-1) lines.
                             * 
                             */ 

                            // czy to znaczy ze dla t = 1 mamy wejsc do tej petli czy nie? - mamy 1 wiersz i 1 kolumne. 
                            for (int k = 1; k <= Math.Pow(a, t - 1); ++k) {
                                /*
                                 * Punkt 4.a.ii.B.*
                                 * For the first row do: Skip the first point(it has been mapped already). Map
                                 * the next a-1 points of the domain, skip the next point. map the next a-1 points etc.
                                 * Do this until the right domain endpoint is reached (a^(t-1) times). Remeber that these
                                 * a-1 point are not consecutive (kolejne) ! To go from one point to the next you must skip
                                 * delta/a^(t-1) points.
                                 */

                                if (k == 1) {



                                } else {

                                    /*
                                     * Punkt 4.a.ii.B.**
                                     * Map the next a-1 lines as follows: At each line map a^t + 1 points. Remember that
                                     * these points are not consecutive (kolejne) ! To o from one point to the next you must
                                     * skip delta/a^(t-1) points.
                                     */

                                }

                            }

                            /*
                             * Punkt 4.a.iii
                             * For the last line do:
                             * Skip the first point (interpolation point). Map the next a-1 points of the domain, skip
                             * the next point. map the next a-1 point etc.
                             * Do this until the right domain endpoint is reached (a^(t-1) times). Remember that these a-1 
                             * point are not consecutive! to go from one point to the next you must skip delta/a^(t-1) points.
                             */ 




                        }

                    }




                }
            }



            return image;
        }

        private List<Region> GenerateRegions(int p) {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
