using UnityEngine;
using System.Collections;

namespace Interfaces
{
	/**
	 * it can be anything with a GameObject representative and a portrait
	 */
	public interface INpc
	{
		Texture GetPortrait ();
		GameObject GetGameObject ();
	}
}