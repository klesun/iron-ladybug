using UnityEngine;
using System.Collections;

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
    }
}