using UnityEngine;
using System.Collections;

public class HeroControl : MonoBehaviour 
{
	const float SPRINT_INTERVAL = 5;

	private float mouseSensitivity = 4.0F;
	public GameObject cameraAngle;
	public AudioClip jumpingSound;
	public AudioClip jumpingEvilSound;
	public AudioClip jumpingSfx;
	public AudioClip sprintingEvilSound;
	public AudioClip sprintingSfx;
	public AudioClip outOfManaEvilSound;
	public Transform gunShotSource;

	public Animator anima;

	private float? lastSprintTime = null;
	private float distToGround;
	private Rigidbody body;

	void Start () 
	{
		Cursor.lockState = CursorLockMode.Locked;
		distToGround = GetComponent<Collider> ().bounds.extents.y;
		body = GetComponent<Rigidbody> ();
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
			anima.SetBool ("isFlying", false);
			if (Input.GetKeyDown(KeyCode.Space)) {
				//anima.Play ("Armature|jump", -1, 0f);
				anima.SetBool ("isFlying", true);
				body.velocity += Vector3.up * 10;

				var snd = Random.Range(0, 10) == 0
					? jumpingEvilSound
					: jumpingSound;

				AudioSource.PlayClipAtPoint(snd, transform.position);
				AudioSource.PlayClipAtPoint(jumpingSfx, transform.position);
			}
			if (Input.GetKeyDown (KeyCode.Mouse1)) {
				AudioSource.PlayClipAtPoint(outOfManaEvilSound, transform.position);
			}
		} else {
			anima.SetBool ("isFlying", true);
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				if (lastSprintTime == null || 
					Time.fixedTime - lastSprintTime > SPRINT_INTERVAL
				) {
					GetComponent<Rigidbody>().velocity += cameraAngle.transform.forward * 10;
					lastSprintTime = Time.fixedTime;
					AudioSource.PlayClipAtPoint(sprintingEvilSound, transform.position);
					AudioSource.PlayClipAtPoint(sprintingSfx, transform.position);
				} else {
					AudioSource.PlayClipAtPoint(outOfManaEvilSound, transform.position);
				}
			}
		}

		var factor = 1f;
		if (Input.GetKey(KeyCode.W)) {
			body.velocity += transform.forward * factor;
			//transform.Translate (Vector3.forward * 0.05f);
		}
		if (Input.GetKey(KeyCode.S)) {
			body.velocity -= transform.forward;
			//transform.Translate (Vector3.forward * -0.05f);
		}
		if (Input.GetKey(KeyCode.A)) {
			body.velocity -= transform.right * factor;
			//transform.Translate (Vector3.right * -0.05f);
		}
		if (Input.GetKey(KeyCode.D)) {
			body.velocity += transform.right * factor;
			//transform.Translate (Vector3.right * 0.05f);
		}

		anima.SetFloat ("xSpeed", body.velocity.x);
		anima.SetFloat ("ySpeed", body.velocity.z);
	}

	bool IsGrounded()
	{
		return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.1f);
	}
}
