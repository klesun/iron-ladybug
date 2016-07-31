using UnityEngine;
using System.Collections;

public class HeroControl : MonoBehaviour 
{
	const float SPRINT_INTERVAL = 5;

	public float mouseSensitivity = 4.0F;
	public AudioClip jumpingSound;
	public AudioClip jumpingEvilSound;
	public AudioClip sprintingEvilSound;
	public AudioClip outOfManaEvilSound;

	private float? lastSprintTime = null;
	private float distToGround;

	Rigidbody m_Rigidbody;

	void Start () 
	{
		m_Rigidbody = GetComponent<Rigidbody>();
		Cursor.lockState = CursorLockMode.Locked;
		distToGround = GetComponent<Collider> ().bounds.extents.y;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate (new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0));
		HandleKeys ();
	}

	private void HandleKeys()
	{
		if (IsGrounded()) {
			if (Input.GetKeyDown(KeyCode.Space)) {
				m_Rigidbody.velocity += Vector3.up * 10;

				var snd = Random.Range(0, 10) == 0
					? jumpingEvilSound
					: jumpingSound;

				AudioSource.PlayClipAtPoint(snd, transform.position);
			}
		} else {
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				if (lastSprintTime == null || 
					lastSprintTime - Time.fixedTime < -SPRINT_INTERVAL
				) {
					AudioSource.PlayClipAtPoint(sprintingEvilSound, transform.position);
					m_Rigidbody.velocity += transform.forward * 20;
					lastSprintTime = Time.fixedTime;
				} else {
					AudioSource.PlayClipAtPoint(outOfManaEvilSound, transform.position);
				}
			}
		}
	}

	bool IsGrounded()
	{
		return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.1f);
	}

}
