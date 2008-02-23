using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna {
    [Serializable]
    class Region: Rectangle {

        private double contractivityFactor = Double.MaxValue;

        public double ContractivityFactor {
            get { return contractivityFactor; }
            set { contractivityFactor = value; }
        }

        private Point domainPosition;

        public Point DomainPosition {
            get { return domainPosition; }
            set { domainPosition = value; }
        }

        private double[] parameters;

        public double[] Parameters {
            get { return parameters; }
            set { parameters = value; }
        }

        

        private int depth;

        private Domain domain;

        /// <summary>
        /// Domena przypisana do regionu
        /// </summary>
        public Domain Domain {
            get { return domain; }
            set { domain = value; }
        }

        /// <summary>
        /// g³êbokoœæ regionu
        /// </summary>
        public int Depth {
            get { return depth; }
            set { depth = value; }
        }

        public Region(int x, int y, int width, int height, int x00, int x10, int x01, int x11)
            : base(x, y, width, height, x00, x10, x01, x11) {
        }
    }
}
