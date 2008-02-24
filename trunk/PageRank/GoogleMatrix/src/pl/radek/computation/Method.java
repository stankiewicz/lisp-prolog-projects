package pl.radek.computation;

import pl.radek.GoogleMatrix;

public interface Method {

	/**
	 * Liczy wartosci wlasne - page rank
	 * @param matrix google matrix
	 * @return page rank - jesli pg[i] > pg[j] to i wyswietlane wyzej od j
	 */
	public double [] computeEigenVector(GoogleMatrix matrix);
}
