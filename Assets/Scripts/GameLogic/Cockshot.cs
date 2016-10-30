using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using Util;
using Interfaces;

namespace GameLogic
{
    public class Cockshot : ITrophy, IPiercable
    {
        public AudioClip explodingBaloonSound;
        private D.Cb onCollected = null;

        public void GetPierced()
        {
            Tls.Inst ().PlayAudio (explodingBaloonSound);
            if (onCollected != null) {
                onCollected ();
            }
            Destroy(gameObject);
        }

        public override ETrophy GetName ()
        {
            return ETrophy.COCKSHOT;
        }

        public override void SetOnCollected (D.Cb callback)
        {
            onCollected = callback;
        }
    }
}