using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

[ExecuteInEditMode]
public class BlockGrid : MonoBehaviour 
{
	public TransformListener endPoint;
	public TransformListener blockRef;
	public GameObject blockCont;
	public float spacingZ = 1.0f;
	public float spacingX = 1.0f;
	public int sideRows = 3;

	void Start () 
	{
	}

	void OnValidate() 
	{
		spacingZ = Mathf.Max (spacingZ, 0.1f);
		spacingX = Mathf.Max (spacingX, 0.1f);

		if (endPoint != null && blockRef != null && blockCont != null) {
			UnityEditor.EditorApplication.delayCall += Renew;
			endPoint.onChange = Renew;
			blockRef.onChange = Renew;
		}
	}

	void Renew()
	{
		if (this == null) {
			// well, it complains about it being 
			// destroyed when i start the game
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
