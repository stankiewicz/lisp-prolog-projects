using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna {
    class Decompressor {

        public int[,] Decompress(System.IO.Stream inputStream) {

            int _delta;
            int _Delta;
            int _dMax;
            LinkedList<Point> iqueue;
            LinkedList<double> cqueue;
            LinkedList<Address> aqueue;
            
            int width,height;
            System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Soap.SoapFormatter();

            width = (int)formatter.Deserialize(inputStream);
            height = (int)formatter.Deserialize(inputStream);
            _dMax = (int)formatter.Deserialize(inputStream);
            _delta = (int)formatter.Deserialize(inputStream);
            _Delta = (int)formatter.Deserialize(inputStream);
            cqueue = (LinkedList<double>)formatter.Deserialize(inputStream);
            iqueue =(LinkedList<Point>) formatter.Deserialize(inputStream);
            aqueue = (LinkedList<Address>)formatter.Deserialize(inputStream);

            int[,] image = new int[width, height];

            /*
             * generowanie 
             */


            return image;
        }
    }
}
