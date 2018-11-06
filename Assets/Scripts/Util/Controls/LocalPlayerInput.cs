using Interfaces;
using UnityEngine;

namespace Util.Controls {
    public class LocalPlayerInput : IPlayerInput {

        public bool GetKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        public bool GetKey(KeyCode key)
        {
            return Input.GetKey(key);
        }

        public Vector2 GetMouseDelta()
        {
            return new Vector2(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")
            );
        }
    }
}