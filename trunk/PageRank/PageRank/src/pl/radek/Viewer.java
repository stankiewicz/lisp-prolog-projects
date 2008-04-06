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
import java.util.GregorianCalendar;
import java.util.HashMap;
import java.util.Map;
import java.util.Random;

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

import engine.Worm;

import pl.radek.computation.Method;
import pl.radek.computation.PowerMethod;
import javax.swing.JPasswordField;
import javax.swing.JLabel;

/**
 * @author Radek
 * 
 */
public class Viewer extends JFrame {

	private final ArrayList<Node> nodes = new ArrayList<Node>(); // @jve:decl-index=0:

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

	private String[] kw = new String[] { "k1", "k2", "k3", "k4", "k5" };

	private ArrayList<String> generateRandomKeywordList() {
		ArrayList<String> list = new ArrayList<String>();
		Random r = new Random(new GregorianCalendar().getTimeInMillis());
		for (String s : kw) {
			if (r.nextBoolean()) {
				list.add(s);
			}
		}
		return list;
	}

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

	private JTextField keywords;

	private JTextField urlFind = null;

	private JButton fillAll = null;

	private JScrollPane polaczeniaDoDanejStrony = null;

	private JTable tabelaPolaczenDoDanejStrony = null;

	private JLabel jLabelPolaczeniaDoWezla = null;

	private JLabel jLabelPolaczeniaZWezla = null;

	private JLabel jLabelWezly = null;

	private JLabel jLabelNazwa = null;

	private JLabel jLabelOd = null;

	private JLabel jLabelDo = null;

	private JLabel jLabeladresy = null;

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

	private double[] generatePersonalizationVector(ArrayList<Node> nodes) {
		String keywords = getKeywords().getText();
		keywords = keywords.trim();
		keywords = keywords.replaceAll("  ", " ");
		keywords = keywords.replaceAll("  ", " ");
		String[] kw = keywords.split(" ");
		double[] v = new double[nodes.size()];
		double sum = 0;
		for (int i = 0; i < v.length; ++i) {
			Node node = nodes.get(i);
			for (String key : kw) {
				if (node.getKeywords().contains(key)) {
					++v[i];
					sum++;
				}
			}
		}
		if (sum == 0) {
			for (int i = 0; i < v.length; ++i) {
				v[i] = 1.0 / v.length;
			}
		} else {
			for (int i = 0; i < v.length; ++i) {
				v[i] /= sum;
			}
		}
		return v;
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
					s[row][col] = 1.0 / nodes.size();
			} else {
				for (Node n : nodes.get(row).links) {
					int idx = nodes.indexOf(n);
					s[row][idx] = 1.0 / num;
				}
			}

		}

		double[] v;// = new double[nodes.size()];
		// TODO
		// for (int i = 0; i < v.length; ++i) {
		// v[i] = 1.0 / v.length;
		// }
		v = generatePersonalizationVector(nodes);
		gm.setAlfa(0.95);
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

				calculatePageRanks2(list);
				((NodesTableModel) getTabelaPolaczenZDanejStrony().getModel())
						.fireTableDataChanged();

				((NodesTableModel) getTabelaPolaczenDoDanejStrony().getModel())
						.fireTableDataChanged();
				((NodesTableModel) getTabelaStronZPolaczeniami().getModel())
						.fireTableDataChanged();

			}

			private void calculatePageRanks2(ArrayList<Node> list) {

				HashMap<Double, Integer> buckets = new HashMap<Double, Integer>();

				double max = 0;
				int idx = 0;
				int size = list.size();
				for (int i = 0; i < size; ++i) {
					if (buckets.containsKey(list.get(i).getPageRank())) {
						buckets.put(list.get(i).getPageRank(), buckets.get(list
								.get(i).getPageRank()) + 1);
					} else {
						buckets.put(list.get(i).getPageRank(), 1);
					}

				}
				int s = buckets.size();
				double[] ranking = new double[s];
				for (int i = 0; i < s; ++i) {
					max = 0;
					for (Double d : buckets.keySet()) {
						if (d > max) {
							max = d;
						}

					}
					buckets.remove(max);
					ranking[i] = max;
				}
				for (int i = 0; i < size; ++i) {
					double r = list.get(i).pageRank;
					for (int id = 0; id < s; ++id) {
						if (ranking[id] == r) {
							list.get(i).setRank(id + 1);
						}
					}
				}
			}

		});
		JMenuItem menuOpenFileItem = new JMenuItem("Otwórz");
		menuOpenFileItem.addActionListener(new ActionListener() {

			public void actionPerformed(ActionEvent e) {
				// TODO Auto-generated method stub

			}

		});

		JMenuItem menuSaveFileItem = new JMenuItem("Zapisz");
		menuSaveFileItem.addActionListener(new ActionListener() {

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
		// menuFile.add(menuOpenFileItem);
		// menuFile.add(menuSaveFileItem);
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
		this.setSize(new Dimension(859, 845));
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
			stronyZPolaczeniami.setBounds(new Rectangle(28, 129, 812, 190));
			stronyZPolaczeniami
					.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
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
			jLabeladresy = new JLabel();
			jLabeladresy.setBounds(new Rectangle(33, 682, 247, 23));
			jLabeladresy.setText("Adresy oddzielone spacjami");
			jLabelDo = new JLabel();
			jLabelDo.setBounds(new Rectangle(183, 48, 113, 21));
			jLabelDo.setText("Do");
			jLabelOd = new JLabel();
			jLabelOd.setBounds(new Rectangle(31, 47, 133, 20));
			jLabelOd.setText("Od");
			jLabelNazwa = new JLabel();
			jLabelNazwa.setBounds(new Rectangle(30, 0, 142, 31));
			jLabelNazwa.setText("Nazwa wêz³a");
			jLabelWezly = new JLabel();
			jLabelWezly.setBounds(new Rectangle(28, 110, 175, 19));
			jLabelWezly.setText("Tabela wêz³ów");
			jLabelPolaczeniaZWezla = new JLabel();
			jLabelPolaczeniaZWezla.setBounds(new Rectangle(30, 360, 180, 33));
			jLabelPolaczeniaZWezla.setText("Po³¹czenia z danego wêz³a");
			jLabelPolaczeniaDoWezla = new JLabel();
			jLabelPolaczeniaDoWezla.setBounds(new Rectangle(30, 525, 178, 20));
			jLabelPolaczeniaDoWezla.setText("Po³¹czenia do danego wêz³a");
			jPanel = new JPanel();
			jPanel.setLayout(null);
			jPanel.add(getStronyZPolaczeniami(), null);
			jPanel.add(getWyborStrony(), null);
			jPanel.add(getPolaczeniaZDanejStrony(), null);
			jPanel.add(getNazwaStrony(), null);
			jPanel.add(getDodajStrone(), null);
			jPanel.add(getKeywords(), null);
			jPanel.add(getOdCombo(), null);
			jPanel.add(getDoCombo(), null);
			jPanel.add(getDodajPolaczenie(), null);
			jPanel.add(getUsun(), null);
			jPanel.add(getUrlFind(), null);
			jPanel.add(getFillAll(), null);
			jPanel.add(getPolaczeniaDoDanejStrony(), null);
			jPanel.add(jLabelPolaczeniaDoWezla, null);
			jPanel.add(jLabelPolaczeniaZWezla, null);
			jPanel.add(jLabelWezly, null);
			jPanel.add(jLabelNazwa, null);
			jPanel.add(jLabelOd, null);
			jPanel.add(jLabelDo, null);
			jPanel.add(jLabeladresy, null);
			jPanel.add(odCombo, null);
			jPanel.add(doCombo, null);
			jPanel.add(wyborStrony, null);

		}
		return jPanel;
	}

	private JTextField getKeywords() {
		if (keywords == null) {
			keywords = new JTextField();
			keywords.setBounds(new Rectangle(350, 28, 249, 20));
			keywords.setVisible(false);
		}
		return keywords;
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
			wyborStrony.setLocation(new Point(30, 330));
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
					getTabelaPolaczenDoDanejStrony().setModel(
							new NodesTableModel(((Node) wyborStrony
									.getSelectedItem()).incomingLinks));
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
			polaczeniaZDanejStrony.setBounds(new Rectangle(30, 390, 812, 133));
			polaczeniaZDanejStrony
					.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
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
			nazwaStrony.setBounds(new Rectangle(27, 28, 154, 18));
		}
		return nazwaStrony;
	}

	private void fillWithData() {

		getTabelaStronZPolaczeniami().setModel(new NodesTableModel(nodes));

		getTabelaPolaczenZDanejStrony().setModel(new NodesTableModel());
		getTabelaPolaczenDoDanejStrony().setModel(new NodesTableModel());
		// wypelnianie combo wyboru strony
		// wyborStrony = getWyborStrony();
		// wyborStrony.setBounds(new Rectangle(30, 480, 226, 25));
		// odComboBox = getOdCombo();

		// JComboBox doo = getDoCombo();

		wyborStrony.removeAllItems();
		odCombo.removeAllItems();
		doCombo.removeAllItems();
		for (Node n : nodes) {
			wyborStrony.addItem(n);
			odCombo.addItem(n);
			doCombo.addItem(n);
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
					newNode.setKeywords(generateRandomKeywordList());
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
			odCombo.setBounds(new Rectangle(30, 75, 256, 31));
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
			doCombo.setBounds(new Rectangle(300, 75, 256, 31));
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
			dodajPolaczenie.setBounds(new Rectangle(570, 75, 121, 33));
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
			usun.setBounds(new Rectangle(705, 75, 135, 33));
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
	 * This method initializes urlFind
	 * 
	 * @return javax.swing.JTextField
	 */
	private JTextField getUrlFind() {
		if (urlFind == null) {
			urlFind = new JTextField();
			urlFind.setBounds(new Rectangle(30, 705, 809, 29));
		}
		return urlFind;
	}

	/**
	 * This method initializes fillAll
	 * 
	 * @return javax.swing.JButton
	 */
	private JButton getFillAll() {
		if (fillAll == null) {
			fillAll = new JButton();
			fillAll.setBounds(new Rectangle(30, 735, 241, 30));
			fillAll.setText("stwórz siatkê po³¹czeñ");
			fillAll.addActionListener(new java.awt.event.ActionListener() {
				public void actionPerformed(java.awt.event.ActionEvent e) {
					System.out.println("actionPerformed()"); // TODO
					// Auto-generated
					// Event stub
					// actionPerformed()
					// Worm w = new Worm();
					// nodes.clear();
					//
					// //nodes.addAll(w.start(getUrlFind().getText()));
					//					
					// nodes.addAll(Progress.showProgress(getUrlFind().getText()));
					// fillWithData();
					stworz();
				}
			});
		}
		return fillAll;
	}

	private void stworz() {

		Worm w = new Worm();
		nodes.clear();

		// nodes.addAll(w.start(getUrlFind().getText()));

		ArrayList<Node> lista = Progress.showProgress(getUrlFind().getText());
		if (lista != null) {
			nodes.addAll(lista);
			fillWithData();
		}
	}

	/**
	 * This method initializes polaczeniaDoDanejStrony
	 * 
	 * @return javax.swing.JScrollPane
	 */
	private JScrollPane getPolaczeniaDoDanejStrony() {
		if (polaczeniaDoDanejStrony == null) {
			polaczeniaDoDanejStrony = new JScrollPane();
			polaczeniaDoDanejStrony.setBounds(new Rectangle(30, 555, 812, 127));
			polaczeniaDoDanejStrony
					.setHorizontalScrollBarPolicy(JScrollPane.HORIZONTAL_SCROLLBAR_ALWAYS);
			polaczeniaDoDanejStrony
					.setViewportView(getTabelaPolaczenDoDanejStrony());
		}
		return polaczeniaDoDanejStrony;
	}

	/**
	 * This method initializes tabelaPolaczenDoDanejStrony
	 * 
	 * @return javax.swing.JTable
	 */
	private JTable getTabelaPolaczenDoDanejStrony() {
		if (tabelaPolaczenDoDanejStrony == null) {
			tabelaPolaczenDoDanejStrony = new JTable(new NodesTableModel());
		}
		return tabelaPolaczenDoDanejStrony;
	}

	/**
	 * @param args
	 */
	public static void main(String[] args) {
		// TODO Auto-generated method stub
		new Viewer();
	}

} // @jve:decl-index=0:visual-constraint="51,29"
