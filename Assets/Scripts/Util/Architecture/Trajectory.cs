using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Util.Misc;
using UnityEngine;
using System.Linq;
using Assets.Scripts.Util.Shorthands;
using Util.Shorthands;

namespace Assets.Scripts.Util.Architecture {
    public enum ETimingMethod {
        CONSTANT_TIME, // time between two points is constant
        STRAIGHT_SPEED, // TODO: implement: treat two point curve as straight
                // time between two points is directly proportional to it's length
        SECTOR_DIVISION, // TODO: implement: split each point pair 20 or
                        // so divisions and calculate progress from their lengths
    };

    /**
     * moves object alongside key
     * points through hermite curves
     */
    [ExecuteInEditMode]
    public class Trajectory : MonoBehaviour {
        public Transform loco;
        [Tooltip("Defines how two point placement affects time")]
        public ETimingMethod timingMethod = ETimingMethod.CONSTANT_TIME;
        [Tooltip("Roundness")]
        [Range(0, 2f)]
        public float tension = 0.5f;

        void Update()
        {
            var keyPointTs = CollectPoints(transform).ToArray();
            if (keyPointTs.Any(p => p.hasChanged)) {
                var points = keyPointTs.Select(p => p.position).ToArray();
                Spline.N(points, tension).get = spline => {
                    S.L(keyPointTs).each = (point, i) => {
                        var lookPoint = spline.GetAt(i * 1.0f / keyPointTs.Length + 0.00001f);
                        point.LookAt(new Vector3(
                            lookPoint.x, point.position.y, lookPoint.z
                        ));
                        point.hasChanged = false;
                    };
                };
            }
        }

        static IEnumerable<Transform> CollectPoints(Transform cont)
        {
            foreach (Transform point in cont) {
                yield return point;
            }
        }

        void OnDrawGizmos()
        {
            var detalisation = 2000;
            var points = CollectPoints(transform).Select(p => p.position).ToArray();
            Spline.N(points, tension).get = spline => {
                for (var j = 0; j < detalisation; ++j) {
                    var p0 = spline.GetAt(j * 1.0f / detalisation);
                    var p1 = spline.GetAt((j + 1) * 1.0f / detalisation);
                    Gizmos.color = new Color(0, 1f, 1f, 1);
                    Gizmos.DrawLine(p0, p1);
                }
            };
        }

        public Opt<Vector3> GetAt(float progress)
        {
            // TODO: preserve points in instance?
            var points = CollectPoints(transform)
                .Select(p => p.position).ToArray();
            return Spline.N(points, tension)
                .Map(spline => spline.GetAt(progress));
        }
    }
}
