using System.Collections;
using System.Collections.Generic;
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
            return new Opt<T>(value != null, value);
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

