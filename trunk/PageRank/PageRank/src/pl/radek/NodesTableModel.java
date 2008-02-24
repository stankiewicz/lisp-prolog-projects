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
	String [] columns = new String [] { "Page Rank", "Nazwa", "Linki wychodz¹ce", "Linki wchodz¹ce", "S³owa kluczowe" , "Personalizacja"};
	public NodesTableModel(ArrayList<Node> nodes) {
		
		this.nodes.addAll(nodes);
	}
	
	/* (non-Javadoc)
	 * @see javax.swing.table.TableModel#getColumnCount()
	 */
	@Override
	public int getColumnCount() {
		return columns.length;
	}

	/* (non-Javadoc)
	 * @see javax.swing.table.TableModel#getRowCount()
	 */
	@Override
	public int getRowCount() {
		
		return nodes.size();
	}

	/* (non-Javadoc)
	 * @see javax.swing.table.TableModel#getValueAt(int, int)
	 */
	@Override
	public Object getValueAt(int row, int col) {
		Node node = nodes.get(row);
		switch(col) {
		case 0:
			return node.getPageRank();
		case 1:
			return node.getName();
		case 2:
			return node.getLinks().size();
		case 3:
			return node.getIncomingLinks().size();
		case 4:
			return node.getKeywords();
		case 5:
			return node.getPersonize();
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
