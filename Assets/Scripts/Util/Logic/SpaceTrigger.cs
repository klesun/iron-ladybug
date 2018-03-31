using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Assets.Scripts.Util.Logic {
    public class SpaceTrigger : MonoBehaviour {

        /**
         * i dunno why, but if you move right-left on same place few times, OnTriggerExit
         * gets triggered even though you are not even close to leaving the region
         * so we will not trigger onOut body _comes back_ in this threshold window
         */
        public float outThreshold = 0.2f;

        private List<D.Cu<Collider>> callbacks = new List<D.Cu<Collider>>();
        private List<D.Cu<Collider>> exitCallbacks = new List<D.Cu<Collider>>();

        void OnTriggerEnter(Collider collider)
        {
            callbacks.ForEach (cb => cb(collider));
        }

        void OnTriggerExit(Collider collider)
        {
            exitCallbacks.ForEach (cb => cb(collider));
        }

        public void OnIn(D.Cu<Collider> callback)
        {
            this.callbacks.Add(callback);
        }

        public void OnOut(D.Cu<Collider> callback)
        {
            this.exitCallbacks.Add(callback);
        }

        public D.Cu<Collider> onIn {
            set { callbacks.Add(value); }
        }

        public D.Cu<Collider> onOut {
            set { exitCallbacks.Add(value); }
        }
    }
}
