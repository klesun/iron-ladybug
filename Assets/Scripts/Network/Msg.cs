using UnityEngine;

namespace Network {
    public class V2 {
        public float x;
        public float y;
    }

    public class Msg {
        public enum EType {KeyDown, KeyUp, Sync, MouseMove};
        public EType type;
        public KeyCode keyCode;
        public V2 mouseDelta;
        public readonly float time = Time.fixedTime; 
    }
}