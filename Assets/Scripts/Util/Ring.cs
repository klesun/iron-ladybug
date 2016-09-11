using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AssemblyCSharp;

namespace Util
{
	public class Ring : MonoBehaviour 
	{
		public GameObject vaneReference;
		public int vaneCount = 24;

		private List<GameObject> vanes;

		void Start () 
		{
			PlaceVanes ((pos) => {
				var drawTrans = Tls.inst ().DullTransform (pos);
				drawTrans.LookAt (transform.position, transform.forward);
				Object.Instantiate(vaneReference, pos, drawTrans.rotation);
			});
			//Destroy (vaneReference);
		}

		void OnDrawGizmos () 
		{
			if (vaneReference != null) {
				PlaceVanes ((pos) => Gizmos.DrawWireCube (pos, new Vector3 (0.5f,0.5f,0.5f)));
			}
		}

		delegate void DPlaceTaker (Vector3 pos);

		void PlaceVanes (DPlaceTaker makeObj) 
		{
			for (float i = 0; i < vaneCount; ++i) {
				var dx = Mathf.Sin (2 * Mathf.PI * i / vaneCount) * Vector3.Distance(vaneReference.transform.position, transform.position);
				var dy = Mathf.Cos (2 * Mathf.PI * i / vaneCount) * Vector3.Distance(vaneReference.transform.position, transform.position);
				var drawPos = transform.position + transform.right * dx + transform.up * dy;

				makeObj (drawPos);
			}
		}
	}
}
