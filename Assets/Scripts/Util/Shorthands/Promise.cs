using Assets.Scripts.Util.Shorthands;

namespace Util.Shorthands {

    /**
     * I believe ES6's promise is something similar to this.
     * This thing represents a value that is retrieved asynchronously.
     * You can hang handlers on it that will be called once value is retrieved
     * (or instantly if retrieved already)
     */
    public class Promise<T>
    {
        private bool done = false;
        private T result;
        private L<D.Cu<T>> thens = new L<D.Cu<T>>();

        public D.Cu<T> thn { set {
            if (done) {
                value(result);
            } else {
                thens.s.Add(value);
            }
        } }

        public Promise(D.Cu<D.Cu<T>> giveMemento)
        {
            giveMemento(result => {
                this.done = true;
                this.result = result;
                this.thens.each = (cb,i) => cb(result);
            });
        }
    }
}
