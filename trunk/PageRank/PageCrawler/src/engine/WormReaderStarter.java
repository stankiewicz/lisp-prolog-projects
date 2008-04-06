package engine;

import gui.MainWindow;

import java.util.concurrent.*;

public class WormReaderStarter implements Runnable {

	public WormReaderStarter( WormMemory mem ) {
		memory = mem;
		threads = new Semaphore(HOW_MANY_THREADS);
		work = true;
	}
	
	
	
	public void run() {
		System.out.println("WormReaderStarter.run()");
		//MainWindow mw = new MainWindow(memory);
		//mw.setVisible(true);
		int i = 100;
		while( work && (i-- >0)) {
			String link = memory.getNewLink();
			if( link == null ) {
				try {
					Thread.sleep(SLEEP_TIME);
				} catch( InterruptedException e ) { System.out.println(e); }
			}
			else {
				Runnable r = new WormReader(link,memory,this);
				Thread t = new Thread(r);
				t.setPriority( Thread.MIN_PRIORITY );
				t.start();
				P();
			}
		}// TODO ograniczyc
	
		threads.acquireUninterruptibly(HOW_MANY_THREADS);
		
		for(String s:memory.getLinkToLinks().keySet()){
			System.out.println("link: "+ s + " - " + memory.getLinkToLinks().get(s));
		}
		System.out.println("invalid: " + memory.getInvalid());
		
		
		
		// Tworzenie pliku
	}

	public void P() {
		try {
			threads.acquire();
		} catch( InterruptedException e ) { System.out.println(e); }
	}
	
	public void V() {
		threads.release();
	}
	
	private WormMemory memory;
	private Semaphore threads;
	private boolean work;
	private final int HOW_MANY_THREADS = 30;
	private final int SLEEP_TIME = 100;
}
