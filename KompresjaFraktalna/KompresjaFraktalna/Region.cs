using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna {
    class Region: Rectangle {

        private Double contractivityFactor;

        public Double ContractivityFactor {
            get { return contractivityFactor; }
            set { contractivityFactor = value; }
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
        /// g��boko�� regionu
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