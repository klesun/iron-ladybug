using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using System.Timers;
using System.Collections.Generic;
using Assets.Scripts.GameLogic;
using Assets.Scripts.Util.Shorthands;
using GameLogic;
using Interfaces;

namespace Util.GameLogic
{
    public class Dialogue : MonoBehaviour
    {
        public TextAsset script;
        public INpcMb speakerA = null; // defaults to player
        public INpcMb speakerB;

        private D.Cb sayNext = null;

        void Start ()
        {
            speakerA = speakerA ?? (Tls.Inst ().GetHero ().GetNpc());
        }

        void Update()
        {
            var skipButtonClicked =
                Input.GetKeyDown (KeyCode.Mouse0) ||
                Input.GetKeyDown (KeyCode.Space);

            var skipAllButtonClicked =
                Input.GetKeyDown (KeyCode.Mouse1) ||
                Input.GetKeyDown (KeyCode.Escape);

            if (sayNext != null && skipButtonClicked) {
                sayNext ();
            }
            if (skipAllButtonClicked) {
                StartCoroutine(skipAll ());
            }
        }

        IEnumerator skipAll()
        {
            while (sayNext != null) {
                yield return Timeout.WaitForRealSeconds(0.05f);
                sayNext ();
            }
        }

        public void Play(D.Cb cb = null)
        {
            cb = cb ?? (() => {});

            var unpause = Tls.Inst ().Pause ();
            Say (0, script.text.Split ('\n'), () => {
                unpause();
                cb();
            });
        }

        /** TODO: rename to SayCustom() */
        public void Say(int i, IList<string> quotes, D.Cb whenDone = null)
        {
            whenDone = whenDone ?? (() => {});

            var speaker = i % 2 == 0 ? speakerA : speakerB;
            if (i < quotes.Count) {
                Sa.Inst().gui.Say (quotes [i], speaker);
                D.Cb cb = () => Say (i + 1, quotes, whenDone);
                var seconds = GetReadingTime (quotes [i]);
                // TODO: it actually does not work when paused - implement
                // another SetTimeout that would not depend on game time
                sayNext = Tls.Inst ().timeout.Real(seconds, cb);
            } else {
                Sa.Inst().gui.EndTalk ();
                whenDone ();
                sayNext = null;
            }
        }

        static float GetReadingTime(string text)
        {
            return Mathf.Max(3, text.Length * 0.15f);
        }
    }
}
