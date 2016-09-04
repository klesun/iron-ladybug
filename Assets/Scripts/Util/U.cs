using System;
using AssemblyCSharp;

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
		public static void If(bool condition, DCallback body) 
		{
			if (condition) {
				body ();
			}
		}
	}
}

