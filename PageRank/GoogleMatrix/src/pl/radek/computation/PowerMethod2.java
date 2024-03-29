/**
 * 
 */
package pl.radek.computation;

import pl.radek.GoogleMatrix;

/**
 * @author Radek
 * 
 */
public class PowerMethod2 implements Method {

	/*
	 * (non-Javadoc)
	 * 
	 * @see pl.radek.computation.IComputation#computeEigenVector(pl.radek.GoogleMatrix)
	 */
	
	public double[] computeEigenVector(GoogleMatrix matrix) {
		// TODO Auto-generated method stub
		double epsilon = Math.pow(10, -8);
		int k = 0;
		double[] eigenVector = initialGuess(matrix);
		
		double [][] s = matrix.getS();
		int len = eigenVector.length;
		double[] v = matrix.getV();
		for (int col = 0; col < len; ++col) {
			
			for (int row = 0; row < len; ++row) {
				s[row][col] *=matrix.getAlfa();
				s[row][col] += (1.0 - matrix.getAlfa())*v[col];
			}			
		}
		
		double alfa = matrix.getAlfa();
		double bound =2;
		
		do {
			++k;
			
			
			bound *= alfa ;
			eigenVector = multiply(eigenVector, s);
			//bound = Math.pow(matrix.getAlfa(), k) * 2.0;
		} while (bound >= epsilon);

		// warunek stopu :
		return eigenVector;
	}

	/**
	 * szybkie mnozenie wektora wlasnego z macierza google
	 * 
	 * @param eigenVector
	 * @param matrix
	 * @return
	 */
	protected double[] multiply(double[] eigenVector, double [][] matrix) {
		/**
		 * TODO: Te petle mozna ladnie zoptymalizowac jeszcze
		 * 
		 */

		int len = eigenVector.length;
		double[] res = new double[len];
		for (int i = 0; i < len; ++i)
			res[i] = 0;


		// eigenVector * alfa * S
		for (int col = 0; col < len; ++col) {
			for (int row = 0; row < len; ++row) {
				res[col] += eigenVector[row] * matrix[row][col];
			}

		}

		double test = 0;
		for (int col = 0; col < len; ++col) {
			test +=res[col];
		}
		if(DEBUG)System.out.println("|xt|1="+test);
		return res;
	}

	/**
	 * pierwszy wektor wartosci wlasnych jest zgadywany - losowe lub ustalone
	 * 
	 * @param matrix
	 * @return
	 */
	private double[] initialGuess(GoogleMatrix matrix) {
		int len = matrix.getS().length;
		double[] x0 = new double[len];
		for (int i = 0; i < len; ++i) {
			x0[i] = 1.0/len;
		}
		return x0;
	}

}
