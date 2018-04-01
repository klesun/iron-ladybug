using System;
using Assets.Scripts.GameLogic;
using Assets.Scripts.Util.Logic;
using Assets.Scripts.Util.Shorthands;
using Interfaces;
using UnityEngine;
using Util;
using Util.Shorthands;

namespace Assets.Scripts.Util.Architecture
{
    /**
     * this is a button that reuires you stay in a
     * region for some time to trigger the action
     */
    public class StayStillButton: MonoBehaviour
    {
        // mandatory
        public SpaceTrigger region;

        // optional
        public float stayDuration = 2.0f;
        public AudioClip pressedSfx = null;
        public Rainbow platformColor = null;
        public Color pressingColor = Color.yellow;
        public Color pressedColor = Color.green;

        public bool wasPressed = false;
        private Opt<Tls.AnimateResult> moving = U.Opt<Tls.AnimateResult>(null);

        void Awake()
        {
            var startPos = transform.position;
            var startColor = U.Opt(platformColor).Map(r => r.color).Def(Color.gray);
            region.onIn =
                (col) => U.Opt(col.GetComponent<IHeroMb>()).get =
                (hero) => {
                    if (wasPressed) return;
                    Sa.Inst().gui.castProgrssBar.gameObject.SetActive(true);
                    U.Opt(platformColor).get =
                        (clr) => clr.color = pressingColor;
                    moving = U.Opt(Tls.Inst().Animate(100, stayDuration, (prog) => {
                        Sa.Inst().gui.castProgrssBar.value = prog;
                        transform.position = startPos + Vector3.down * prog * 0.5f;
                    }));
                    moving.get = anim => anim.thn = () => {
                        wasPressed = true;
                        Sa.Inst().gui.castProgrssBar.gameObject.SetActive(false);
                        U.Opt(pressedSfx).get = Tls.Inst().PlayAudio;
                        U.Opt(platformColor).get =
                            (clr) => clr.color = pressedColor;
                    };
                };
            region.onOut =
                (col) => U.Opt(col.GetComponent<IHeroMb>()).get =
                (hero) => {
                    if (wasPressed) return;
                    Sa.Inst().gui.castProgrssBar.gameObject.SetActive(false);
                    moving.get = anim => anim.Stp();
                    transform.position = startPos;
                    U.Opt(platformColor).get =
                        (clr) => clr.color = startColor;
                };
        }
    }
}
