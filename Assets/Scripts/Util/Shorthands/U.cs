using System;
using AssemblyCSharp;
using UnityEngine;

namespace Util
{
	/** 
	 * U stands for "Uporotostj"
	 * the thing is: i hate brackets. i HATE them
	 * this class provides some calls that would 
	 * be a less traditional way of writing code
	 */
	public static class U
	{
		public static IfResult If(bool condition, D.Cb body) 
		{
			if (condition) {
				body ();
				return new IfResult () { applied = true };
			} else {
				return new IfResult () { applied = false };
			}
		}

		public class IfResult
		{
			public bool applied = true;

			public void Else(D.Cb elseBody)
			{
				if (!applied) {
					elseBody ();
				}
			}
		}
	}
}

