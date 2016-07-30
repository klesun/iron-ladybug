using UnityEngine;
using System.Collections;

public class HeroControl : MonoBehaviour 
{
	public float mouseSensitivity = 4.0F;
	public AudioSource jumpingSound;

	Rigidbody m_Rigidbody;

	void Start () 
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate (new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0));
		HandleKeys ();
	}

	private void HandleKeys()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0)) {
			m_Rigidbody.velocity += transform.forward * 20;
		}
		if (Input.GetKeyDown(KeyCode.Space)) {
			m_Rigidbody.velocity += Vector3.up * 10;
			jumpingSound.Play ();
		}
	}
}
