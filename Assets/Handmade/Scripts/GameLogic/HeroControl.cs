using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class HeroControl : MonoBehaviour 
{
	public GameObject cameraAngle;
	public AudioClip jumpingSound;
	public AudioClip jumpingEvilSound;
	public AudioClip sprintingEvilSound;
	public AudioClip outOfManaEvilSound;

	public NpcControl npc;

	private float mouseSensitivity = 4.0F;
	private List<EnemyLogic> enemies = new List<EnemyLogic>();

	void Start () 
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void AcquireEnemy(EnemyLogic enemy)
	{
		enemies.Add (enemy);
	}
	
	void Update () 
	{
		transform.Rotate (new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0));
		HandleKeys ();
		enemies = enemies.Where (e => !e.npc.IsDead).ToList();
		npc.anima.SetBool ("isInBattle", enemies.Count > 0);
	}

	void HandleKeys()
	{
		npc.Move (GetKeyedDirection ());

		if (npc.IsGrounded()) {
			if (Input.GetKeyDown(KeyCode.Space) && npc.Jump()) {
				AudioSource.PlayClipAtPoint(
					Random.Range(0, 10) == 0
						? jumpingEvilSound
						: jumpingSound, 
					transform.position
				);
			}
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				if (npc.Attack()) {
					// battle cry!
				} else {
					AudioSource.PlayClipAtPoint(outOfManaEvilSound, transform.position);
				}
			}
			if (Input.GetKeyDown (KeyCode.Mouse1)) {
				npc.Parry ();
			}

		} else {
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				if (npc.Boost(cameraAngle.transform.forward)) {
					AudioSource.PlayClipAtPoint(sprintingEvilSound, transform.position);
				} else {
					AudioSource.PlayClipAtPoint(outOfManaEvilSound, transform.position);
				}
			}
		}
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
}
