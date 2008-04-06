package util;

import java.util.Comparator;

public class LinkAndPriComparator implements Comparator {

	public int compare(Object o1, Object o2) {
		// TODO Auto-generated method stub
		LinkAndPriority p1 = (LinkAndPriority)o1;
		LinkAndPriority p2 = (LinkAndPriority)o2;
		return p1.priority-p2.priority;
	}

}
