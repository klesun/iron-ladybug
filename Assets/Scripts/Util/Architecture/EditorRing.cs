
using UnityEngine;
using Util;
using System.Collections.Generic;

namespace Assets.Scripts.Util.Architecture
{
    [ExecuteInEditMode]
    [SelectionBase]
    class EditorRing : MonoBehaviour
	{
		const float REVALIDATION_PERIOD = 0.1f;

		public TransformListener blockRef;
		public GameObject blockCont;
		public int linkCount = 24;
		public float completeness = 1;
		public float radius = 1;

		double? lastValidatedOn = null;
		bool revalidationRequested = false;

		#if UNITY_EDITOR
		void OnValidate() 
		{
			linkCount = Mathf.Min(Mathf.Max (linkCount, 1), 360);

			if (blockRef != null && blockCont != null) {
				UnityEditor.EditorApplication.delayCall += () => revalidationRequested = true;
				blockRef.onChange = () => revalidationRequested = true;
			}
		}
		#endif

		void Update()
		{
			var now = System.DateTime.Now.Ticks / 10000000d; // microsoft is microsoft
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

			RemoveLinks ();
			AddLinks ();
		}

		delegate void DPlaceTaker (Vector3 pos);

		void RemoveLinks()
		{
			var deadmen = new List<GameObject>();
			foreach (Transform ch in blockCont.transform) {
				deadmen.Add (ch.gameObject);
			}
			deadmen.ForEach (DestroyImmediate);
		}

		void AddLinks()
		{
			for (float i = 0; i <= completeness * linkCount; ++i) {
				var dx = Mathf.Sin (2 * Mathf.PI * i / linkCount) * radius;
				var dy = Mathf.Cos (2 * Mathf.PI * i / linkCount) * radius;
				// var drawPos = blockCont.transform.right * dx + blockCont.transform.up * dy;
				var drawPos = Vector3.right * dx + Vector3.up * dy;

				var block = UnityEngine.Object.Instantiate (blockRef.gameObject);
				block.name = "_link_" + i;
				block.transform.SetParent (blockCont.transform);
				block.transform.localPosition = drawPos;
				block.transform.LookAt (blockCont.transform.position, blockCont.transform.forward);
			}
		}
    }
}