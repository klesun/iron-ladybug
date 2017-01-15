using Assets.Scripts.Interfaces;
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


#if UNITY_EDITOR
        void OnValidate()
        {
            UnityEditor.EditorApplication.delayCall += () => {
                if (!Application.isPlaying) {
                    UpdatePosition(0);
                }
            };
        }
#endif

        void UpdatePosition(float progress)
        {
            for (var i = 0; i < wagons.Length; ++i) {
                var spacing = (wagons.Length - i - 1) * this.spacing;
                var wagonProgress = (progress + offset + spacing) % 1;
                var wagon = wagons[i];
                trajectory.GetAt(wagonProgress).get
                = point => wagon.position = point;
                trajectory.GetAt(wagonProgress + 0.00001f).get
                = point => wagon.LookAt(point);
            }
        }

        void FixedUpdate()
        {
            UpdatePosition(Time.fixedTime / 180);
        }
    }
}
