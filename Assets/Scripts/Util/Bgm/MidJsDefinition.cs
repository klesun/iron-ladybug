using UnityEngine;

namespace Util.Midi
{
    [System.Serializable]
    public class MidJsDefinition
    {
        public Staff[] staffList;
    }

    [System.Serializable]
    public class Staff
    {
        public StaffConfig staffConfig;
        public Chord[] chordList;
    }

    [System.Serializable]
    public class Chord
    {
        public Note[] noteList;
        // represents float absolute chord start time
        // in note lengths: (1/2, 10/4, 15/3, ...)
        public float? timeFraction;
    }

    [System.Serializable]
    public class Note
    {
        // 1.0 - semibreve; 0.25 - quarter note; 0.1666 - triplet of a half note; and so on
        // depending on circumstances may be either a float or fraction string
        public string length;
        // midi channel number in range [0..16)
        public int channel;
        // midi noteOn event second byte - range [0..128)
        public int tune;
        // 127 (default) - max volume; 0 - zero volume.
        public int? velocity;
    }

    [System.Serializable]
    public class StaffConfig
    {
        /*
         * when tempo is 60: 1/4 note length = 1 second;
         * when tempo is 240: 1/4 note length = 1/4 second
         */
        public float tempo;
        /*
         * a number in range [-7, 7]. when -1: Ti is flat;
         * when -2: Ti and Mi are flat; when +2: Fa and Re are sharp and so on...
         */
        public int keySignature;
        /*
         * tact size in legacy format (i used to store numbers in bytes ages ago...)
         * if you divide it with, uh well, 8, you get the tact size
         */
        public int? numerator;
        // loop start tact number
        public int loopStart = 0;
        // count of times playback will be rewinded
        // to the loopStart after last chord
        public int loopTimes;
        public Channel[] channelList;
    }

    [System.Serializable]
    public class Channel
    {
        // midi program number in range [0..127]
        public int instrument;
        // midi channel number in range [0..16)
        public int channelNumber;
        // midi channel starting volume [0..127]
        public int? volume;
    }
}