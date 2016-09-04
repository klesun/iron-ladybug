using System;
using System.Collections.Generic;

namespace Util
{
	/** 
	 * S stands for the "Shortcut"
	 * i may lack knowledge of how to code in C#, but writing 
	 *     var list = new List<Abracadabra<Piu.Piu.Ololo, DumDum<Yachatta>>>(someInferencableArray);
	 * instead of just say
	 *     var list = new List<>(someInferencableArray);
	 * pissed me off enough to make me write this class that 
	 * contains shortcuts to Dict, List and maybe more constructors
	 */
	public static class S
	{
		public static List<T> List<T>(IEnumerable<T> someArray) {
			return new List<T> (someArray);
		}

		public static Queue<T> Queue<T>(IEnumerable<T> someArray) {
			return new Queue<T> (someArray);
		}
	}
}

