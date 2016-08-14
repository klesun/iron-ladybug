using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class NpcControl : MonoBehaviour, IPiercable
{
	public const int LOUNGE_HEIGHT = 1;
	public const int LOUNGE_LENGTH = 5;

	const float SPRINT_INTERVAL = 5;

	const float FRICTION_FORCE = 12;
	const float MAX_RUNNING_SPEED = 7;
	const float RUNNING_BOOST = 18;
	const float JUMP_BOOST = 8;
	
	public AudioClip hitSound;
	public Animator anima;

	public AudioClip jumpingSfx;
	public AudioClip sprintingSfx;
	public AudioClip epeeSwingSound;
	public EmmitterControl boostEmmitter;

	private int health = 100;
	private Rigidbody body;
	private bool isDead = false;
	private float? lastSprintTime = null;
	private float distToGround;

	// Use this for initialization
	void Start () 
	{
		distToGround = GetComponent<Collider> ().bounds.extents.y;
		body = GetComponent<Rigidbody> ();
		body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isDead) {
			Live();
		}
	}

	void Live()
	{
		if (health <= 0) {
			isDead = true;
			body.constraints = 0; // so it fell
			body.velocity -= transform.forward * 3;
			// Destroy(this.gameObject);
		}
		if (IsGrounded ()) {
			anima.SetBool ("isFlying", false);
			ApplyFriction ();
		} else {
			anima.SetBool ("isFlying", true);
			anima.SetBool ("isInBattle", false);
		}

		anima.SetFloat ("xSpeed", body.velocity.x);
		anima.SetFloat ("ySpeed", body.velocity.z);
	}

	public void Face(Vector3 enemy)
	{
		if (!isDead) {
			transform.LookAt (new Vector3(enemy.x, transform.position.y, enemy.z));
		}
	}

	public bool Attack()
	{
		anima.SetBool ("isInBattle", true);
		if (CanAttack()) {
			body.velocity += 
				+ NpcControl.LOUNGE_HEIGHT * Vector3.up
				+ NpcControl.LOUNGE_LENGTH * transform.forward;

			lastSprintTime = Time.fixedTime;
			anima.SetTrigger ("attacking");
			AudioSource.PlayClipAtPoint (epeeSwingSound, transform.position);
			return true;
		}
		return false;
	}

	public bool Jump()
	{
		if (!isDead && IsGrounded()) {
			anima.SetBool ("isFlying", true);
			body.velocity += Vector3.up * JUMP_BOOST;

			AudioSource.PlayClipAtPoint(jumpingSfx, transform.position);
			return true;
		}
		return false;
	}

	public bool Boost(Vector3 direction)
	{
		if (!isDead && !IsGrounded ()) {
			if (lastSprintTime == null || 
				Time.fixedTime - lastSprintTime > SPRINT_INTERVAL
			) {
				body.velocity += direction * 10;
				lastSprintTime = Time.fixedTime;
				AudioSource.PlayClipAtPoint(sprintingSfx, transform.position);
				boostEmmitter.Emmit ();
			}
		}
		return false;
	}

	public void pierce()
	{
		if (!isDead) {
			health -= 50;
			AudioSource.PlayClipAtPoint(hitSound, transform.position);
		}
	}

	public void Move(Vector3 keyedDirection)
	{
		if (!isDead) {
			if (IsGrounded()) {
				if (keyedDirection.magnitude > 0) {
					anima.SetBool ("isInBattle", false);
				}
				Speeden (keyedDirection, RUNNING_BOOST, MAX_RUNNING_SPEED);
			} else {
				Speeden (keyedDirection, RUNNING_BOOST / 2, MAX_RUNNING_SPEED / 4);
			}
		}
	}

	public void StopMoving()
	{
		if (IsGrounded()) {
			body.velocity = Vector3.zero;
		}
	}

	public bool IsGrounded()
	{
		return Mathf.Abs(body.velocity.y) < 0.1f
			&& Physics.Raycast (transform.position, -Vector3.up * 0.1f, distToGround + 0.1f);
	}

	public bool CanAttack()
	{
		return !isDead && IsGrounded () && (
		    lastSprintTime == null ||
		    Time.fixedTime - lastSprintTime > SPRINT_INTERVAL
		);
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

	public Vector3 GetVelocity()
	{
		return body.velocity;
	}
}
