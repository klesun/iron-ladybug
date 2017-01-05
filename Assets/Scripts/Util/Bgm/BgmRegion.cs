using System.Collections.Generic;
using Assets.Scripts.Util.Logic;
using Assets.Scripts.Util.Shorthands;
using Interfaces;
using Newtonsoft.Json;
using UnityEngine;
using Util;
using Util.Midi;

namespace Assets.Scripts.Util.Bgm
{
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
            var playback = new Playback(parsedSong, true);
            foreach (var trigger in triggers) {
                trigger.onIn = (c) =>
                    U.If(c.GetComponent<IHero>() != null).then = () =>
                        Bgm.Inst().AddRegion(playback);
                trigger.onOut = (c) =>
                    U.If(c.GetComponent<IHero>() != null).then = () =>
                        Bgm.Inst().RemoveRegion(playback);;
            }
//            foreach (var trigger in triggers) {
//                trigger.onIn = (c) =>
//                    U.If(c.GetComponent<IHero>() != null).then = () => {
//                        // TODO: if some song is already being triggered, but have more time to switch than this, drop it
//                        ++inLevel;
//                        interruptStitch();
//                        interruptStitch = Tls.Inst().Timeout(stitchTime).Game(
//                            () => U.If(inLevel > 0).then =
//                            () => Bgm.Inst ().SetBgm (parsedSong).SetVolumeFactor(volumeFactor)
//                        );
//                    };
//                trigger.onOut = (c) =>
//                    U.If(c.GetComponent<IHero>() != null).then = () => {
//                        --inLevel;
//                        interruptStitch();
//                        interruptStitch = Tls.Inst().Timeout(stitchTime).Game(
//                            () => U.If(inLevel < 1).then =
//                            () => Bgm.Inst ().UnsetBgm (parsedSong)
//                        );
//                    };
//            }
        }
    }
}
