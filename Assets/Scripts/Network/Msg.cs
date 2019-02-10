using System.Collections.Generic;
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

    public class HeroState {
        public int hp;
        public int hpMax;
        public float mpFactor;
        public string[] spells = {};
    }

    public class Msg {
        public enum EType {
            /*from client: */ KeyDown, KeyUp, Sync, MouseMove, CastSpell,
            /*from server: */ Error, State,
        };
        public EType type;
        public string strValue = "";
        public KeyCode keyCode;
        public V2 mouseDelta;
        public readonly float time = Time.fixedTime;
        public HeroState state = null;
    }
}