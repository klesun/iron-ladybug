using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Assets.Scripts.Util.Logic {
    public class SpaceTrigger : MonoBehaviour {

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
