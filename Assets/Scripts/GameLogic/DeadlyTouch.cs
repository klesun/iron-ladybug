using UnityEngine;
using System.Collections;
using Assets.Scripts.GameLogic;

namespace GameLogic
{
    public class DeadlyTouch : MonoBehaviour
    {
        void OnCollisionEnter(Collision collision)
        {
            foreach (var npc in collision.collider.gameObject.GetComponents<NpcControl>()) {
                npc.SetHeroHealth(0);
            }
        }

        void OnTriggerEnter(Collider collider)
        {
            foreach (var npc in collider.gameObject.GetComponents<NpcControl>()) {
                npc.SetHeroHealth(0);
            }
        }
    }
}