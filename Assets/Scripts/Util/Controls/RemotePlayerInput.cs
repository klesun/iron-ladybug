using System.Collections.Generic;
using Network;
using Interfaces;
using UnityEngine;

namespace Util.Controls {
    public class RemotePlayerInput : IPlayerInput {
        HashSet<KeyCode> pressedKeys = new HashSet<KeyCode>();

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
            if (msgData.type == Msg.EType.KeyDown) {
                pressedKeys.Add(msgData.keyCode);
            } else if (msgData.type == Msg.EType.KeyUp) {
                pressedKeys.Remove(msgData.keyCode);
            }
        }
    }
}