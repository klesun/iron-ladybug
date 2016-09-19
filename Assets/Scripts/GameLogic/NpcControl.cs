﻿using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using Util;
using Interfaces;

namespace GameLogic
{
	[RequireComponent (typeof(Rigidbody))]
	[RequireComponent (typeof(Collider))]
	[SelectionBase]
	public class NpcControl : INpcMb, IPiercable, INpc
	{
		public const float LOUNGE_HEIGHT = 1.5f;
		public const float LOUNGE_LENGTH = 6;

		const float SPRINT_INTERVAL = 5;

		const float FRICTION_FORCE = 12;
		const float MAX_RUNNING_SPEED = 13;
		const float RUNNING_BOOST = 18;
		public const float JUMP_BOOST = 8;
		
		public AudioClip hitSound;
		public Animator anima;

		public AudioClip jumpingSfx;
		public AudioClip sprintingSfx;
		public AudioClip epeeSwingSound;
		public EmmitterControl boostEmmitter;
		public Blade epee;
		public Texture icon;

		private int health = 100;
		private Rigidbody body;
		private bool isDead = false;
		public bool IsDead {
			get { return isDead; }
		}
		private float? lastSprintTime = null;
		private float distToGround;
		private RaycastHit floor;
		private bool isCloseToGround = false;
		private float lastGroundTime;

		void Awake () 
		{
			distToGround = GetComponent<Collider> ().bounds.extents.y;
			lastGroundTime = Time.fixedTime;
			body = GetComponent<Rigidbody> ();
			body.constraints = 
				RigidbodyConstraints.FreezeRotationX | 
				RigidbodyConstraints.FreezeRotationZ | 
				RigidbodyConstraints.FreezeRotationY;

			epee.onClash = LoseGrip;
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
			isCloseToGround = Physics.Raycast (transform.position, -Vector3.up * 0.1f, out floor, distToGround + 0.1f);
			if (health <= 0) {
				Die ();
			} else {
				if (IsGrounded ()) {
					lastGroundTime = Time.fixedTime;
					anima.SetBool ("isFlying", false);
					ApplyFriction ();
				} else {
					if (Time.fixedTime - lastGroundTime > 0.2) {
						anima.SetBool ("isFlying", true);
						anima.SetBool ("isInBattle", false);
					}
				}

				epee.isParrying = anima.GetCurrentAnimatorStateInfo (0).IsName ("Armature|batman");

				anima.SetFloat ("xSpeed", body.velocity.x);
				anima.SetFloat ("ySpeed", body.velocity.z);
			}
		}

		public void Die()
		{
			Tls.Inst().PlayAudio(Sa.Inst ().audioMap.npcDeathScream);
			isDead = true;
			body.constraints = 0; // so it fell
			body.velocity -= transform.forward * 3;
			epee.Disarm ();
			// Destroy(this.gameObject);
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
				anima.SetTrigger ("attacking");
				body.velocity += 
					+ NpcControl.LOUNGE_HEIGHT * Vector3.up
					+ NpcControl.LOUNGE_LENGTH * transform.forward;

				lastSprintTime = Time.fixedTime;
				AudioSource.PlayClipAtPoint (epeeSwingSound, transform.position);
				return true;
			}
			return false;
		}

		public bool Parry()
		{
			if (!isDead && IsGrounded() &&
				anima.GetCurrentAnimatorStateInfo (0).IsName ("Armature|battleStance")
			) {
				anima.SetTrigger ("parrying");
				body.velocity += transform.forward * 4;
			}
			return false;
		}

		public bool Jump()
		{
			if (!isDead && isCloseToGround) {
				anima.SetBool ("isFlying", true);
				/** @debug */
				body.velocity = new Vector3(body.velocity.x, 0, body.velocity.z);

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
					return true;
				}
			}
			return false;
		}

		public void GetPierced()
		{
			if (!isDead) {
				health -= 20;
				AudioSource.PlayClipAtPoint(hitSound, transform.position);
				LoseGrip ();
			}
		}

		public void LoseGrip()
		{
			anima.SetTrigger ("hit");
			body.velocity += 
				- transform.forward * 3
				+ Vector3.up * 1;

			var penalizedTime = Time.fixedTime - SPRINT_INTERVAL + 1;
			lastSprintTime = Mathf.Max(lastSprintTime ?? penalizedTime, penalizedTime);
		}

		public void Move(Vector3 keyedDirection)
		{
			if (!isDead) {
				if (IsGrounded()) {
					if (keyedDirection.magnitude > 0) {
						anima.SetBool ("isInBattle", false);
					}
					var vector = keyedDirection - floor.normal * Vector3.Dot(floor.normal, keyedDirection);
					Speeden (vector, RUNNING_BOOST, MAX_RUNNING_SPEED);
				} else {
					Speeden (keyedDirection, RUNNING_BOOST / 2, MAX_RUNNING_SPEED / 4);
				}
			}
		}

		public bool IsGrounded()
		{
			return isCloseToGround && (
				body.velocity.magnitude < 0.1 || 
				Vector3.Angle(floor.normal, body.velocity) >= 89.99
			);
		}

		public bool CanAttack()
		{
			return !isDead && IsGrounded () && (
			    lastSprintTime == null ||
			    Time.fixedTime - lastSprintTime > SPRINT_INTERVAL
			) && !anima.GetCurrentAnimatorStateInfo (0).IsName ("Armature|open");
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

		override public Texture GetPortrait()
		{
			return icon;
		}

		override public GameObject GetGameObject()
		{
			return gameObject;
		}
	}
}