using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Util.Logic;
using UnityEngine;

namespace Assets.Scripts.Util.Architecture
{
    public class FlowingStairs : MonoBehaviour
    {
        public TransformListener stairReference;
        public TransformListener endPoint;
        public TransformListener blockCont;

        public AudioClip stepSfx = null;
        public int stairCount = 5;
        public float spacing = 1;
        // defines how big part of a musical
        // tact will single step take
        public int stepMusicNumerator = 1;
        public int stepMusicDenominator = 1;

        private Transform[] stairs = new Transform[0];

        // for sfx
        private int lastSemiStepIndex = -1;

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

        static Transform PlaceStairsStateless(
            IList<Transform> stairs,
            float completion,
            Vector3 direction,
            float spacing,
            int length,
        int O_O) {
            Transform moved = null;
            if (stairs.Count == 0) {
                return moved;
            }

            int fullMovingIdx = (int) Mathf.Floor(length * completion);
            var movingIdx = fullMovingIdx % stairs.Count;
            var segmentTime = length > 0 ? 1f / length : 1;
            var dy = (direction * spacing).y;
            var dx = (direction * spacing - Vector3.up * dy).magnitude;

            for (var idx = 0; idx < stairs.Count; ++idx) {
                var stair = stairs[idx];
                var relIdx = idx - movingIdx;
                if (relIdx < 0) {
                    relIdx = stairs.Count + relIdx;
                }
                var fullIdx = fullMovingIdx + relIdx;
                var basePos = fullIdx * spacing * direction;

                if (relIdx == 0) {
                    var segmentCompletion = completion % segmentTime / segmentTime;
                    if (segmentCompletion < 0.5f) { // half
                        stair.localPosition = basePos
                            + Vector3.forward * stairs.Count * dx * segmentCompletion * 2;
                    } else {
                        stair.localPosition = basePos
                            + Vector3.forward * stairs.Count * dx
                            + Vector3.up * stairs.Count * dy * (segmentCompletion - 0.5f) * 2;
                    }
                    moved = stair;
                } else {
                    stair.localPosition = basePos;
                }
            }
            return moved;
        }

        void PlaceStairs()
        {
            var localEndPoint = blockCont.gameObject.transform.InverseTransformPoint(endPoint.transform.position);
            var length = (int) Mathf.Floor(localEndPoint.magnitude / spacing) - stairs.Length;;
            var period = length * 2 * stepMusicNumerator * 1.0f / stepMusicDenominator;
            var completeTime = Bgm.Bgm.Inst().GetProgress() / period;
            var loopedTime = completeTime % 1;

            var moved = PlaceStairsStateless(
                stairs: stairs,
                completion: loopedTime < 0.5f
                    ? loopedTime * 2
                    : 1 - (loopedTime - 0.5f) * 2,
                direction: localEndPoint.normalized,
                spacing: spacing,
                length: length,
                O_O: 0 - 0
            );

            var semiStepIndex = (int)(length * 2 * completeTime);
            if (lastSemiStepIndex != semiStepIndex) {
                lastSemiStepIndex = semiStepIndex;
                if (stepSfx != null && moved != null && Application.isPlaying) {
                    AudioSource.PlayClipAtPoint(stepSfx, moved.position);
                }
            }
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

        void OnDrawGizmos ()
        {
            if (blockCont != null && endPoint != null) {
                Vector3 startPos = blockCont.transform.position;
                Vector3 endPos = endPoint.transform.position;
                var cnt = (int)(Vector3.Distance (endPos, startPos) / spacing);
                endPos = Vector3.Lerp (startPos, endPos, cnt * spacing / Vector3.Distance(startPos, endPos));
                for (float i = 0; i < cnt; i++) {
                    var drawPos = Vector3.Lerp (startPos, endPos, i / cnt);
                    Gizmos.DrawWireSphere(drawPos, spacing / 2);
                }
            }
        }
    }
}
