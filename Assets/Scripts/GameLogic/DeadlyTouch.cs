using UnityEngine;
using System.Collections;

public class DeadlyTouch : MonoBehaviour 
{
	void OnCollisionEnter(Collision collision)
	{
		foreach (var npc in collision.collider.gameObject.GetComponents<NpcControl>()) {
			npc.Die ();
		}
	}
}
