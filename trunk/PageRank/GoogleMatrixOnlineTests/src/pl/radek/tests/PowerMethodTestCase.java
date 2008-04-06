/**
 * 
 */
package pl.radek.tests;

import java.util.GregorianCalendar;

import junit.framework.TestCase;
import pl.radek.GoogleMatrix;
import pl.radek.computation.Method;
import pl.radek.computation.PowerMethod;
import pl.radek.computation.PowerMethod2;
import pl.radek.computation.PowerMethod3;

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
	
	private GoogleMatrix provideGM(){
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
		
		return gm;
	}
	
	public void test2Benchmark(){
		
		GoogleMatrix gm = provideGM();
		long time = new GregorianCalendar().getTimeInMillis();
		
		for(int i =0; i<10000;++i){
			Method m = new PowerMethod2();
			m.computeEigenVector(gm);
			
		}
		long to = new GregorianCalendar().getTimeInMillis();
		
		System.out.println("200 * PowerMethod2 = " + (to - time));
		gm = provideGM();
		time = new GregorianCalendar().getTimeInMillis();
		for(int i =0; i<10000;++i){
			Method m = new PowerMethod3();
			m.computeEigenVector(gm);
			
		}
		to = new GregorianCalendar().getTimeInMillis();
		
		System.out.println("200 * PowerMethod3 = " + (to - time));
		gm = provideGM();

		time = new GregorianCalendar().getTimeInMillis();
		for(int i =0; i<10000;++i){
			Method m = new PowerMethod();
			m.computeEigenVector(gm);
			
		}
		to = new GregorianCalendar().getTimeInMillis();
		
		System.out.println("200 * PowerMethod = " + (to - time));
	}
	
	private Method getMethod() {
		
		return new PowerMethod3();
	}
}
