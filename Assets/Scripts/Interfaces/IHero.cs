using UnityEngine;
using System.Collections;

namespace Interfaces
{
    /**
     * IHero is anything that implements this interface
     */
    public interface IHero
    {
        INpcMb GetNpc();
    }
}