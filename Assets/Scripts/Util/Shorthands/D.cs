using System;

namespace Util
{
    /** D stands for "Delegates" */
    public class D
    {
        /** "Function" */
        public delegate Tout F1<Tin, Tout>(Tin value);
        /** "CallBack" */
        public delegate void Cb();
        /** "Consumer" */
        public delegate void Cu<T>(T value);
        public delegate void Cu2<T, T2>(T value, T2 value2);
        public delegate T Su<T>();
    }
}

