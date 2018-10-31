using GameLogic;
using GameLogic.Destructibles;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.GameLogic
{
    /**
     * Sa stands for "Static Assets"
     * this class provides refactorable references to the assets in /Resources/
     *
     * don't forget to press "Apply To Prefab" after changing it!
     */
    public class Sa: MonoBehaviour
    {
        public AudioMap audioMap;
        public Gui gui;

        public FireBall fireBallRef;

        private static Sa inst;

        public static Sa Inst()
        {
            // should be _the only_ instantiation from /Resources/ call in the project
            // for it is the root resource provider, rest should be accessed through it
            return inst ?? (inst = ((GameObject)GameObject.Instantiate (Resources.Load("staticAssets"))).GetComponent<Sa> ());
        }
    }
}

