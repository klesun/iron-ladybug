using UnityEngine;
using System.Collections;

public class HeroControl : MonoBehaviour {

	public float mouseSensitivity = 4.0F;

	Rigidbody m_Rigidbody;

	void Start () {
		m_Rigidbody = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0));

		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			m_Rigidbody.velocity += transform.forward * 20;
		}
	}
}
