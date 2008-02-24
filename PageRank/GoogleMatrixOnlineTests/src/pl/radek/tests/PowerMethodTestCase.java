/**
 * 
 */
package pl.radek.tests;

import junit.framework.TestCase;
import pl.radek.GoogleMatrix;
import pl.radek.computation.*;

/**
 * @author Radek
 *
 */
public class PowerMethodTestCase extends TestCase {

	public void test1(){
		GoogleMatrix gm = new GoogleMatrix();
		
		gm.setAlfa(0.8);
		
		double [][] s = new double [][] {
				new double [] {0,1.0/2.0,0,1.0/2.0,0 },
				new double [] {0,0,1.0/3.0,1.0/3.0,1.0/3.0 },
				new double [] {0,0,0,1,0 },
				new double [] {0,0,0,0,1 },
				new double [] {1,0,0,0,0 }				
		};
		
		gm.setS(s);
		
		/**
		 * personalization vector v>=0
		 * |v|1 =1
		 */
		double [] personalizationVector = new double [] {0.2,0.1,0.1,0.3,0.3};
		gm.setV(personalizationVector);
		Method method = getMethod();
		double [] pageRank = method.computeEigenVector(gm);
		
		for(int i =0; i<s.length;++i)System.out.println(pageRank[i]);
	}
	
	private Method getMethod() {
		
		return new PowerMethod3();
	}
}
