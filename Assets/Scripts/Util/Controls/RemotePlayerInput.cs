using System.Collections.Generic;
using Network;
using Interfaces;
using UnityEngine;

namespace Util.Controls {
    public class RemotePlayerInput : IPlayerInput {
        HashSet<KeyCode> pressedKeys = new HashSet<KeyCode>();
        Vector2 mouseDelta = Vector2.zero;
        int lastFrame = -1;

        public bool GetKeyDown(KeyCode key)
        {
            // TODO: make it return true only on the frame key was pressed
            return pressedKeys.Contains(key);
        }

        public bool GetKey(KeyCode key)
        {
            return pressedKeys.Contains(key);
        }

        public void HandleEvent(Msg msgData)
        {
            lastFrame = -1;
            mouseDelta = Vector2.zero;
            if (msgData.type == Msg.EType.KeyDown) {
                pressedKeys.Add(msgData.keyCode);
            } else if (msgData.type == Msg.EType.KeyUp) {
                pressedKeys.Remove(msgData.keyCode);
            } else if (msgData.type == Msg.EType.MouseMove) {
                mouseDelta = msgData.mouseDelta.toStd();
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