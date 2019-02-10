using System.Collections.Generic;
using Network;
using Interfaces;
using JetBrains.Annotations;
using UnityEngine;

namespace Util.Controls {
    public class RemotePlayerInput : IPlayerInput {
        HashSet<KeyCode> pressedKeys = new HashSet<KeyCode>();
        HashSet<KeyCode> frameKeys = new HashSet<KeyCode>();
        Vector2 mouseDelta = Vector2.zero;
        Queue<string> spellQueue = new Queue<string>();
        int lastFrame = -1;

        public bool GetKeyDown(KeyCode key)
        {
            if (lastFrame == -1) {
                lastFrame = Time.frameCount;
            }
            if (lastFrame == Time.frameCount) {
                return frameKeys.Contains(key);
            } else {
                frameKeys = new HashSet<KeyCode>();
                return false;
            }
        }

        public bool GetKey(KeyCode key)
        {
            return pressedKeys.Contains(key);
        }

        public string GetNextSpell()
        {
            if (spellQueue.Count > 0) {
                return spellQueue.Dequeue();
            } else {
                return null;
            }
        }

        public void HandleEvent(Msg msgData)
        {
            lastFrame = -1;
            mouseDelta = Vector2.zero;
            if (msgData.type == Msg.EType.KeyDown) {
                pressedKeys.Add(msgData.keyCode);
                frameKeys.Add(msgData.keyCode);
            } else if (msgData.type == Msg.EType.KeyUp) {
                pressedKeys.Remove(msgData.keyCode);
            } else if (msgData.type == Msg.EType.MouseMove) {
                mouseDelta = msgData.mouseDelta.toStd();
            } else if (msgData.type == Msg.EType.CastSpell) {
                spellQueue.Enqueue(msgData.strValue);
            } else {
                Debug.Log("unsupported socket event type " + msgData.type);
            }
        }

        public Vector2 GetMouseDelta()
        {
            if (lastFrame == -1) {
                lastFrame = Time.frameCount;
            }
            if (lastFrame == Time.frameCount) {
                return mouseDelta;
            } else {
                return Vector2.zero;
            }
        }
    }
}