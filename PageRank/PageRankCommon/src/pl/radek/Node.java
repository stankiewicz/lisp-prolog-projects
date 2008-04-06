package pl.radek;

import java.util.ArrayList;
import java.util.List;

public class Node {
	String name;
	double personalize;
	List<String> keywords;
	ArrayList<Node> links;
	double pageRank;
	ArrayList<Node> incomingLinks;
	boolean visited = false;
	int rank;
	
	
	public boolean isVisited() {
		return visited;
	}

	public void setVisited(boolean visited) {
		this.visited = visited;
	}

	public int getRank() {
		return rank;
	}

	public void setRank(int rank) {
		this.rank = rank;
	}

	public Node() {
		keywords = new ArrayList<String>();
		incomingLinks = new ArrayList<Node>();
		links = new ArrayList<Node>();
		pageRank = Double.MAX_VALUE;
		personalize = Double.MAX_VALUE;
		rank = Integer.MAX_VALUE;
	}
	
	@Override
	public String toString() {
		// TODO Auto-generated method stub
		return name + " " +(rank==Integer.MAX_VALUE ? "":rank);
	}

	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public double getPersonalize() {
		return personalize;
	}
	public void setPersonalize(double personize) {
		this.personalize = personize;
	}
	public List<String> getKeywords() {
		return keywords;
	}
	public void setKeywords(List<String> name2) {
		this.keywords = name2;
	}
	public ArrayList<Node> getLinks() {
		return links;
	}
	public void setLinks(ArrayList<Node> links) {
		this.links = links;
	}
	public double getPageRank() {
		return pageRank;
	}
	public void setPageRank(double pageRank) {
		this.pageRank = pageRank;
	}
	public ArrayList<Node> getIncomingLinks() {
		return incomingLinks;
	}
	public void setIncomingLinks(ArrayList<Node> incomingLinks) {
		this.incomingLinks = incomingLinks;
	}
}
