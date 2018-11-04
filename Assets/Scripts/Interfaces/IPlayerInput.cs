using UnityEngine;

namespace Interfaces {
    /**
     * there will be 2 implementations: local player, through UnityEngine.Input
     * class and remote player sending pressed keys via sockets
     */
    public interface IPlayerInput {
        /** key is down */
        bool GetKeyDown(KeyCode key);
        /** key was pressed in this frame */
        bool GetKey(KeyCode key);
    }
}