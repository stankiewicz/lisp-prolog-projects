/**
 * 
 */
package pl.radek;

import java.util.ArrayList;

import javax.swing.table.AbstractTableModel;

/**
 * @author Radek
 *
 */
public class NodesTableModel extends AbstractTableModel {

	/**
	 * 
	 */
	private static final long serialVersionUID = -2432705830445647773L;
	ArrayList<Node> nodes = new ArrayList<Node>();
	String [] columns = new String [] { "Nazwa", "Linki wychodz�ce", "Linki wchodz�ce", "S�owa kluczowe" , "Personalizacja","Page Rank","Rank"};
	public NodesTableModel(ArrayList<Node> nodes) {
		
		this.nodes.addAll(nodes);
	}
	
	public NodesTableModel() {
		// TODO Auto-generated constructor stub
	}

	/* (non-Javadoc)
	 * @see javax.swing.table.TableModel#getColumnCount()
	 */
	
	public int getColumnCount() {
		return columns.length;
	}

	/* (non-Javadoc)
	 * @see javax.swing.table.TableModel#getRowCount()
	 */
	
	public int getRowCount() {
		
		return nodes.size();
	}

	/* (non-Javadoc)
	 * @see javax.swing.table.TableModel#getValueAt(int, int)
	 */
	
	public Object getValueAt(int row, int col) {
		Node node = nodes.get(row);
		switch(col) {
		case 0:
			return node.getName();
		case 1:
			return node.getLinks().size();
		case 2:
			return node.getIncomingLinks().size();
		case 3:
			return node.getKeywords();
		case 4:
			return node.getPersonalize()== Double.MAX_VALUE?"Nieustalone":node.getPersonalize();
		case 5:
			return node.getPageRank()== Double.MAX_VALUE?"Nieustalone":node.getPageRank();
		case 6:
			return node.getRank()== Integer.MAX_VALUE?"Nieustalone":node.getRank();
		}
		return null;
	}

	@Override
	public String getColumnName(int arg0) {
		
		return columns[arg0];
	}
	
	public void addNode(Node node) {
		nodes.add(node);
		fireTableDataChanged();
	}
	
	public Node getNodeAt(int row) {
		return nodes.get(row);
	}
}
