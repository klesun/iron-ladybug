using UnityEngine;
using System.Collections;

/** 
 * ties hero to this triggerbox, which in his 
 * turn is tied to some sort of visual platform
 */
public class CharacterHolder : MonoBehaviour {
	void OnTriggerEnter(Collider hero) {
		hero.transform.parent = transform.parent;
	}
	void OnTriggerExit(Collider hero) {
		hero.transform.parent = null;
	}
}
