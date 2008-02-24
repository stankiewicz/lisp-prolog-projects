/**
 * 
 */
package pl.radek;

import java.awt.Dimension;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;

import javax.swing.JButton;
import javax.swing.JComboBox;
import javax.swing.JFrame;
import javax.swing.JMenu;
import javax.swing.JMenuBar;
import javax.swing.JMenuItem;
import javax.swing.JPanel;
import javax.swing.JScrollPane;
import javax.swing.JTable;
import javax.swing.JTextField;

/**
 * @author Radek
 *
 */
public class Viewer extends JFrame {

	
	
	/**
	 * 
	 */
	private static final long serialVersionUID = 8502377759600889595L;
	private JScrollPane stronyZPolaczeniami = null;
	private JTable tabelaStronZPolaczeniami = null;
	private JPanel jPanel = null;
	private JComboBox wyborStrony = null;
	private JScrollPane polaczeniaZDanejStrony = null;
	private JTable tabelaPolaczenZDanejStrony = null;
	private JTextField nazwaStrony = null;
	private JButton dodajStrone = null;
	private JComboBox odCombo = null;
	private JComboBox doCombo = null;
	private JButton dodajPolaczenie = null;
	private JButton usun = null;
	public Viewer(){
		super("viewer");
		initialize();
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		
		JMenuBar menuBar = new JMenuBar();
		JMenu menu = new JMenu("File");
		JMenuItem menuItem = new JMenuItem("Exit");
		menuItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				System.exit(0);
			}
		});
		menu.add(menuItem);
		menuBar.add(menu);
		setJMenuBar(menuBar);
		System.out.println(getJPanel().getMinimumSize());
		//this.setSize(getJPanel().getSize());
		//this.setPreferredSize(getJPanel().getSize());
		//this.setMinimumSize(getJPanel().getSize());
		//pack();
		setVisible(true);
		
	}
	
	
	/**
	 * This method initializes this
	 * 
	 */
	private void initialize() {
        this.setSize(new Dimension(761, 612));
        this.setTitle("Viewer");
        this.setContentPane(getJPanel());
			
	}


	/**
	 * This method initializes stronyZPolaczeniami	
	 * 	
	 * @return javax.swing.JScrollPane	
	 */
	private JScrollPane getStronyZPolaczeniami() {
		if (stronyZPolaczeniami == null) {
			stronyZPolaczeniami = new JScrollPane();
			stronyZPolaczeniami.setBounds(new Rectangle(28, 112, 687, 190));
			stronyZPolaczeniami.setViewportView(getTabelaStronZPolaczeniami());
		}
		return stronyZPolaczeniami;
	}


	/**
	 * This method initializes tabelaStronZPolaczeniami	
	 * 	
	 * @return javax.swing.JTable	
	 */
	private JTable getTabelaStronZPolaczeniami() {
		
		// 1 kolumna - nazwa strony
		// 2 kolumna - ilosc stron wychodzacych
		// 3 kolumna - ilosc stron wchodzacych
		
		if (tabelaStronZPolaczeniami == null) {
			tabelaStronZPolaczeniami = new JTable();
		}
		return tabelaStronZPolaczeniami;
	}


	/**
	 * This method initializes jPanel	
	 * 	
	 * @return javax.swing.JPanel	
	 */
	private JPanel getJPanel() {
		if (jPanel == null) {
			jPanel = new JPanel();
			jPanel.setLayout(null);
			jPanel.add(getStronyZPolaczeniami(), null);
			jPanel.add(getWyborStrony(), null);
			jPanel.add(getPolaczeniaZDanejStrony(), null);
			jPanel.add(getNazwaStrony(), null);
			jPanel.add(getDodajStrone(), null);
			jPanel.add(getOdCombo(), null);
			jPanel.add(getDoCombo(), null);
			jPanel.add(getDodajPolaczenie(), null);
			jPanel.add(getUsun(), null);
		}
		return jPanel;
	}


	/**
	 * This method initializes wyborStrony	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getWyborStrony() {
		if (wyborStrony == null) {
			wyborStrony = new JComboBox();
			wyborStrony.setSize(new Dimension(304, 31));
			wyborStrony.setLocation(new Point(28, 305));
			wyborStrony.addActionListener(new java.awt.event.ActionListener() {
				public void actionPerformed(java.awt.event.ActionEvent e) {
					System.out.println("actionPerformed()"); // TODO Auto-generated Event stub actionPerformed()
				}
			});
		}
		return wyborStrony;
	}


	/**
	 * This method initializes polaczeniaZDanejStrony	
	 * 	
	 * @return javax.swing.JScrollPane	
	 */
	private JScrollPane getPolaczeniaZDanejStrony() {
		if (polaczeniaZDanejStrony == null) {
			polaczeniaZDanejStrony = new JScrollPane();
			polaczeniaZDanejStrony.setBounds(new Rectangle(27, 340, 691, 133));
			polaczeniaZDanejStrony.setViewportView(getTabelaPolaczenZDanejStrony());
		}
		return polaczeniaZDanejStrony;
	}


	/**
	 * This method initializes tabelaPolaczenZDanejStrony	
	 * 	
	 * @return javax.swing.JTable	
	 */
	private JTable getTabelaPolaczenZDanejStrony() {
		if (tabelaPolaczenZDanejStrony == null) {
			tabelaPolaczenZDanejStrony = new JTable();
		}
		return tabelaPolaczenZDanejStrony;
	}


	/**
	 * This method initializes nazwaStrony	
	 * 	
	 * @return javax.swing.JTextField	
	 */
	private JTextField getNazwaStrony() {
		if (nazwaStrony == null) {
			nazwaStrony = new JTextField();
			nazwaStrony.setBounds(new Rectangle(27, 28, 149, 20));
		}
		return nazwaStrony;
	}


	/**
	 * This method initializes dodajStrone	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getDodajStrone() {
		if (dodajStrone == null) {
			dodajStrone = new JButton();
			dodajStrone.setBounds(new Rectangle(191, 27, 148, 22));
			dodajStrone.setText("Dodaj Nowa Strone");
			dodajStrone.addActionListener(new java.awt.event.ActionListener() {
				public void actionPerformed(java.awt.event.ActionEvent e) {
					System.out.println("actionPerformed()"); // TODO Auto-generated Event stub actionPerformed()
				}
			});
		}
		return dodajStrone;
	}


	/**
	 * This method initializes odCombo	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getOdCombo() {
		if (odCombo == null) {
			odCombo = new JComboBox();
			odCombo.setBounds(new Rectangle(28, 77, 188, 25));
		}
		return odCombo;
	}


	/**
	 * This method initializes doCombo	
	 * 	
	 * @return javax.swing.JComboBox	
	 */
	private JComboBox getDoCombo() {
		if (doCombo == null) {
			doCombo = new JComboBox();
			doCombo.setBounds(new Rectangle(253, 78, 188, 25));
		}
		return doCombo;
	}


	/**
	 * This method initializes dodajPolaczenie	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getDodajPolaczenie() {
		if (dodajPolaczenie == null) {
			dodajPolaczenie = new JButton();
			dodajPolaczenie.setBounds(new Rectangle(448, 74, 135, 33));
			dodajPolaczenie.setText("dodaj po³¹czenie");
		}
		return dodajPolaczenie;
	}


	/**
	 * This method initializes usun	
	 * 	
	 * @return javax.swing.JButton	
	 */
	private JButton getUsun() {
		if (usun == null) {
			usun = new JButton();
			usun.setBounds(new Rectangle(592, 74, 135, 33));
			usun.setText("usuñ po³¹czenie");
		}
		return usun;
	}


	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		new Viewer();
	}

}  //  @jve:decl-index=0:visual-constraint="51,29"
