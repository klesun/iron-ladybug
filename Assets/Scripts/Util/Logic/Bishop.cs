using System;
using System.Collections.Generic;
using UnityEngine;

namespace Util.Logic
{
    /**
     * @see https://en.wikipedia.org/wiki/Bishop_(comics)
     *
     * takes punches from other scripts and releases
     * them all back in single bundle after a while
     */
    public class Bishop: MonoBehaviour
    {
        public float hitBackDelay = 10;

        List<D.Cb> hits = new List<D.Cb>();
        bool hitBackRequested = false;
        float delayStart;

        public void Awake()
        {
            delayStart = Time.fixedTime;
        }

        public void Bundle(D.Cb hitBack)
        {
            if (hits.Count == 0) {
                hitBackRequested = true;
                delayStart = Time.fixedTime;
            }
            hits.Add (hitBack);
        }

        void Update()
        {
            var now = Time.fixedTime;
            if (hitBackRequested && now - delayStart > hitBackDelay) {
                hitBackRequested = false;
                hits.ForEach (h => h());
                hits.Clear ();
            }
        }
    }
}

