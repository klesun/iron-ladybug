using UnityEngine;
using System.Collections;
using AssemblyCSharp;

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
		public float triggerDelay = 10;

		private Vector3 initialPosition;
		private Quaternion initialRotation;
		private float initialMass;
		private bool isRecovering = false;

		void Start ()
		{
			initialPosition = transform.position;
			initialRotation = transform.rotation;
			initialMass = body.mass;
		}

		void Update () 
		{
			var isMoved = Vector3.Distance (initialPosition, transform.position) > 0.1;
			var isRotated = Quaternion.Angle (initialRotation, transform.rotation) > 0.1;

			if ((isMoved || isRotated) && !isRecovering) {
				isRecovering = true;

				Tls.inst().SetTimeout(triggerDelay, () => {
					body.mass = 0;
					body.velocity = Vector3.zero;
					body.angularVelocity = Vector3.zero;
					foreach (var collider in gameObject.GetComponents<Collider>()) {
						collider.enabled = false;
					}

					var finalPosition = transform.position;
					var finalRotation = transform.rotation;
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
							transform.position = Vector3.Lerp (finalPosition, initialPosition, i * 1.0f / steps);
							transform.rotation = Quaternion.Lerp (finalRotation, initialRotation, i * 1.0f / steps);
							Tls.inst().SetTimeout(returnDuration / steps, doStep);
						} else {
							body.mass = initialMass;
							isRecovering = false;
							foreach (var collider in gameObject.GetComponents<Collider>()) {
								collider.enabled = true;
							}
						}
					};

					doStep();
				});
			}
		}
	}
}