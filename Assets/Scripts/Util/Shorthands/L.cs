using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Util.Shorthands
{
	/** 
	 * a wrapper for List
	 * 
	 * apparently, methods like "RemoveAt()" return void in C# - unforgivable!
	 * 
	 * Note: if you need this for primitives, define a separate class cuz i cannot 
	 * 
	 * TODO: list can contain nulls, so returning null when not found is bad idea
	 * ... IMPLKEMENT AN OPTIONAL CLASS?!
	 */
	public class L<T> where T : class
	{
		public readonly List<T> s;

		public L (IEnumerable<T> subj)
		{
			this.s = new List<T> (subj);
		}

		public L ()
		{
			this.s = new List<T> ();
		}

		public static L<Tf> N<Tf>(IEnumerable<Tf> subj) where Tf : class
		{
			return new L<Tf> (subj);
		}

		/** 
		 * built-in list does not have it and answers on SO suggest using Queue
		 * but Queue have some limitations. for example, it does not have "RemoveAt()"
		 */
		public Opt<T> Pop()
		{
			if (s.Count > 0) {
				var tmp = s [s.Count - 1];
				s.RemoveAt (s.Count - 1);
				return Opt<T>.Some(tmp);
			} else {
				return Opt<T>.None<T> ();
			}
		}

		/** 
		 * built-in Linq version throws InvalidOperationException
		 */
		public Opt<T> Last()
		{
			return s.Count > 0 
				? Opt<T>.Some(s [s.Count - 1]) 
				: Opt<T>.None<T>();
		}
	}
}

