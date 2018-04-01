using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using Util.Logic;

namespace Util
{
    /**
     * returns moved rigidbody elements to the start
     * position after a while if they are moved
     */
    public class Wolverine : MonoBehaviour
    {
        public Rigidbody body;
        public float returnDuration = 5;
        public Bishop bundler;

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        private float initialMass;
        private bool isRecoverScheduled = false;

        void Awake ()
        {
            initialPosition = transform.localPosition;
            initialRotation = transform.localRotation;
            initialMass = body.mass;
        }

        void Update ()
        {
            var isMoved = Vector3.Distance (initialPosition, transform.localPosition) > 0.1;
            var isRotated = Quaternion.Angle (initialRotation, transform.localRotation) > 0.1;

            if ((isMoved || isRotated) && !isRecoverScheduled) {
                bundler.Bundle (RestoreState);
                isRecoverScheduled = true;
            }
        }

        void RestoreState()
        {
            body.mass = 0;
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            foreach (var collider in gameObject.GetComponents<Collider>()) {
                collider.enabled = false;
            }

            var finalPosition = transform.localPosition;
            var finalRotation = transform.localRotation;
            var i = 0;
            var steps = returnDuration * 60;
            D.Cb doStep = null;
            doStep = () => {
                if (this == null) {
                    // since we are using SetTimeout(), there is a
                    // possibility this instance is destroyed by the time
                    return;
                }
                if (i++ < steps) {
                    transform.localPosition = Vector3.Lerp (finalPosition, initialPosition, i * 1.0f / steps);
                    transform.localRotation = Quaternion.Lerp (finalRotation, initialRotation, i * 1.0f / steps);
                    Tls.Inst().SetGameTimeout(returnDuration / steps, doStep);
                } else {
                    body.mass = initialMass;
                    isRecoverScheduled = false;
                    foreach (var collider in gameObject.GetComponents<Collider>()) {
                        collider.enabled = true;
                    }
                }
            };

            doStep();
        }
    }
}
