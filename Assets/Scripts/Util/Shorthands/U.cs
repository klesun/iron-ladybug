using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Util.Shorthands;

namespace Assets.Scripts.Util.Shorthands
{
    /**
     * U stands for "Uporotostj"
     * the thing is: i hate brackets. i HATE them
     * this class provides some calls that would
     * be a less traditional way of writing code
     */
    public static class U
    {
        public static ThenResult IfOld(bool condition, D.Cb body)
        {
            if (condition) {
                body ();
                return new ThenResult () { applied = true };
            } else {
                return new ThenResult () { applied = false };
            }
        }

        public static IfResult If(bool condition)
        {
            return new IfResult() { applied = condition };
        }

        public static Opt<T> Opt<T>(T value)
        {
            // sometimes some Transform var that is real "null" inside MonoBehaviour, becomes
            // a Transform that throws exceptions on access when you pass it outside. Casting it to string
            // results in "null". I guess this is the only way to check if value is _really_ not null
            var isNull = value == null || value + "" == "null";
            return new Opt<T>(!isNull, value);
        }

        public static L<T> L<T>(IList<T> elements)
        {
            return new L<T>(elements);
        }

        public class IfResult
        {
            public bool applied = true;

            public D.Cb then {
                set {
                    if (applied) {
                        value();
                    }
                }
            }

            public ThenResult Then(D.Cb cb)
            {
                if (applied) {
                    cb();
                }
                return new ThenResult() { applied = applied };
            }
        }

        public class ThenResult
        {
            public bool applied = true;

            public void Else(D.Cb elseBody)
            {
                if (!applied) {
                    elseBody ();
                }
            }
        }
    }
}

