using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using AssemblyCSharp;

public class HeroControl : MonoBehaviour 
{
	public GameObject cameraAngle;
	public AudioClip jumpingSound;
	public AudioClip jumpingEvilSound;
	public AudioClip sprintingEvilSound;
	public AudioClip outOfManaEvilSound;

	public NpcControl npc;
	public HeroStats stats;
	public GuiControl gui;

	private float mouseSensitivity = 4.0F;
	private HashSet<EnemyLogic> enemies = new HashSet<EnemyLogic>();

	void Start () 
	{
		Cursor.lockState = CursorLockMode.Locked;
	}

	public void AcquireEnemy(EnemyLogic enemy)
	{
		if (!enemy.npc.IsDead) {
			enemies.Add (enemy);
		}
	}
	
	void Update () 
	{
		transform.Rotate (new Vector3(0, Input.GetAxis("Mouse X") * mouseSensitivity, 0));
		HandleKeys ();
		enemies = new HashSet<EnemyLogic>(enemies.Where (e => !e.npc.IsDead));
		npc.anima.SetBool ("isInBattle", enemies.Count > 0);
	}

	void HandleKeys()
	{
		if (Tls.inst().IsPaused()) {
			return;
		}

		npc.Move (GetKeyedDirection ());

		if (npc.IsGrounded()) {
			if (Input.GetKeyDown(KeyCode.Space) && npc.Jump()) {
				Tls.inst ().PlayAudio (
					Random.Range(0, 10) == 0
						? jumpingEvilSound
						: jumpingSound);
			}
			if (Input.GetKeyDown (KeyCode.Mouse0)) {
				if (npc.Attack()) {
					// battle cry!
				} else {
					Tls.inst ().PlayAudio (outOfManaEvilSound);
				}
			}
			if (Input.GetKeyDown (KeyCode.Mouse1)) {
				npc.Parry ();
			}
			/** @debug */
			if (Input.GetKeyDown (KeyCode.G)) {
				Fluid.Inst ().PlayNote (69, 0);
			}
		} else {
			if (Input.GetKeyDown(KeyCode.Mouse0)) {
				if (npc.Boost(cameraAngle.transform.forward)) {
					Tls.inst ().PlayAudio (sprintingEvilSound);
				} else {
					Tls.inst ().PlayAudio (outOfManaEvilSound);
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
