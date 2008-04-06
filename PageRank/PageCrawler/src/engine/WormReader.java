package engine;

import java.net.*;
import java.io.*;
import java.util.*;

//import util.*;

public class WormReader implements Runnable {
	public WormReader(String url, WormMemory mem, WormReaderStarter s) {
		urlName = url;
		memory = mem;
		starter = s;
		stack = new Stack<Integer>();
		stack.push(new Integer(DEFAULT_SIZE));
	}

	public void run() {
		// TODO Auto-generated method stub
		try {
			System.out.println("WormReader.run() " + urlName);
			URL url = new URL(urlName);
			URLConnection connection = url.openConnection();
			connection.setConnectTimeout(1000);
			connection.connect();
			Scanner in = new Scanner(connection.getInputStream());
			String page = "";
			while (in.hasNextLine()) {
				page += in.nextLine();
				page += '\n';
			}
			page = page.toLowerCase();
			page = page.replaceAll("&nbsp", "");
			// System.out.println(page);
			Vector<String> linksFound = parseForLinks(page);
			ArrayList<String> kw = parseForKeyWords(page);
			// parseForWordsInTitle(page);
			// parseForWordsInHeaders(page);
			// parseForWordsInText(page);
			memory.getLinkToKeywords().put(urlName, kw);
			for (int i = 0; i < linksFound.size(); ++i) {
				memory.putNewLink(linksFound.elementAt(i));
			}
		} catch (MalformedURLException e) {
			memory.getInvalid().add(urlName);
			System.out.println(e);
		} catch (IOException e) {
			System.out.println(e);
			memory.getInvalid().add(urlName);
		}
		starter.V();
	}

	private ArrayList<String> parseForKeyWords(String page) {
		page = page.toLowerCase();
		ArrayList<String> kw = new ArrayList<String>();
		int from, to;
		int d;
		from = page.indexOf("<meta name=\"Keywords\" content=\"");
		d="<meta name=\"Keywords\" content=\"".length();
		if(from ==-1){
			from = page.indexOf("<meta name=\"keywords\" content=\"");
			d = "<meta name=\"keywords\" content=\"".length();
		}
		
		if(from==-1) return kw;
		to = page.indexOf("\"",from + d);
		
		if(to == -1) return kw;
		
		String key = page.substring(from + d, to);
		System.out.println(key);
		String [] tab;
		if(key.indexOf(",")!=-1){
			tab = key.split(",");
		}else{
		 tab = key.split(" ");
		}
		for(String tmp1:tab){
			if(tmp1!=null && tmp1.length()!=0)
			kw.add(tmp1.trim());
		}
		return kw;
	}

	private Vector<String> parseForLinks(String page) {
		Vector<String> links = new Vector<String>();
		int indexBeg = 0;
		int indexEnd = 0;
		String link;
		boolean bad = false;
		while (indexBeg++ != -1) { // dopoki znajduje jakies stringi zakonczone
									// na .html
			indexBeg = page.indexOf("<a href=", indexBeg);

			if (indexBeg == -1)
				continue;
			if (page.substring(indexBeg + 8, indexBeg + 9).equals("\"")) {
				bad = false;
			} else if (page.substring(indexBeg + 8, indexBeg + 9).equals("'")) {
				bad = true;
			} else {
				continue;
			}
			indexEnd = page.indexOf(bad ? "'" : "\"", indexBeg + 9);
			if (indexEnd == -1)
				continue;
			try {
				link = page.substring(indexBeg + 9, indexEnd);
				// System.out.println("WormReader.parseForLinks() " + link);
				// if( indexBeg != - 1 && (link.contains(".html") ||
				// (link.contains(".html")))) {
				if (link.contains("http://")) {
					link = link.substring("http://".length());
					if (link.indexOf("/") != -1) {
						link = link.substring(0, link.indexOf("/"));

					}
					link = "http://" + link;

				} else {

					continue;
				}
				// else {
				// link = urlName+link;
				// }
				// System.out.print(indexBeg + " " + link);

				// }
				links.add(link);
			} catch (StringIndexOutOfBoundsException e) {
			}
		}
		ArrayList<String> tmp = new ArrayList<String>();
		Vector<String> ret = new Vector<String>();
		for (String link1 : links) {
			if (tmp.contains(link1))
				continue;
			tmp.add(link1);
		}
		memory.getLinkToLinks().put(urlName, tmp);
		for (String link1 : tmp) {
			if (memory.isLinkVisited(link1) == false) {
				ret.addElement(link1);
				//System.out.println("WormReader.parseForLinks() ####" + link1);
			}
		}
		return ret;
	}

	// private Vector<String> parseForLinks( String page ) {
	// Vector< String > links = new Vector< String >();
	// int indexBeg=0;
	// int indexEnd=0;
	// String link;
	// while( indexBeg++ != -1 ) { //dopoki znajduje jakies stringi zakonczone
	// na .html
	// indexBeg = page.indexOf("<a href=",indexBeg);
	// indexEnd = page.indexOf("\"",indexBeg+9);
	// try {
	// link = page.substring(indexBeg+9,indexEnd);
	// if( indexBeg != - 1 && (link.contains(".html") ||
	// (link.contains(".html")))) {
	// if( link.contains("http://") ) {
	// //foo
	// }
	// else {
	// link = urlName+link;
	// }
	// //System.out.print(indexBeg + " " + link);
	// if( memory.isLinkVisited(link) == false ) {
	// links.addElement( link );
	// }
	// }
	// } catch( StringIndexOutOfBoundsException e ) {}
	// }
	// return links;
	// }

	private void parseForWordsInTitle(String page) {
		// przeszukaj dla taga <title>
		int indexBeg = 0;
		int indexEnd = 0;
		indexBeg = page.indexOf("<title>", 0); // wyznacza pocz¹tek <title>
		indexEnd = page.indexOf("</title>", indexBeg); // wyznacza koniec
														// <title>
		// System.out.println( indexBeg + " " + indexEnd );
		if (indexBeg != -1 && indexEnd != -1) {
			String title = page.substring(page.indexOf(">", indexBeg) + 1,
					indexEnd); // wyciaga calego title
			Vector<String> words = parseText(title);
			for (int i = 0; i < words.size(); ++i) {
				memory.putWord(words.elementAt(i), urlName, TITLE_PRIORITY);
			}
		}
		// koniec przeszukiwania dla taga <title>
	}

	private void parseForWordsInHeaders(String page) {
		int beghx = 0, tmpBeg = 0, tmpEnd = 0;
		Vector<String> words;
		for (int i = 1; i <= 6; ++i) { // w ten sposob przeiteruje od H1 do H6
										// :]
			do {
				beghx = page.indexOf("<h" + i, tmpBeg); // wyszukuje ma³e hx
				if (beghx != -1) {
					tmpBeg = page.indexOf(">", beghx) + 1; // przeskakuje
															// wskaznikiem za
															// taga <hX>
					tmpEnd = page.indexOf("</h" + i, beghx);
					words = parseText(page.substring(tmpBeg, tmpEnd));
					if (words != null) {
						for (int k = 0; k < words.size(); ++k) {
							memory.putWord(words.elementAt(k), urlName,
									HEADER_PRIORITY[i - 1]);
						}
					}
				}
			} while (beghx != -1);
		}
	}

	private void parseForWordsInText(String page) {
		String word = "";
		int begtag = 0;
		char c;
		for (int i = 0; i < page.length(); ++i) {
			c = page.charAt(i);
			switch (c) {
			case '\t': // patrz nizej
			case '\n': // patrz nizej
			case ' ': // oddziela mi wyrazy
				if (word.length() > MIN_WORD_LENGTH) {
					if (stack.isEmpty() == false) {
						memory.putWord(word, urlName, stack.peek().intValue());
					} else {
						memory.putWord(word, urlName, 0);
					}
				}
				word = "";
				break;
			case '<': // prawdopodobnie poczatek taga
				begtag = i;
				do {
					++i;
				} while (i < page.length() && page.charAt(i) != '>');
				if (page.substring(begtag, i).contains("font")
						&& !page.substring(begtag, i).contains("/font")
						&& !page.substring(begtag, i).contains("<!--")) { // otwieram
																			// taga
					int whereSize = page.indexOf("size=", begtag);
					if (whereSize < i) {
						// tutaj jeszcze poprawic, zeby znajdowal rozmiar
						// automatycznie, a nie na sztywno
						try {
							Integer newSize = new Integer(Integer.parseInt(page
									.substring(whereSize + 7, whereSize + 8)));
							stack.push(newSize);
						} catch (NumberFormatException e) {
							stack.push(DEFAULT_SIZE);
						}
					} else {
						stack.push(DEFAULT_SIZE);
					}
				}
				if (page.substring(begtag, i + 1).contains("/font>")) { // zamykam
																		// taga
					try {
						// System.out.println("POOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOP
						// " + urlName );
						stack.pop();
					} catch (EmptyStackException e) {
					}
				}
				break;
			default: // zwykly znak, wiec go sobie radosnie obsluze
				word += c;
				break;
			}
		}
	}

	private Vector<String> parseText(String text) {
		int tmpIndexBeg = 0; // tempowe indeksy dla wyrazow zawartych w title
		int tmpIndexEnd = 0; // drugi tempowy indeks dla wyrazow zawartych w
								// title
		int tmpForSpace, tmpForTab, tmpForLF;
		Vector<String> words = new Vector<String>();
		String word;
		do {
			/*
			 * Dzialanie petli: pobiera pierwszy znak spacja, tabulator albo
			 * znak nowej linii, jesli znajdzie to go zapisuje jesli nie to
			 * zapisuje MAX_VALUE dla int'a, potem wyszukuje najblizszy wyraz i
			 * zapisuje go do struktury przechowujacej hasla z najwyzszym
			 * priorytetem.
			 */
			tmpForSpace = (text.indexOf(" ", tmpIndexBeg) != -1) ? text
					.indexOf(" ", tmpIndexBeg) : Integer.MAX_VALUE;
			tmpForTab = (text.indexOf("\t", tmpIndexBeg) != -1) ? text.indexOf(
					"\t", tmpIndexBeg) : Integer.MAX_VALUE;
			tmpForLF = (text.indexOf("\n", tmpIndexBeg) != -1) ? text.indexOf(
					"\n", tmpIndexBeg) : Integer.MAX_VALUE;
			// System.out.println(tmpForSpace + " " + tmpForTab + " " +
			// tmpForLF);
			tmpIndexEnd = Math.min(tmpForSpace, Math.min(tmpForTab, tmpForLF));
			// System.out.println(
			// text.substring(tmpIndexBeg,(tmpIndexEnd!=Integer.MAX_VALUE)?tmpIndexEnd:text.length()));
			word = text.substring(tmpIndexBeg,
					(tmpIndexEnd != Integer.MAX_VALUE) ? tmpIndexEnd : text
							.length());
			words.add(word);
			tmpIndexBeg = tmpIndexEnd + 1;
		} while (tmpIndexEnd != Integer.MAX_VALUE);
		return words;
	}

	private String urlName;

	private WormMemory memory;

	private WormReaderStarter starter;

	private Stack<Integer> stack;

	private final Integer DEFAULT_SIZE = new Integer(0);

	private final int HEADER_PRIORITY[] = { 105, 104, 103, 102, 101, 100 };

	private final int TITLE_PRIORITY = 106;

	private final int MIN_WORD_LENGTH = 4;
}
