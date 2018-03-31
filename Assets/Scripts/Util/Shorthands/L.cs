using System;
using System.Collections.Generic;
using UnityEngine.AI;
using Util;
using Util.Shorthands;

namespace Assets.Scripts.Util.Shorthands
{
    /**
     * a wrapper for List
     *
     * apparently, methods like "RemoveAt()" return void in C# - unforgivable!
     *
     * Note: if you need this for primitives, define a separate class cuz i cannot
     */
    public class L<T>
    {
        public readonly List<T> s;

        public D.Cu2<T, int> each { set {
            for (var i = 0; i < s.Count; ++i) {
                value(s[i], i);
            }
        } }

        public L (IEnumerable<T> subj)
        {
            this.s = new List<T> (subj);
        }

        public L ()
        {
            this.s = new List<T> ();
        }

        public static L<Tf> N<Tf>(IEnumerable<Tf> subj)
        {
            return new L<Tf> (subj);
        }

        /**
         * built-in list does not have it and answers on SO suggest using Queue
         * but Queue have some limitations. for example, it does not have "RemoveAt()"
         */
        public Opt<T> Pop()
        {
            if (s.Count > 0) {
                var tmp = s [s.Count - 1];
                s.RemoveAt (s.Count - 1);
                return Opt<T>.Some(tmp);
            } else {
                return Opt<T>.None<T> ();
            }
        }

        /** "first" */
        public Opt<T> Fst()
        {
            return s.Count > 0
                ? Opt<T>.Some(s[0])
                : Opt<T>.None<T>();
        }

        /**
         * built-in Linq version throws InvalidOperationException
         */
        public Opt<T> Last()
        {
            return s.Count > 0
                ? Opt<T>.Some(s [s.Count - 1])
                : Opt<T>.None<T>();
        }

        public L<Tn> Change<Tn>(D.F1<List<T>, IEnumerable<Tn>> changer)
        {
            return new L<Tn>(changer(this.s));
        }

        /** filter */
        public L<T> Flt(Predicate<T> pred)
        {
            var newList = new List<T>();
            foreach (var el in s) {
                if (pred(el)) {
                    newList.Add(el);
                }
            }
            return new L<T>(newList);
        }

        /** sort */
        public L<T> Srt(Func<T, IComparable> getOrder)
        {
            s.Sort((a,b) => getOrder(a).CompareTo(getOrder(b)));
            return this;
        }
    }
}

