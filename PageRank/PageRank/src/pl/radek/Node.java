package pl.radek;

import java.util.ArrayList;

public class Node {
	String name;
	double personize;
	ArrayList<String> keywords;
	ArrayList<Node> links;
	double pageRank;
	ArrayList<Node> incomingLinks;
	public String getName() {
		return name;
	}
	public void setName(String name) {
		this.name = name;
	}
	public double getPersonize() {
		return personize;
	}
	public void setPersonize(double personize) {
		this.personize = personize;
	}
	public ArrayList<String> getKeywords() {
		return keywords;
	}
	public void setKeywords(ArrayList<String> keywords) {
		this.keywords = keywords;
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
