using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using Assets.Scripts.Util.Bgm;
using Assets.Scripts.Util.Logic;
using Util;

namespace GameLogic
{
    public class Strawberry : ITrophy
    {
        public AudioClip collectedSound = null;
        public AudioClip collectedEvilSound = null;
        public SpaceTrigger trigger;
        public ETrophy trophyName;

        private D.Cb onCollected = () => {};

        void Awake ()
        {
            trigger.OnIn(OnGrab);
        }

        void OnGrab(Collider collider)
        {
            if (collider.gameObject.GetComponent<HeroControl>() != null) {
                var snd = collectedSound == null || collectedEvilSound != null && Random.Range (0, 10) == 0
                    ? collectedEvilSound
                    : collectedSound;

                if (snd) {
                    Tls.Inst ().PlayAudio (snd);
                }
                if (trophyName == ETrophy.ORB_OF_MOTIVATION) {
                    var bgm = Sa.Inst ().audioMap.missionCompleteBgm;
                    Bgm.Inst ().SetBgm (bgm);
                }

                onCollected ();
                Destroy (gameObject);
            }
        }

        override public void SetOnCollected(D.Cb cb)
        {
            onCollected = cb;
        }

        override public ETrophy GetName()
        {
            return trophyName;
        }
    }
}