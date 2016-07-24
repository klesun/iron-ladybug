using UnityEngine;
using System.Collections;

public class ObjectSettings : MonoBehaviour {


	public int objectCount = 1;
	public float spacing = 1;
	public GameObject original;
	public Mesh gizmoMesh = null;

	void Start () {
		PositionElements ((pos, rot) => Object.Instantiate (original, pos, rot));
	}

	delegate void ElMaker(Vector3 pos, Quaternion rot);
	void PositionElements(ElMaker makeEl)
	{
		for (int i = 0; i < objectCount; i++) {
			var pos = original.transform.position + Vector3.forward * spacing * i;
			var rot = original.transform.rotation;
			makeEl(pos, rot);
		}
	}

	void OnDrawGizmos () {
		PositionElements ((pos, rot) => Gizmos.DrawWireCube(pos, new Vector3 (1,1,1)));
	}

	// Update is called once per frame
	void Update () {
		
	}
}
