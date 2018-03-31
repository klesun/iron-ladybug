using Interfaces;
using UnityEngine;

namespace Assets.Scripts.Interfaces
{
    /**
     * a skill is a special action that an npc can perform once in a while i intend to
     * rip-off that concept from the Dota, where skills are not bound to a particular unit
     */
    public abstract class ISkillMb: MonoBehaviour
    {
        public abstract void Perform(INpcMb caster);
    }
}
