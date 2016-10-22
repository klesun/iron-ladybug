using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Util
{
	[ExecuteInEditMode]
	[SelectionBase]
	public class BlockGrid : MonoBehaviour 
	{
		const float REVALIDATION_PERIOD = 0.1f;

		public TransformListener endPoint;
		public TransformListener blockRef;
		public GameObject blockCont;
		public float spacingZ = 1.0f;
		public float spacingX = 1.0f;
		public int sideRows = 3;

		double? lastValidatedOn = null;
		bool revalidationRequested = false;

		void Awake () 
		{
		}

		#if UNITY_EDITOR
		void OnValidate() 
		{
			spacingZ = Mathf.Max (spacingZ, 0.1f);
			spacingX = Mathf.Max (spacingX, 0.1f);

			if (endPoint != null && blockRef != null && blockCont != null) {
				UnityEditor.EditorApplication.delayCall += () => revalidationRequested = true;
				endPoint.onChange = () => revalidationRequested = true;
				blockRef.onChange = () => revalidationRequested = true;
			}
		}
		#endif

		void Update()
		{
			var now = System.DateTime.Now.Ticks / 10000000d;
			if (revalidationRequested && (lastValidatedOn == null || now - lastValidatedOn > REVALIDATION_PERIOD)) {
				revalidationRequested = false;
				lastValidatedOn = now;
				Renew ();
			}
		}

		void Renew()
		{
			if (this == null) {
				// well, it complains about it being 
				// destroyed when i starts the game
				return;
			}
			if (Application.isPlaying) {
				return;
			}

			var deadmen = new List<GameObject>();
			foreach (Transform ch in blockCont.transform) {
				deadmen.Add (ch.gameObject);
			}
			deadmen.ForEach (DestroyImmediate);

			blockCont.transform.LookAt (endPoint.transform);

			var dist = Vector3.Distance (blockRef.gameObject.transform.position, endPoint.gameObject.transform.position);
			for (var i = 0; i < dist / spacingZ; ++i) {
				for (var j = -sideRows; j <= sideRows; ++j) {
					var block = UnityEngine.Object.Instantiate (blockRef.gameObject);
					block.name = "_block" + i + "x" + j;
					block.transform.SetParent (blockCont.transform);
					block.transform.localPosition = new Vector3 (j * spacingX, 0, i * spacingZ);
				}
			}
		}
	}
}