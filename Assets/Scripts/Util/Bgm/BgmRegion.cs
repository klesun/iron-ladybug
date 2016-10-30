using UnityEngine;
using System.Collections;
using Newtonsoft.Json;
using Util.Midi;
using GameLogic;
using Interfaces;
using System.Collections.Generic;
using Assets.Scripts.Util.Bgm;

namespace Util
{
    /**
     * TODO: move to GameLogic namespace or use some other
     * way do determine that game object is player's npc
    */
    public class BgmRegion : MonoBehaviour
    {
        public TextAsset audioFile;
        public List<SpaceTrigger> triggers;

        public float stitchTime = 5;
        public float volumeFactor = 1;

        private static Dictionary<TextAsset, MidJsDefinition> songByPath = new Dictionary<TextAsset, MidJsDefinition>();

        private int inLevel = 0;
        private D.Cb interruptStitch = () => {};

        void Awake ()
        {
            if (!songByPath.ContainsKey(audioFile)) {
                var text = audioFile.text;
                songByPath[audioFile] = JsonConvert.DeserializeObject<MidJsDefinition> (text);
            }
            var parsedSong = songByPath [audioFile];
            foreach (var trigger in triggers) {
                trigger.OnIn((c) => U.If(
                    c.GetComponent<IHero>() != null,
                    () => {
                        ++inLevel;
                        interruptStitch();
                        interruptStitch = Tls.Inst().timeout.Game(stitchTime, () =>
                            U.If(inLevel > 0, () => Bgm.Inst ().SetBgm (parsedSong).SetVolumeFactor(volumeFactor)));
                    }
                ));
                trigger.OnOut((c) => U.If(
                    c.GetComponent<IHero>() != null,
                    () => {
                        --inLevel;
                        interruptStitch();
                        interruptStitch = Tls.Inst().timeout.Game(stitchTime, () =>
                            U.If(inLevel < 1, () => Bgm.Inst ().UnsetBgm (parsedSong)));
                    }
                ));
            }
        }
    }
}
