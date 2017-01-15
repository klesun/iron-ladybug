namespace Assets.Scripts.Interfaces {
    public interface IAnimatable {
        /** @param progress - value in [0..1] */
        void OnProgress(float progress);
    }
}