using UnityEngine;
using System.Collections;

public class HeroControl : MonoBehaviour 
{
	const float SPRINT_INTERVAL = 5;

	private float mouseSensitivity = 4.0F;
	public GameObject cameraAngle;
	public AudioClip jumpingSound;
	public AudioClip jumpingEvilSound;
	public AudioClip sprintingEvilSound;
	public AudioClip outOfManaEvilSound;

	private float? lastSprintTime = null;
	private float distToGround;

	void Start () 
	{
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
				GetComponent<Rigidbody>().velocity += Vector3.up * 10;

				var snd = Random.Range(0, 10) == 0
					? jumpingEvilSound
					: jumpingSound;

				AudioSource.PlayClipAtPoint(snd, transform.position);
			}
		} else {
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				if (lastSprintTime == null || 
					Time.fixedTime - lastSprintTime > SPRINT_INTERVAL
				) {
					GetComponent<Rigidbody>().velocity += cameraAngle.transform.forward * 10;
					lastSprintTime = Time.fixedTime;
					AudioSource.PlayClipAtPoint(sprintingEvilSound, transform.position);
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
