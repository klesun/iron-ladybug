using UnityEngine;
using System.Collections;

public class HeroControl : MonoBehaviour 
{
	const float SPRINT_INTERVAL = 5;

	const float FRICTION_FORCE = 12;
	const float MAX_RUNNING_SPEED = 7;
	const float RUNNING_BOOST = 18;
	const float JUMP_BOOST = 8;

	private float mouseSensitivity = 4.0F;
	public GameObject cameraAngle;
	public AudioClip jumpingSound;
	public AudioClip jumpingEvilSound;
	public AudioClip jumpingSfx;
	public AudioClip sprintingEvilSound;
	public AudioClip sprintingSfx;
	public AudioClip outOfManaEvilSound;
	public Transform gunShotSource;
	public AudioClip epeeSwingSound;

	public Animator anima;

	private float? lastSprintTime = null;
	private float distToGround;
	private Rigidbody body;

	void Start () 
	{
		Cursor.lockState = CursorLockMode.Locked;
		distToGround = GetComponent<Collider> ().bounds.extents.y;
		body = GetComponent<Rigidbody> ();
		body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate (new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0));
		HandleKeys ();
	}

	void HandleKeys()
	{
		var keyedDirection = GetKeyedDirection ();

		if (IsGrounded()) {
			anima.SetBool ("isFlying", false);
			ApplyFriction ();
			if (keyedDirection.magnitude > 0) {
				anima.SetBool ("isInBattle", false);
			}
			Speeden (keyedDirection, RUNNING_BOOST, MAX_RUNNING_SPEED);
			if (Input.GetKeyDown(KeyCode.Space)) {
				//anima.Play ("Armature|jump", -1, 0f);
				anima.SetBool ("isFlying", true);
				body.velocity += Vector3.up * JUMP_BOOST;

				var snd = Random.Range(0, 10) == 0
					? jumpingEvilSound
					: jumpingSound;

				AudioSource.PlayClipAtPoint(snd, transform.position);
				AudioSource.PlayClipAtPoint(jumpingSfx, transform.position);
			}
			if (Input.GetKeyDown (KeyCode.Mouse1)) {
				AudioSource.PlayClipAtPoint(outOfManaEvilSound, transform.position);
			}
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				if (anima.GetBool ("isInBattle") || body.velocity.magnitude > 0.1) {
					if (lastSprintTime == null ||
					    Time.fixedTime - lastSprintTime > SPRINT_INTERVAL
					) {
						body.velocity += Vector3.up * 2 + transform.forward * 7;
						lastSprintTime = Time.fixedTime;
						anima.SetTrigger ("attacking");
						AudioSource.PlayClipAtPoint (epeeSwingSound, transform.position);
					} else {
						AudioSource.PlayClipAtPoint(outOfManaEvilSound, transform.position);
					}
				}
				anima.SetBool ("isInBattle", true);
			}
		} else {
			anima.SetBool ("isFlying", true);
			anima.SetBool ("isInBattle", false);
			Speeden (keyedDirection, RUNNING_BOOST / 2, MAX_RUNNING_SPEED / 4);
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				if (lastSprintTime == null || 
					Time.fixedTime - lastSprintTime > SPRINT_INTERVAL
				) {
					body.velocity += cameraAngle.transform.forward * 10;
					lastSprintTime = Time.fixedTime;
					AudioSource.PlayClipAtPoint(sprintingEvilSound, transform.position);
					AudioSource.PlayClipAtPoint(sprintingSfx, transform.position);
				} else {
					AudioSource.PlayClipAtPoint(outOfManaEvilSound, transform.position);
				}
			}
		}

		anima.SetFloat ("xSpeed", body.velocity.x);
		anima.SetFloat ("ySpeed", body.velocity.z);
	}

	Vector3 GetKeyedDirection()
	{
		var result = new Vector3 ();

		if (Input.GetKey(KeyCode.W)) {
			result += transform.forward;
		}
		if (Input.GetKey(KeyCode.S)) {
			result -= transform.forward;
		}
		if (Input.GetKey(KeyCode.A)) {
			result -= transform.right;
		}
		if (Input.GetKey(KeyCode.D)) {
			result += transform.right;
		}

		return result;
	}

	void Speeden(Vector3 keyedDirection, float boost, float maxSpeed)
	{
		var wasSpeed = body.velocity;

		body.velocity += keyedDirection * Time.deltaTime * boost;

		// nullyfying this frame boost if limit surpassed
		if (body.velocity.magnitude > maxSpeed &&
			body.velocity.magnitude > wasSpeed.magnitude
		) {
			if (wasSpeed.magnitude > maxSpeed) {
				body.velocity = body.velocity.normalized * wasSpeed.magnitude;
			} else {
				body.velocity = body.velocity.normalized * maxSpeed;
			}
		}
	}

	void ApplyFriction()
	{
		// applying friction force
		var frictionForce = -body.velocity.normalized * Time.deltaTime * FRICTION_FORCE;
		if (frictionForce.magnitude > body.velocity.magnitude) {
			body.velocity = Vector3.zero;
		} else {
			body.velocity += frictionForce;
		}
	}

	bool IsGrounded()
	{
		return Physics.Raycast (transform.position, -Vector3.up, distToGround + 0.1f);
	}
}
