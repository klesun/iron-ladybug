using UnityEngine;
using System.Collections;

namespace Interfaces
{
    /**
     * Nb stands for "Mono Behaviour"
     * this is a wrapper for INpc to allow setting it from editor
     */
    abstract public class IHeroMb : MonoBehaviour, IHero
    {
        abstract public INpcMb GetNpc();
    }
}