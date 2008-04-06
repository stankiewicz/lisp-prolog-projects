package engine;

import java.util.ArrayList;
import java.util.List;

import pl.radek.Node;

public class Worm {
	public Worm() {
		memory = new WormMemory();
	}
	public void startWormage( String urlName ) {
		System.out.println("Worm.startWormage() " + urlName);
		Runnable r = new WormReaderStarter(memory);
		memory.putNewLink( urlName );
		Thread t = new Thread(r);
		t.start();
	}
	
	public static void main(String []args){
		Worm w = new Worm();
		List<Node> list = w.start("http://www.mini.pw.edu.pl");
		System.out.println("Worm.main()" + list.size());
	}
	
	public ArrayList<Node> start(String urlName){
		String [] urls = urlName.split(" ");
		WormReaderStarter r = new WormReaderStarter(memory);
		for(String url: urls){
			memory.putNewLink(url);
		}
		r.run();
		
		
		ArrayList<Node> nodes = new ArrayList<Node>();
		for(String s:memory.getLinkToLinks().keySet()){
			if(memory.getInvalid().contains(s)){
				continue;
			}
			Node n = null;
			for(Node tmp: nodes){
				if(tmp.getName().equals(s)){
					n = tmp;
					break;
				}
			}
			if(n==null){
				n = new Node();
				nodes.add(n);
			}			
			
			n.setName(s);
			List<String> list = memory.getLinkToKeywords().get(s);
			n.setKeywords(list);
			ArrayList<Node> li = new ArrayList<Node>();
			for(String link: memory.getLinkToLinks().get(s)){
				if(memory.getInvalid().contains(link)){
					continue;
				}
				Node found = null;
				for(Node tmp: nodes){
					if(tmp.getName().equals(link)){
						found = tmp;
						break;
					}
				}
				if(found == null){
					found = new Node();
					found.setName(link);
					nodes.add(found);
				}
				li.add(found);
				found.getIncomingLinks().add(n);
			}
			n.setLinks(li);
			System.out.println("link: "+ s + " kw: "+ list+" - " + memory.getLinkToLinks().get(s));
		}
		System.out.println("invalid: " + memory.getInvalid());
		for(Node n: nodes){
			n.getIncomingLinks().remove(n);
			n.getLinks().remove(n);
		}
		return nodes;
	}
	
	
	
	private WormMemory memory;
}
