using System;
using UnityEngine;
using Util;
using System.Collections.Generic;
using Assets.Scripts.Util.Shorthands;

namespace Util.SoundFontPlayer
{
    public class Fluid
    {
        const float VOLUME_FACTOR = 0.5f;

        private static Fluid instance;
        private static D.F1<int, float> semitoneToHertz = s => 440 * Mathf.Pow (2, (s - 69) / 12.0f);

        public Fluid ()
        {
        }

        public static Fluid Inst()
        {
            return instance
                ?? (instance = new Fluid());
        }

        public D.Cb PlayNote(int semitone, int presetN, float volumeFactor = 1)
        {
            var adapted = Adapter.Get (semitone, presetN);

            var audioSourceEl = new GameObject ("staticAudio", typeof(AudioSource), typeof(AudioSource));
            var audioSources = S.List(audioSourceEl.GetComponents<AudioSource> ());
            audioSources.ForEach ((audioSource) => {
                audioSource.clip = adapted.audioClip;
                audioSource.pitch = adapted.frequencyFactor;
                audioSource.volume = VOLUME_FACTOR * volumeFactor;
            });


            // broken
            // TODO: fix
            // var stop = LoopSeamless (S.Queue(audioSources), adapted.loopStart, adapted.loopEnd, semitone);
            audioSources[0].Play();

            return () => {
                // stop();
                audioSources.ForEach((a) => a.Stop());
                GameObject.Destroy(audioSourceEl);
            };
        }

        /**
         * @param srcSwaps expected to be at least two elements long
         */
        static D.Cb LoopSeamless(Queue<AudioSource> srcSwaps, float loopStart, float loopEnd, int semitone)
        {
            var interrupted = false;
            var loopLength = loopEnd - loopStart;

            var now = AudioSettings.dspTime;
            var busy = srcSwaps.Dequeue ();
            var free = srcSwaps.Dequeue ();
            busy.PlayScheduled(now);
            busy.SetScheduledEndTime(now + loopEnd);
            var loopEndTime = now + loopEnd;

            var wavePeriod = 1.0f / semitoneToHertz (semitone);
            // taking nearest to quartet round number
            var crossFadeTime = Mathf.Floor(loopLength / 4 / wavePeriod) * wavePeriod;

            // TODO: fix... well, it definitely sounds not how one would expect
            // maybe my fancy way of doing SetTimeout() is the reason of detiming
            // fade does not work too

            D.Cb swap = null;
            swap = () => U.If(!interrupted, () => {
                now = AudioSettings.dspTime;

                free.time = loopStart;
                free.PlayScheduled(loopEndTime - crossFadeTime);
                loopEndTime = loopEndTime - crossFadeTime + loopLength;
                free.SetScheduledEndTime(loopEndTime);

                // something does not work -_-
//                Tls.inst().SetTimeout((float)(loopEndTime - crossFadeTime - now), () => {
//                    Fade(true, busy, crossFadeTime);
//                    Fade(false, free, crossFadeTime);
//                });

                var tmp = free;
                free = busy;
                busy = tmp;

                Tls.Inst().timeout.Real ((float)(loopEndTime - now - (loopLength - crossFadeTime) / 2), swap);
            });
            Tls.Inst().timeout.Real ((float)(loopEndTime - now - (loopLength - crossFadeTime) / 2), swap);

            return () => interrupted = true;
        }

        static void Fade(bool outward, AudioSource src, float duration)
        {
            var steps = 10;
            var step = 0;
            D.Cb doStep = null;
            doStep = () => U.If (src != null && ++step < steps, () => {
                src.volume = VOLUME_FACTOR * (outward ? steps - step : step) / steps;
                Tls.Inst().timeout.Real((float)(duration / steps), doStep);
            });
            doStep ();
        }
    }
}

