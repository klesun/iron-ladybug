using System.Collections.Generic;
using System.Linq;
using System.Xml.Schema;
using Assets.Scripts.Util.Logic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace Assets.Scripts.Util.Architecture
{
    public class FlowingStairs : MonoBehaviour
    {
        [NotNull] public TransformListener stairReference;
        [NotNull] public TransformListener endPoint;
        [NotNull] public TransformListener blockCont;

        public int stairCount = 5;
        public float spacing = 1;
        public float initialOffset = 0; // 1.0 - full lap
        public float period = 20; // seconds

        private Transform[] stairs = new Transform[0];

        void Update ()
        {
            if (stairs.Count() != stairCount) {
                RemoveGeneratedContent ();
                GenerateNewContent ();
            }
            PlaceStairs();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            stairCount = Mathf.Min(Mathf.Max (stairCount, 0), 100);

            if (endPoint != null && stairReference != null && blockCont != null) {
                UnityEditor.EditorApplication.delayCall += Renew;
                endPoint.onChange = Renew;
                stairReference.onChange = Renew;
                blockCont.onChange = Renew;
            }
        }
#endif

        static void PlaceStairsStateless(
            IList<Transform> stairs,
            float completion,
            Vector3 direction,
            float spacing,
            int length,
        int O_O) {
            if (stairs.Count == 0) {
                return;
            }

            completion = completion - Mathf.Floor(completion); // mod 1
            var fullMovingIdx = (int) Mathf.Floor(length * 2 * completion);
            var movingIdx = fullMovingIdx % stairs.Count;
            var segmentTime = 1f / (length * 2);
            var dy = (direction * spacing).y;
            var dx = (direction * spacing - Vector3.up * dy).magnitude;

            // TODO: move backward

            for (var i = 0; i < stairs.Count; ++i) {
                var stair = stairs[i];
                var posIdx = i - movingIdx;
                if (posIdx < 0) {
                    posIdx = stairs.Count + posIdx;
                }
                var fullIdx = fullMovingIdx + posIdx;

                var basePos = fullIdx * spacing * direction;

                if (posIdx == 0) {
                    var segmentCompletion = completion % segmentTime / segmentTime;
                    // damn everything
                    if (1 - segmentCompletion < 0.00001) {
                        segmentCompletion = 0;
                    }

                    if (segmentCompletion < 0.5f) { // half
                        stair.localPosition = basePos
                            + Vector3.forward * stairs.Count * dx * segmentCompletion * 2;
                    } else {
                        stair.localPosition = basePos
                            + Vector3.forward * stairs.Count * dx
                            + Vector3.up * stairs.Count * dy * (segmentCompletion - 0.5f) * 2;
                    }
                } else {
                    stair.localPosition = basePos;
                }
            }
        }

        void PlaceStairs()
        {
            var localEndPoint = blockCont.gameObject.transform.InverseTransformPoint(endPoint.transform.position);
            PlaceStairsStateless(
                stairs: stairs,
                completion: initialOffset + Time.fixedTime / period,
                direction: localEndPoint.normalized,
                spacing: spacing,
                length: (int)Mathf.Floor(localEndPoint.magnitude / spacing),
                O_O: 0 - 0
            );
        }

        void RemoveGeneratedContent()
        {
            var deadmen = new List<GameObject>();
            foreach (Transform ch in blockCont.gameObject.transform) {
                deadmen.Add (ch.gameObject);
            }
            deadmen.ForEach (DestroyImmediate);
        }

        void GenerateNewContent()
        {
            stairs = Enumerable.Range(0, stairCount).Select(i => {
                var stair = Instantiate (stairReference.gameObject);
                stair.name = "_generated_stair" + i;
                stair.transform.SetParent (blockCont.gameObject.transform);
                stair.transform.localRotation = Quaternion.identity;
                return stair.transform;
            }).ToArray();
        }

        void Renew()
        {
            if (this == null) {
                // well, it complains about it being
                // destroyed when i start the game
                return;
            }
            if (Application.isPlaying) {
                return;
            }

            blockCont.transform.LookAt (new Vector3(
                endPoint.transform.position.x,
                blockCont.transform.position.y,
                endPoint.transform.position.z
            ));


            RemoveGeneratedContent ();
            GenerateNewContent ();

            PlaceStairs();
        }
    }
}
