using System;
using System.Collections.Generic;
using System.Text;

namespace KompresjaFraktalna {
    class Address {
        private Domain _domain;
        private double _hij;
        private double _contractivityFactor;
        private Params _otherParameters;

		public Params OtherParameters {
            get { return _otherParameters; }
            set { _otherParameters = value; }
        }
	
        public Domain Domain {
            get { return _domain; }
            set { _domain = value; }
        }
        

        public double Hij {
            get { return _hij; }
            set { _hij = value; }
        }
        

        public double ContractivityFactor {
            get { return _contractivityFactor; }
            set { _contractivityFactor = value; }
        }
    }
}
