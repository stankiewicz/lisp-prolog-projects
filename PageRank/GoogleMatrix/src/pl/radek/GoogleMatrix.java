/**
 * 
 */
package pl.radek;

/**
 * @author Radek
 *
 */
public class GoogleMatrix {

	/**
	 * Wspolczynnik
	 */
	double alfa;
	/**
	 * Macierz polaczen
	 */
	double [][] s;
	
	/**
	 * personalization vector v>=0
	 * |v|1 =1
	 */
	double []v;

	public double getAlfa() {
		return alfa;
	}

	public void setAlfa(double alfa) {
		this.alfa = alfa;
	}

	/**
	 * s[row][col] - wiersze odpowiadaja polaczeniom strony 'row' do poszczegolnych stron 'col'
	 * @return
	 */
	public double[][] getS() {
		return s;
	}

	public void setS(double[][] s) {
		this.s = s;
	}
	/**
	 * personalization vector v>=0
	 * |v|1 =1
	 */
	public double[] getV() {
		return v;
	}
	/**
	 * personalization vector v>=0
	 * |v|1 =1
	 */
	public void setV(double[] v) {
		this.v = v;
	}
}
