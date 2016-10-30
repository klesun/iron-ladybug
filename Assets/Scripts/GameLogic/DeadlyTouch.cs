using UnityEngine;
using System.Collections;

namespace GameLogic
{
    public class DeadlyTouch : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            foreach (var npc in collision.collider.gameObject.GetComponents<NpcControl>()) {
                npc.Die ();
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            foreach (var npc in collider.gameObject.GetComponents<NpcControl>()) {
                npc.Die ();
            }
        }
    }
}