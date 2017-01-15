namespace Assets.Scripts.Util.Shorthands {
    /** tuple of 4 elements */
    public class T4<T> {
        public readonly T a;
        public readonly T b;
        public readonly T c;
        public readonly T d;

        public T4(T a, T b, T c, T d)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.d = d;
        }
    }
}