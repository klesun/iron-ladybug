using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Util.Architecture;
using UnityEngine;

/**
 * takes a GameObject and a _Train_ component and arranges
 * copies of the n game object equally alongside the curve
 */
[ExecuteInEditMode]
public class ArrSlider : MonoBehaviour
{
	const float REVALIDATION_PERIOD = 0.1f;

	public GameObject wagonHolder;
	public GameObject wagonRef;
	public int copyCnt = 1;
	public Train train;

	double? lastValidatedOn = null;
	bool revalidationRequested = false;

	#if UNITY_EDITOR
	void OnValidate()
	{
		UnityEditor.EditorApplication.delayCall += () => {
			UnityEditor.EditorApplication.delayCall += () => revalidationRequested = true;
		};
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

	void Renew() {
		if (this == null) {
			// well, it complains about it being
			// destroyed when i starts the game
			return;
		}
		if (Application.isPlaying) {
			return;
		}

		var deadmen = new List<GameObject>();
		foreach (Transform ch in wagonHolder.transform) {
			deadmen.Add (ch.gameObject);
		}
		deadmen.ForEach (DestroyImmediate);

		if (copyCnt > 0) {
			train.spacing = 1f / copyCnt;
		}
		var copies = new List<Transform>();
		for (var i = 0; i < copyCnt; ++i) {
			var copy = Instantiate (wagonRef.gameObject, wagonHolder.transform);
			copy.name = "_wagon" + i;
			copies.Add(copy.transform);
		}
		train.wagons = copies.ToArray();
		train.UpdatePosition(0);
	}
}
