using System.Collections.Generic;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Util.Shorthands;
using Interfaces;
using UnityEngine;
using UnityEngine.UI;
using Util;
using Util.Shorthands;
using UnityEngine.UI;

namespace Assets.Scripts.GameLogic
{
    [RequireComponent (typeof(Rigidbody))]
    [RequireComponent (typeof(Collider))]
    [SelectionBase]
    public class NpcControl : INpcMb, IPiercable
    {
        public const float LOUNGE_HEIGHT = 1.5f;
        public const float LOUNGE_LENGTH = 6;

        const float SPRINT_INTERVAL = 3;

        const float FRICTION_FORCE = 12;
        const float MAX_RUNNING_SPEED = 13;
        public const float RUNNING_BOOST = 18;
        public const float JUMP_BOOST = 8;

        public static readonly int START_HP = 100;

        public int health;
        public UnityEngine.UI.Slider HealthBar;

        public Animator anima;
        // optional
        public EmmitterControl boostEmmitter = null;
        public Blade epee;
        public Texture icon;
        public List<ISkillMb> skills = new List<ISkillMb>();

        public Rigidbody body;
        private bool isDead = false;
        public bool IsDead {
            get { return isDead; }
        }
        private float? lastSprintTime = null;
        private float distToGround;
        private RaycastHit floor;
        private bool isCloseToGround = false;
        private float lastGroundTime;
        private bool isFlying = true;

        void Awake ()
        {
            var hbX = Screen.width - 140;
            var hbY = Screen.height - Screen.height + 40;
            if (HealthBar) {
                HealthBar.transform.position = new Vector3(hbX, hbY, 1);
            }
            SetHealth(START_HP);
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

        public void ChangeHealth(int diff)
        {
            SetHealth(GetHealth() + diff);
        }

        public void SetHealth(int value)
        {
            health = value;
            if (health < 1) {
                Die();
            }
            RenderHealthBar();
        }

        public int GetHealth ()
        {
            return health;
        }

        void RenderHealthBar()
        {
            if (HealthBar) {
                HealthBar.value = GetHealth();
            }
        }

        void Live()
        {
            isCloseToGround = Physics.Raycast (
                transform.position, -Vector3.up * 0.1f, out floor, distToGround + 0.1f,
                layerMask: -5, queryTriggerInteraction: QueryTriggerInteraction.Ignore
            );

            if (GetHealth() <= 0) {
                Die ();
            } else {
                if (IsGrounded ()) {
                    isFlying = false;
                    lastGroundTime = Time.fixedTime;
                    anima.SetBool ("isFlying", false);
                    ApplyFriction ();
                } else {
                    if (Time.fixedTime - lastGroundTime > 0.2) {
                        isFlying = true;
                        anima.SetBool ("isFlying", true);
                        anima.SetBool ("isInBattle", false);
                    }
                }

                epee.isParrying = anima.GetCurrentAnimatorStateInfo (0).IsName ("Armature|batman");

                anima.SetFloat ("xSpeed", body.velocity.x);
                anima.SetFloat ("ySpeed", body.velocity.z);
            }
        }

        public override void Die()
        {
            Tls.Inst().PlayAudio(Sa.Inst ().audioMap.npcDeathScream);
            isDead = true;
            body.constraints = 0; // so it fell
            body.velocity -= transform.forward * 3;
            epee.Disarm ();
            transform.parent = null;

            if (body.GetComponent<IHeroMb>())
            {
                var go = GameObject.Find("death_text");
                if (go) {
                    foreach (var death_text in go.GetComponents<Text>()) {
                        death_text.transform.position = new Vector3(Screen.width / 2, Screen.height / 2, 1);
                        death_text.enabled = true;
                    }
                }
                Camera.main.transform.parent = null;
            }

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
                AudioSource.PlayClipAtPoint (Sa.Inst().audioMap.npcEpeeSwingSfx, transform.position);
                return true;
            }
            return false;
        }

        public bool UseSkill(ISkillMb skill)
        {
            if (CanAttack()) {
                anima.SetTrigger ("parrying");
                lastSprintTime = Time.fixedTime;
                skill.Perform(this);
                return true;
            } else {
                return false;
            }
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

        public bool Jump(float boost = JUMP_BOOST)
        {
            if (!isDead && !isFlying) {
                anima.SetBool ("isFlying", true);
                /** @debug */
                body.velocity = new Vector3(body.velocity.x, 0, body.velocity.z);

                body.velocity += Vector3.up * boost;

                AudioSource.PlayClipAtPoint(Sa.Inst().audioMap.npcJumpSfx, transform.position);
                lastGroundTime += -100;
                return true;
            }
            return false;
        }

        public bool Boost(Vector3 direction)
        {
            if (!isDead) {
                if (lastSprintTime == null ||
                    Time.fixedTime - lastSprintTime > SPRINT_INTERVAL
                ) {
                    body.velocity += direction * 10;
                    lastSprintTime = Time.fixedTime;
                    AudioSource.PlayClipAtPoint(Sa.Inst().audioMap.npcSprintSfx, transform.position);
                    if (boostEmmitter != null) {
                        boostEmmitter.Emmit ();
                    }
                    return true;
                }
            }
            return false;
        }

        public void GetPierced()
        {
            if (!isDead) {
                SetHealth(GetHealth() - 20);
                AudioSource.PlayClipAtPoint(Sa.Inst().audioMap.npcHitSfx, transform.position);
                LoseGrip();
                Sa.Inst().gui.SayShyly("HP left: " + GetHealth(), this);
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

        public override bool IsGrounded()
        {
            return isCloseToGround && (
                body.velocity.magnitude < 0.1 ||
                Vector3.Angle(floor.normal, body.velocity) >= 89.99 &&
                Vector3.Angle(floor.normal, Vector3.up) <= 45.00
            );
        }

        public Opt<Vector3> GetGround()
        {
            return IsGrounded()
                ? S.Opt(transform.position)
                : S.Opt(floor).Map(r => r.point);
        }

        public float GetMpFactor()
        {
            if (lastSprintTime != null) {
                var mpFactor = (Time.fixedTime - lastSprintTime.Value) / SPRINT_INTERVAL / 1.0f;
                return mpFactor > 1.0f ? 1.0f : mpFactor;
            } else {
                return 1.0f;
            }
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
