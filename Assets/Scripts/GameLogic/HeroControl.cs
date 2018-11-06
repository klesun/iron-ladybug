using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Util.Bgm;
using Assets.Scripts.Util.Shorthands;
using GameLogic;
using GameLogic.Entities;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;
using Util;
using Util.Controls;
using Util.Midi;
using Util.SoundFontPlayer;

namespace Assets.Scripts.GameLogic
{
    public class HeroControl : IHeroMb
    {
        public MouseLook cameraAngle;
        public AudioClip jumpingSound;
        public AudioClip jumpingEvilSound;
        public AudioClip sprintingEvilSound;
        public AudioClip outOfManaEvilSound;

        public NpcControl npc;
        public HeroStats stats;

        /** @debug */
        public TextAsset testSong;

        private float mouseSensitivity = 4.0F;
        private HashSet<EnemyLogic> enemies = new HashSet<EnemyLogic>();
        private MidJsDefinition currentBattleBgm = null;

        private IPlayerInput input = new LocalPlayerInput();

        public void SetInput(IPlayerInput input)
        {
            this.input = input;
        }

        void Awake ()
        {
            Cursor.lockState = CursorLockMode.Locked;
            currentBattleBgm = Sa.Inst().audioMap.battleBgm;
        }

        public void AcquireEnemy(EnemyLogic enemy)
        {
            if (!enemy.npc.IsDead) {
                enemies.Add (enemy);
            }
        }

        void Update ()
        {
            if (!Tls.Inst().IsPaused()) {
                transform.Rotate (new Vector3(0, input.GetMouseDelta().x * mouseSensitivity, 0));
                cameraAngle.Rotate (input.GetMouseDelta().y);
                HandleKeys ();
            }
            enemies = new HashSet<EnemyLogic>(enemies.Where (e => !e.npc.IsDead));
            if (enemies.Count > 0) {
                npc.anima.SetBool ("isInBattle", true);
                if (currentBattleBgm == null) {
                    currentBattleBgm = Sa.Inst().audioMap.battleBgm;
                    foreach (var enemy in enemies) {
                        currentBattleBgm = U.Opt(enemy.bgm)
                            .Map(ebgm => ebgm.getParsed())
                            .Def(currentBattleBgm);
                    }
                    Bgm.Inst().SetBgm(currentBattleBgm).SetVolumeFactor(0.4f);
                }
            } else {
                npc.anima.SetBool ("isInBattle", false);
                Bgm.Inst().UnsetBgm(currentBattleBgm);
                currentBattleBgm = null;
            }
        }

        private void OpenSpellBook()
        {
            new SpellBook(this).Open();
        }

        void HandleKeys()
        {
            npc.Move (GetKeyedDirection ());

            if (input.GetKeyDown(KeyCode.E)) {
                OpenSpellBook();
            }

            if (input.GetKeyDown(KeyCode.Space) && npc.Jump()) {
                Tls.Inst ().PlayAudio (
                    Random.Range(0, 10) == 0
                    ? jumpingEvilSound
                    : jumpingSound);
            }

            if (npc.IsGrounded()) {
                if (input.GetKeyDown (KeyCode.Mouse0)) {
                    Cursor.lockState = CursorLockMode.Locked;
                    if (npc.Attack()) {
                        // battle cry!
                    } else {
                        Tls.Inst ().PlayAudio (outOfManaEvilSound);
                    }
                }
                if (input.GetKeyDown (KeyCode.Mouse1)) {
                    npc.Parry ();
                }
                if (input.GetKeyDown (KeyCode.Tab)) {
                    CheckpointUtil.Inst().JumpToNext(this);
                }
                /** @debug */
                if (input.GetKeyDown (KeyCode.G)) {
                    var stop = Fluid.Inst ().PlayNote (35, 43);
                    Tls.Inst ().SetGameTimeout (5f, () => {
                        stop();
                        stop = Fluid.Inst ().PlayNote (38, 43);
                        Tls.Inst ().SetGameTimeout (1f, () => {
                            stop();
                            stop = Fluid.Inst ().PlayNote (39, 43);
                            Tls.Inst ().SetGameTimeout (1f, () => {
                                stop();
                                stop = Fluid.Inst ().PlayNote (41, 43);
                                Tls.Inst ().SetGameTimeout (1f, stop);
                            });
                        });
                    });
                }
                /** @debug */
                if (input.GetKeyDown (KeyCode.H)) {
                    new Playback (JsonConvert.DeserializeObject<MidJsDefinition> (testSong.text)).Play();
                }
            } else {
                if (input.GetKeyDown(KeyCode.Mouse0)) {
                    Cursor.lockState = CursorLockMode.Locked;
                    if (npc.Boost(cameraAngle.transform.forward)) {
                        Tls.Inst ().PlayAudio (sprintingEvilSound);
                    } else {
                        Tls.Inst ().PlayAudio (outOfManaEvilSound);
                    }
                }
            }
        }

        Vector3 GetKeyedDirection()
        {
            var result = new Vector3 ();

            if (input.GetKey(KeyCode.W)) {
                result += transform.forward;
            }
            if (input.GetKey(KeyCode.S)) {
                result -= transform.forward;
            }
            if (input.GetKey(KeyCode.A)) {
                result -= transform.right;
            }
            if (input.GetKey(KeyCode.D)) {
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
