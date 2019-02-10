using UnityEngine;

namespace Network {
    public class V2 {
        public float x;
        public float y;

        public Vector2 toStd()
        {
            return new Vector2(x, y);
        }
    }

    public class Msg {
        public enum EType {KeyDown, KeyUp, Sync, MouseMove, CastSpell, Error};
        public EType type;
        public string strValue = "";
        public KeyCode keyCode;
        public V2 mouseDelta;
        public readonly float time = Time.fixedTime;
    }
}