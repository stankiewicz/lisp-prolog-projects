/**
 * 
 */
package pl.radek.computation;

import pl.radek.GoogleMatrix;

/**
 * @author Radek
 * 
 */
public class PowerMethod implements Method {

	/*
	 * (non-Javadoc)
	 * 
	 * @see pl.radek.computation.IComputation#computeEigenVector(pl.radek.GoogleMatrix)
	 */
	@Override
	public double[] computeEigenVector(GoogleMatrix matrix) {
		// TODO Auto-generated method stub
		double epsilon = Math.pow(10, -8);
		int k = 0;
		double[] eigenVector = initialGuess(matrix);
		double bound =0;
		do {
			++k;
			eigenVector = multiply(eigenVector, matrix);
			bound = Math.pow(matrix.getAlfa(), k) * 2.0;
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
	protected double[] multiply(double[] eigenVector, GoogleMatrix matrix) {
		/**
		 * TODO: Te petle mozna ladnie zoptymalizowac jeszcze
		 * 
		 */

		int len = eigenVector.length;
		double[] res = new double[len];
		for (int i = 0; i < len; ++i)
			res[i] = 0;
		double alfa = matrix.getAlfa();
		// s[row][col]
		double[][] s = matrix.getS();
		double[] v = matrix.getV();

		// eigenVector * alfa * S
		for (int col = 0; col < len; ++col) {
			for (int row = 0; row < len; ++row) {
				res[col] += eigenVector[row] * s[row][col];
			}
			res[col] *= alfa;
		}
		double tmp1 = 1;
		//for (int col = 0; col < len; ++col) {
		//	tmp1 += eigenVector[col];
		//}
		tmp1 *= (1.0 - alfa);
		double test = 0;
		for (int col = 0; col < len; ++col) {
			res[col] += tmp1 * v[col];
			test +=res[col];
		}
		System.out.println("|xt|1="+test);
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
