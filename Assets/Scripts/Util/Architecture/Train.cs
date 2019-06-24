using System;
using Assets.Scripts.Interfaces;
using Assets.Scripts.Util.Logic;
using UnityEngine;

namespace Assets.Scripts.Util.Architecture {
    /**
     * moves array of game object along the trajectory
     */
    [ExecuteInEditMode]
    public class Train : MonoBehaviour
    {
        public Transform[] wagons = {};
        public Trajectory trajectory;
        [Range(0,0.5f)]
        public float spacing = 0.05f;
        [Range(0,1)]
        public float offset = 0;
        public float speed = 1f / 180;
        public TransformListener[] transformListeners = {};

#if UNITY_EDITOR
        private void OnEnable()
        {
            foreach (var listener in transformListeners) {
                Console.WriteLine("ololo listener");
                listener.onChange = () => UpdatePosition(0);
            }
        }

        void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += () => {
                if (!Application.isPlaying) {
                    UpdatePosition(0);
                }
            };
        }
#endif

        public void UpdatePosition(float progress)
        {
            for (var i = 0; i < wagons.Length; ++i) {
                var wagon = wagons[i];
                if (!wagon) {
                    // if this element was removed from scene,
                    // if it was a collected berry for example
                    continue;
                }
                var spacing = (wagons.Length - i - 1) * this.spacing;
                var wagonProgress = (progress + offset + spacing) % 1;
                trajectory.GetAt(wagonProgress).get
                = point => wagon.position = point;
                trajectory.GetAt(wagonProgress + 0.00001f).get
                = point => wagon.LookAt(point);
            }
        }

        void FixedUpdate()
        {
            UpdatePosition(speed * Time.fixedTime);
        }
    }
}
