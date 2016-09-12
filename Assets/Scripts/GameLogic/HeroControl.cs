using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using AssemblyCSharp;
using Util.Midi;
using Newtonsoft.Json;
using Util;
using Util.SoundFontPlayer;
using Interfaces;

namespace GameLogic
{
	public class HeroControl : IHeroMb
	{
		public GameObject cameraAngle;
		public AudioClip jumpingSound;
		public AudioClip jumpingEvilSound;
		public AudioClip sprintingEvilSound;
		public AudioClip outOfManaEvilSound;

		public NpcControl npc;
		public HeroStats stats;
		public GuiControl gui;

		/** @debug */
		public TextAsset testSong;

		private float mouseSensitivity = 4.0F;
		private HashSet<EnemyLogic> enemies = new HashSet<EnemyLogic>();

		void Awake () 
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
					var stop = Fluid.Inst ().PlayNote (35, 43);
					Tls.inst ().SetTimeout (5f, () => {
						stop();
						stop = Fluid.Inst ().PlayNote (38, 43);
						Tls.inst ().SetTimeout (1f, () => {
							stop();
							stop = Fluid.Inst ().PlayNote (39, 43);
							Tls.inst ().SetTimeout (1f, () => {
								stop();
								stop = Fluid.Inst ().PlayNote (41, 43);
								Tls.inst ().SetTimeout (1f, stop);
							});
						});
					});
				}
				/** @debug */
				if (Input.GetKeyDown (KeyCode.H)) {
					new Player (JsonConvert.DeserializeObject<MidJsDefinition> (testSong.text)).Play();
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

		override public INpcMb GetNpc()
		{
			return npc;
		}
	}
}
