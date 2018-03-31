using UnityEngine;
using System.Collections;
using Util.Shorthands;

namespace Interfaces
{
    /**
     * Nb stands for "Mono Behaviour"
     * this is a wrapper for INpc to allow setting it from editor
     */
    public abstract class INpcMb : MonoBehaviour, INpc
    {
        abstract public Texture GetPortrait();
        abstract public GameObject GetGameObject();
        abstract public void Die();
        abstract public Opt<RaycastHit> GetFloor();
    }
}