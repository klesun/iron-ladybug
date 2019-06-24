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
	public GameObject wagonHolder;
	public GameObject wagonRef;
	public int copyCnt = 1;
	public Train train;

#if UNITY_EDITOR
	void OnValidate()
	{
		UnityEditor.EditorApplication.delayCall += () => {
			if (!Application.isPlaying) {
				Renew();
			}
		};
	}
#endif
	
	void Renew() {
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
