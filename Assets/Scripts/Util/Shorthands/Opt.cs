using System;

namespace Util.Shorthands
{
    public class Opt<T>
    {
        public bool isPresent;
        private T value;

        private Opt ()
        {
        }

        public static Opt<Tf> Some<Tf>(Tf valueArg)
        {
            return new Opt<Tf> () { isPresent = true, value = valueArg };
        }

        public static Opt<Tf> None<Tf>()
        {
            return new Opt<Tf> () { isPresent = false };
        }

        public T Or(T defaultValue)
        {
            return isPresent ? value : defaultValue;
        }

        public Opt<T0> Map<T0>(D.F1<T, T0> mapper)
        {
            return isPresent
                ? Opt<T0>.Some (mapper (value))
                : Opt<T0>.None<T0> ();
        }

        public T Unwrap()
        {
            if (isPresent) {
                return value;
            } else {
                throw new Exception ("Tried to unwrap absent value!");
            }
        }

        public Util.U.IfResult If(D.Cu<T> body)
        {
            if (isPresent) {
                body (value);
                return new Util.U.IfResult () { applied = true };
            } else {
                return new Util.U.IfResult () { applied = false };
            }
        }
    }
}

