using System;
using Assets.Scripts.Util.Shorthands;
using UnityEngine;
using Util;
using Util.Midi;
using Util.Shorthands;

namespace Assets.Scripts.Util.Bgm
{
    public class Bgm
    {
        private static Bgm instance;
        private D.Cb stopPlayback = () => {};

        private L<Playback> pendingSongs;
        // TODO: guarantee that new songs starts playing exactly when old song tact ends
        private int tactsPassedBeforeSongStart = 0;
        private float songStartedAt = 0;
        private float tempo = 120;

        public Bgm()
        {
            pendingSongs = new L<Playback> ();
        }

        public static Bgm Inst()
        {
            return instance
                ?? (instance = new Bgm());
        }

        public Playback SetBgm(MidJsDefinition song)
        {
            if (song != pendingSongs.Last ().Map (p => p.song).Or (null)) {
                var player = new Playback (song);
//                {
//                    whenDone = () => U.If(!interrupted, () => UnsetBgm(song)),
//                };
                pendingSongs.s.Add (player);
                Play ();
                return player;
            } else {
                return pendingSongs.Last ().Unwrap ();
            }
        }

        public void UnsetBgm(MidJsDefinition song)
        {
            if (song == pendingSongs.Last ().Map (p => p.song).Or (null)) {
                pendingSongs.Pop ();
                Play ();
            } else {
                var idx = pendingSongs.s.FindLastIndex (ps => ps.song == song);
                if (idx > -1) {
                    pendingSongs.s.RemoveAt (idx);
                }
            }
        }

        /**
         * @return number of tacts that passed since
         * start of game + current tact completion factor
         */
        public float GetTactProgress()
        {
            return tactsPassedBeforeSongStart + GetSongCurrentTime();
        }

        public float GetSongCurrentTime()
        {
            // 240 is a tempo in which whole note matches one second
            return (Time.fixedTime - songStartedAt) * tempo / 240;
        }

        void ResetTactCounter(float newTempo)
        {
            // implying tact size is always 4/4
            // this should be changed in future
            tactsPassedBeforeSongStart += (int)GetSongCurrentTime();
            tempo = newTempo;
            songStartedAt = Time.fixedTime;
        }

        void Play()
        {
            stopPlayback ();
            stopPlayback = () => {};
            pendingSongs.Last ().If (player => {
                var interrupted = false;
                player.whenDone = () => U.If(!interrupted, () => UnsetBgm(player.song));
                ResetTactCounter(player.song.staffList[0].staffConfig.tempo);
                var stopPlaybackTmp = player.Play ();
                stopPlayback = () => {
                    stopPlaybackTmp();
                    interrupted = true;
                };
            });
        }
    }
}