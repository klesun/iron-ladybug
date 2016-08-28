using System;
using UnityEngine;

namespace AssemblyCSharp
{
	public enum ETrophy{STRAWBERRY, ORB_OF_MOTIVATION, COCKSHOT, ENEMY}

	/**
	 * represents some goods scattered 
	 * around the world player can pick up
	 */
	public abstract class ITrophy: MonoBehaviour
	{
		public abstract void SetOnCollected(DCallback callback);
		public abstract ETrophy GetName();
	}
}

