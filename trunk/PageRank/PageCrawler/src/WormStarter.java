

//import javax.swing.*;

import engine.Worm;
import javax.swing.*;

public class WormStarter {

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		System.out.println("HELLO THIS IS WORM :]");
		Worm worm = new Worm();
		String q = JOptionPane.showInputDialog(null);
		if( q != null ) {
			worm.startWormage(q);
		}
	}
}
