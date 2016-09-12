using UnityEngine;
using System.Collections;

namespace GameLogic
{
	/**
	 * if my guess is correct, i can keep the static mapping from code to 
	 * audio files here preserving the ability to easily refactor them 
	 * (renaming files/moving them/instantly knowing if some references are not resolved/etc)
	 * 
	 * all that i have to do is to make a single prefab having this script and putting it to Resources
	 */
	public class AudioMap : MonoBehaviour 
	{
		public AudioClip npcDeathScream;
	}
}
