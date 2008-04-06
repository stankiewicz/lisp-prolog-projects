package engine;

import java.util.*;
import java.util.concurrent.*;

import pl.radek.Node;
import util.*;

public class WormMemory {
	private ConcurrentHashMap<String, List<String>> linkToKeywords;
	private ConcurrentHashMap<String, List<String>> linkToLinks;
	private List<String> invalid;
	public ConcurrentHashMap<String, List<String>> getLinkToKeywords() {
		return linkToKeywords;
	}

	public ConcurrentHashMap<String, List<String>> getLinkToLinks() {
		return linkToLinks;
	}

	public WormMemory() {
		visitedLinks = new ConcurrentHashMap< String, Object >();
		linkToKeywords = new ConcurrentHashMap< String, List<String> >();
		linkToLinks= new ConcurrentHashMap< String, List<String> >();
		wordsIndex = new ConcurrentHashMap< String, LinkedList< LinkAndPriority > >();
		notVisitedLinks = Collections.synchronizedList( new LinkedList< String >() );
		invalid = Collections.synchronizedList( new LinkedList< String >() );
		nodes = new ConcurrentHashMap<String, Node>();
	}
	
	public List<String> getInvalid() {
		return invalid;
	}

	public void putVisitedLink( String link ) {
		visitedLinks.put( link, new Object());
	}
	
	public boolean isLinkVisited( String link ) {
		return visitedLinks.containsKey(link);
	}
	
	public void putNewLink( String link ) {
		if( isLinkVisited( link ) == false && notVisitedLinks.contains( link ) == false ) {
			notVisitedLinks.add( link );
		}
	}
	
	public String getNewLink() {
		if( !notVisitedLinks.isEmpty() ) {
			String link = notVisitedLinks.remove(0);
			putVisitedLink( link );
			return link;
		}
		else { return null; }
	}
	
	public void putWord( String word, String link, int priority ) {
		//System.out.println(" PUTWORD " + word + " " + link + " " + priority);
		if( wordsIndex.containsKey( word ) ) {
			wordsIndex.get( word ).add( new LinkAndPriority(link,priority) );
			//System.out.println( wordsIndex.get(word).size() );
			//System.out.println("w put word rozmiar to: " + wordsIndex.get(word).size() );
		}
		else {
			//System.out.println( "PUT WORD ELSE" );
			LinkedList< LinkAndPriority > newList = new LinkedList< LinkAndPriority >();
			newList.add( new LinkAndPriority(link,priority));
			wordsIndex.put( word, newList );
		}
	}
	
	public Object[] query( String word ) {
		LinkedList< LinkAndPriority > a = wordsIndex.get( word );
		if( a == null ) { return null; }
		//System.out.println(a.size());
		Object[] array = a.toArray();
		Arrays.sort( array, new LinkAndPriComparator() );
		return array;
	}
	
	public synchronized Node tryAddGetNode(String link){
		if(nodes.containsKey(link)){
			return nodes.get(link);
		}else {
			Node n = new Node();
			n.setName(link);
			nodes.put(link, n);
			return n;
		}
		
	}
	
	private ConcurrentHashMap<String, Node> nodes;
	private ConcurrentHashMap< String, Object > visitedLinks;
	private ConcurrentHashMap< String, LinkedList< LinkAndPriority > > wordsIndex;
	private List< String > notVisitedLinks;
	public ConcurrentHashMap<String, Object> getVisitedLinks() {
		return visitedLinks;
	}
}