using System.Linq;
using Assets.Scripts.Util.Shorthands;
using UnityEngine;
using Util;
using Util.Midi;
using Util.SoundFontPlayer;

namespace Assets.Scripts.Util.Bgm
{
    public class Playback
    {
        public D.Cb whenDone = () => {};
        public readonly MidJsDefinition song;

        int position = 0;
        Staff staff;
        int loopsLeft;
        float volumeFactor = 1;
        readonly bool useGameTime;

        public Playback (MidJsDefinition song, bool useGameTime = false)
        {
            this.song = song;
            this.staff = song.staffList[0];
            this.useGameTime = useGameTime;
            loopsLeft = this.staff.staffConfig.loopTimes;
        }

        public D.Cb Play()
        {
            var flag = new RunInfo();

            // timeouting so playback started in the next assync code block
            // to allow user set some configuration in the next statements
            Timeout(0.001f, () => PlayNext (flag, GetTime()));

            return () => flag.isInterrupted = true;
        }

        int FindByTime(float chordTime)
        {
            var sumFrac = 0f;
            for (var i = 0; i < staff.chordList.Length; ++i) {
                if (sumFrac >= chordTime) {
                    return i;
                } else {
                    sumFrac += S.L(staff.chordList[i].noteList)
                        .Change(l => l.Select(n => ParseFraction(n.length)))
                        .Change(l => l.OrderBy(n => n))
                        .Last().Or(0.0f);
                }
            }

            return -1;
        }

        void PlayNext(RunInfo flag, float expectedTime)
        {
            var actualTime = GetTime();
            if (!flag.isInterrupted) {
                if (position < staff.chordList.Length) {
                    var chord = staff.chordList [position++];
                    var seconds = PlayChord (chord);
                    Timeout (
                        seconds - (actualTime - expectedTime),
                        () => PlayNext (flag, expectedTime + seconds)
                    );
                } else if (loopsLeft > 0) {
                    --loopsLeft;
                    position = FindByTime(staff.staffConfig.loopStart);
                    PlayNext (flag, expectedTime);
                } else {
                    whenDone ();
                }
            }
        }

        public Playback SetVolumeFactor(float value)
        {
            this.volumeFactor = value;
            return this;
        }

        float PlayChord(Chord chord)
        {
            float? chordLength = null;
            foreach (var note in chord.noteList) {
                var noteLength = ParseFraction (note.length);
                var instr = S.List(staff.staffConfig.channelList)
                    .Where(c => c.channelNumber == note.channel)
                    .Select(c => c.instrument)
                    .FirstOrDefault();

                var isRest = note.channel == 9 && note.tune == 0;
                var stop = !isRest
                    ? Fluid.Inst ().PlayNote (note.tune, instr, volumeFactor)
                    : () => {};
                Timeout (AcadToSeconds(noteLength), stop);

                chordLength = chordLength != null ? Mathf.Min (chordLength ?? 0, noteLength) : noteLength;
            }

            return AcadToSeconds(chordLength ?? 0);
        }

        float GetTime()
        {
            return useGameTime
                ? Time.fixedTime
                : Time.realtimeSinceStartup;
        }

        D.Cb Timeout(float seconds, D.Cb callback)
        {
            if (seconds <= 0) {
                callback();
                return () => {};
            } else {
                var tout = Tls.Inst().Timeout(seconds);
                return useGameTime
                    ? tout.Game(callback)
                    : tout.Real(callback);
            }
        }

        static float ParseFraction(string fraction)
        {
            var tokens = fraction.Split ('/').Select(t => t.Trim()).ToList();
            var num = float.Parse(tokens[0]);
            var denom = tokens.Count == 2 ? float.Parse(tokens[1]) : 1;
            return num / denom;
        }

        float AcadToSeconds(float length)
        {
            // semibreve takes 1 second when tempo is 240
            return 240f * length / staff.staffConfig.tempo;
        }

        public class RunInfo
        {
            public bool isInterrupted = false;
        }
    }
}

