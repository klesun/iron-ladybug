using System;

namespace Util
{
	/** D stands for "Delegates" */
	public class D
	{
		/** "Function" */
		public delegate Tout F1<Tin, Tout>(Tin value);
		/** "CallBack" */
		public delegate void Cb();
		/** "Consumer" */
		public delegate void Cu<T>(T value);
	}
}

