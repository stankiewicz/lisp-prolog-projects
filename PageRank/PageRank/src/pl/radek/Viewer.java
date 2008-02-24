/**
 * 
 */
package pl.radek;

import java.awt.Dimension;
import java.awt.Point;
import java.awt.Rectangle;
import java.awt.event.ActionEvent;
import java.awt.event.ActionListener;
import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.Map;

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

import pl.radek.computation.Method;
import pl.radek.computation.PowerMethod;

/**
 * @author Radek
 * 
 */
public class Viewer extends JFrame {

	private final ArrayList<Node> nodes = new ArrayList<Node>();

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

	private class procKiller extends Thread {
		ProcessBuilder pb;
		Process proc;

		public procKiller(ProcessBuilder proc) {
			this.pb = proc;

		}

		public void killProc() {
			proc.destroy();

		}

		@Override
		public void run() {
			super.run();
			proc = null;
			try {
				proc = pb.start();
			} catch (IOException e1) {
				// TODO Auto-generated catch block
				e1.printStackTrace();
				return;
			}
			do {
				try {
					try {
						Thread.sleep(1000);
					} catch (InterruptedException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
					int i = proc.exitValue();

					System.out.println("returned: " + i);
					return;
				} catch (IllegalThreadStateException ex) {

				}
			} while (true);
		}

	}

	private procKiller procKiller;

	private void prepareFile() {
		OutputStream stream = null;
		File nodes = new File("run\\tmp.lua").getAbsoluteFile();
		if (nodes.exists() == false) {
			try {
				nodes.createNewFile();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		try {
			stream = new FileOutputStream(nodes, false);
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		writeFile(stream);
		try {
			stream.flush();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		try {
			stream.close();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	private void writeFile(OutputStream stream) {

		StringBuffer sb = new StringBuffer();
		Map<Node, String> map = new HashMap<Node, String>();
		int i = 0;
		String fNewLine = System.getProperty("line.separator");
		sb.append("n3d.begin_graph();");
		sb.append(fNewLine);
		for (Node node : nodes) {
			String id = "id" + i;
			i++;
			map.put(node, id);
			sb.append(id + "=n3d.add_node(\"" + node.name + "\")");
			sb.append(fNewLine);
		}
		for (Node node : nodes) {
			String idFrom = map.get(node);
			for (Node to : node.links) {
				String idTo = map.get(to);
				sb.append("n3d.add_edge_by_ids(" + idFrom + "," + idTo + ")");
				sb.append(fNewLine);

			}

		}

		sb.append("n3d.end_graph();");
		sb.append(fNewLine);
		try {
			stream.write(sb.toString().getBytes("UTF-8"));
		} catch (UnsupportedEncodingException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}

	private GoogleMatrix generateMatrix(ArrayList<Node> nodes) {
		GoogleMatrix gm = new GoogleMatrix();
		double[][] s = new double[nodes.size()][];
		for (int row = 0; row < nodes.size(); ++row) {
			s[row] = new double[nodes.size()];
			for (int col = 0; col < nodes.size(); ++col)
				s[row][col] = 0;
			double num = nodes.get(row).links.size();
			if (num == 0) {
				for (int col = 0; col < nodes.size(); ++col)
					s[row][col] = 1.0/nodes.size();
			} else {
				for (Node n : nodes.get(row).links) {
					int idx = nodes.indexOf(n);
					s[row][idx] = 1.0 / num;
				}
			}

		}

		double[] v = new double[nodes.size()];
		// TODO
		for (int i = 0; i < v.length; ++i) {
			v[i] = 1.0 / v.length;
		}

		gm.setAlfa(0.85);
		gm.setS(s);
		gm.setV(v);

		return gm;
	}

	public Viewer() {
		super("viewer");
		initialize();
		setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);

		JMenuBar menuBar = new JMenuBar();
		JMenu menuFile = new JMenu("Plik");
		JMenu menuVisualize = new JMenu("Wizualizacja");
		JMenu menuRank = new JMenu("Page Rank");
		JMenuItem menuCountRankItem = new JMenuItem("Przelicz ranking");
		menuCountRankItem.addActionListener(new ActionListener() {

			@SuppressWarnings("unchecked")
			@Override
			public void actionPerformed(ActionEvent e) {
				System.out.println("Przeliczam ranking");
				ArrayList<Node> list = (ArrayList<Node>) nodes.clone();
				GoogleMatrix gm = generateMatrix(list);

				Method method = new PowerMethod();

				double[] pageRank = method.computeEigenVector(gm);
				if (pageRank.length != list.size()) {
					System.out.println("dlugosc page rank sie nie zgadza");
					return;
				}

				for (int i = 0; i < pageRank.length; ++i) {
					list.get(i).setPageRank(pageRank[i]);
				}

				double max = 0;
				int idx = 0;
				int size = list.size();
				for (int i = 0; i < size; ++i) {
					max = 0;
					for (int j = 0; j < list.size(); ++j) {
						if (list.get(j).getPageRank() > max) {
							max = list.get(j).getPageRank();
							idx = j;
						}
					}
					Node n = list.get(idx);
					n.setRank(i + 1);
					list.remove(n);
				}
				((NodesTableModel) getTabelaPolaczenZDanejStrony().getModel())
						.fireTableDataChanged();

				((NodesTableModel) getTabelaStronZPolaczeniami().getModel())
						.fireTableDataChanged();

			}

		});
		JMenuItem menuOpenFileItem = new JMenuItem("Otwórz");
		menuOpenFileItem.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {
				// TODO Auto-generated method stub

			}

		});

		JMenuItem menuSaveFileItem = new JMenuItem("Zapisz");
		menuSaveFileItem.addActionListener(new ActionListener() {

			@Override
			public void actionPerformed(ActionEvent e) {
				// TODO Auto-generated method stub

			}

		});

		JMenuItem menuCloseVisualizeItem = new JMenuItem("Zamknij wizualizacje");
		menuCloseVisualizeItem.addActionListener(new ActionListener() {

			public void actionPerformed(ActionEvent e) {
				if (procKiller != null) {
					procKiller.killProc();
				}

			}

		});
		JMenuItem menuVisualizeItem = new JMenuItem("Poka¿");
		menuVisualizeItem.addActionListener(new ActionListener() {

			public void actionPerformed(ActionEvent e) {
				prepareFile();
				File nodes = new File("run\\nodes3d.exe").getAbsoluteFile();
				System.out.println("uruchamiam " + nodes);
				ProcessBuilder pb = new ProcessBuilder(nodes.toString(),
						"tmp.lua");

				pb.directory(new File("run\\").getAbsoluteFile());
				procKiller = new procKiller(pb);
				procKiller.start();
				// } catch (IOException e1) {
				// // TODO Auto-generated catch block
				// e1.printStackTrace();
				// }
			}
		});
		JMenuItem menuItem = new JMenuItem("Exit");
		menuItem.addActionListener(new ActionListener() {
			public void actionPerformed(ActionEvent e) {
				System.exit(0);
			}
		});
		menuRank.add(menuCountRankItem);
		menuVisualize.add(menuVisualizeItem);
		menuVisualize.add(menuCloseVisualizeItem);
		menuFile.add(menuOpenFileItem);
		menuFile.add(menuSaveFileItem);
		menuFile.add(menuItem);
		menuBar.add(menuFile);
		menuBar.add(menuVisualize);
		menuBar.add(menuRank);
		setJMenuBar(menuBar);
		System.out.println(getJPanel().getMinimumSize());
		// this.setSize(getJPanel().getSize());
		// this.setPreferredSize(getJPanel().getSize());
		// this.setMinimumSize(getJPanel().getSize());
		// pack();
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
			tabelaStronZPolaczeniami = new JTable(new NodesTableModel());
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
					if (wyborStrony.getSelectedItem() == null)
						return;
					System.out.println("####wybrano strone "
							+ wyborStrony.getSelectedItem()); // TODO
					// Auto-generated
					// Event stub
					// actionPerformed()

					getTabelaPolaczenZDanejStrony().setModel(
							new NodesTableModel(((Node) wyborStrony
									.getSelectedItem()).links));
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
			polaczeniaZDanejStrony
					.setViewportView(getTabelaPolaczenZDanejStrony());
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
			tabelaPolaczenZDanejStrony = new JTable(new NodesTableModel());
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

	private void fillWithData() {

		getTabelaStronZPolaczeniami().setModel(new NodesTableModel(nodes));

		getTabelaPolaczenZDanejStrony().setModel(new NodesTableModel());
		// wypelnianie combo wyboru strony
		JComboBox wybor = getWyborStrony();
		JComboBox od = getOdCombo();
		JComboBox doo = getDoCombo();
		wybor.removeAllItems();
		od.removeAllItems();
		doo.removeAllItems();
		for (Node n : nodes) {
			wybor.addItem(n);
			od.addItem(n);
			doo.addItem(n);
		}

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
					if (getNazwaStrony().getText() == null
							|| getNazwaStrony().getText().equals("")) {
						System.out.println("pusta nazwa strony");
						return;
					}

					Node newNode = new Node();
					newNode.setName(getNazwaStrony().getText());
					nodes.add(newNode);

					fillWithData();
					System.out.println("nodes - dlugosc " + nodes.size());
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
			dodajPolaczenie
					.addActionListener(new java.awt.event.ActionListener() {
						public void actionPerformed(java.awt.event.ActionEvent e) {
							Node from = (Node) getOdCombo().getSelectedItem();
							Node to = (Node) getDoCombo().getSelectedItem();
							if (from == null || to == null) {
								System.out.println("nullowa wartosc od:" + from
										+ " , do: " + to);
								return;
							}

							if (from.links.contains(to)) {
								System.out.println("juz jest link od:" + from
										+ " , do: " + to);
								return;
							}

							from.links.add(to);
							to.incomingLinks.add(from);
							((NodesTableModel) getTabelaPolaczenZDanejStrony()
									.getModel()).fireTableDataChanged();

							((NodesTableModel) getTabelaStronZPolaczeniami()
									.getModel()).fireTableDataChanged();
							if (getWyborStrony().getSelectedItem().equals(from)) {

								getTabelaPolaczenZDanejStrony().setModel(
										new NodesTableModel(from.getLinks()));
							}

							System.out.println("nodes - dlugosc "
									+ nodes.size());
						}
					});
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
			usun.addActionListener(new java.awt.event.ActionListener() {
				public void actionPerformed(java.awt.event.ActionEvent e) {
					Node from = (Node) getOdCombo().getSelectedItem();
					Node to = (Node) getDoCombo().getSelectedItem();
					if (from == null || to == null) {
						System.out.println("nullowa wartosc od:" + from
								+ " , do: " + to);
						return;
					}

					if (from.links.contains(to)) {
						from.links.remove(to);
						to.incomingLinks.remove(from);
						((NodesTableModel) getTabelaPolaczenZDanejStrony()
								.getModel()).fireTableDataChanged();

						((NodesTableModel) getTabelaStronZPolaczeniami()
								.getModel()).fireTableDataChanged();
						if (getWyborStrony().getSelectedItem().equals(from)) {

							getTabelaPolaczenZDanejStrony().setModel(
									new NodesTableModel(from.getLinks()));
						}

					} else {
						System.out.println("nie ma takiego polaczenia od:"
								+ from + " , do: " + to);

					}
				}
			});
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

} // @jve:decl-index=0:visual-constraint="51,29"
