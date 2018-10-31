using System;
using System.Text.RegularExpressions;
using Assets.Scripts.Util.Shorthands;

namespace Util.Shorthands
{
    public class Opt<T>
    {
        public bool isPresent;
        private T value;

        public D.Cu<T> get {
            set {
                if (isPresent) {
                    value(this.value);
                }
            }
        }

        public Opt (bool isPresent, T value)
        {
            this.isPresent = isPresent;
            this.value = value;
        }

        public static Opt<Tf> Some<Tf>(Tf valueArg)
        {
            return new Opt<Tf> (true, valueArg);
        }

        public static Opt<Tf> None<Tf>()
        {
            return new Opt<Tf> (false, default(Tf));
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

        public T Def(T fallback)
        {
            return isPresent ? value : fallback;
        }

        public bool Has()
        {
            return isPresent;
        }

        public T Unwrap()
        {
            if (isPresent) {
                return value;
            } else {
                throw new Exception ("Tried to unwrap absent value!");
            }
        }

        public U.ThenResult If(D.Cu<T> body)
        {
            if (isPresent) {
                body (value);
                return new U.ThenResult () { applied = true };
            } else {
                return new U.ThenResult () { applied = false };
            }
        }

        public T0 Match<T0>(D.F1<T, T0> mapper, D.Su<T0> elser)
        {
            return Has() ? mapper(value) : elser(); 
        }
    }
}

