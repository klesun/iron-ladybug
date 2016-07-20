using UnityEngine;
using System.Collections;

public class HeroControl : MonoBehaviour {

	public float mouseSensitivity = 4.0F;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0));
	}
}
