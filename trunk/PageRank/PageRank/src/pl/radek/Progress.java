package pl.radek;

import java.awt.Rectangle;
import java.util.ArrayList;

import javax.swing.JButton;
import javax.swing.JDialog;
import javax.swing.JFrame;
import javax.swing.JLabel;
import javax.swing.JPanel;
import javax.swing.JProgressBar;

import engine.Worm;

public class Progress extends JPanel implements Runnable{

	private static final long serialVersionUID = 1L;
	private JProgressBar jProgressBar = null;
	private JLabel jLabel = null;
	private JButton jButton = null;

	/**
	 * This is the default constructor
	 */
	public Progress() {
		super();
		initialize();
	}

	/**
	 * This method initializes this
	 * 
	 * @return void
	 */
	private void initialize() {
		jLabel = new JLabel();
		jLabel.setBounds(new Rectangle(133, 13, 113, 27));
		jLabel.setText("Tworzenie siatki");
		this.setSize(396, 222);
		this.setLayout(null);
		this.add(getJProgressBar(), null);
		this.add(jLabel, null);
		this.add(getJButton(), null);
	}

	/**
	 * This method initializes jProgressBar	
	 * 	
	 * @return javax.swing.JProgressBar	
	 */
	private JProgressBar getJProgressBar() {
		if (jProgressBar == null) {
			jProgressBar = new JProgressBar();
			jProgressBar.setBounds(new Rectangle(12, 46, 368, 26));
			jProgressBar.setIndeterminate(true);
		}
		return jProgressBar;
	}
	ArrayList<Node> list;
	String addr;
	JDialog d;
	protected void runWorm(String addr, JDialog d){
		
		this.addr = addr;
		this.d = d;
		
	}
	Thread thread;  //  @jve:decl-index=0:
	
	public static ArrayList<Node> showProgress(String  addr){
		
		JDialog jf = new JDialog();
		
		
		Progress pg = new Progress();
		pg.runWorm(addr, jf);
		pg.thread = new Thread(pg);
		
		jf.setSize(pg.getSize());
		jf.setPreferredSize(pg.getSize());
		jf.setMaximumSize(pg.getSize());
		jf.setContentPane(pg);
		pg.thread.start();
		jf.setModal(true);
		jf.setVisible(true);
		
		return pg.list;
	}

	/**
	 * This method initializes jButton	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getJButton() {
		if (jButton == null) {
			jButton = new JButton();
			jButton.setBounds(new Rectangle(124, 87, 131, 22));
			jButton.setText("Przerwij");
			jButton.addActionListener(new java.awt.event.ActionListener() {
				public void actionPerformed(java.awt.event.ActionEvent e) {
					System.out.println("actionPerformed()"); // TODO Auto-generated Event stub actionPerformed()
					thread.interrupt();
					Progress.this.d.setVisible(false);
				}
			});
		}
		return jButton;
	}

	public void run() {
		try {
			thread.sleep(1000);
		} catch (InterruptedException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		Worm w = new Worm();
		list= w.start(addr);
		d.setVisible(false);
		System.out.println("koncze");
	}

}  //  @jve:decl-index=0:visual-constraint="10,10"
