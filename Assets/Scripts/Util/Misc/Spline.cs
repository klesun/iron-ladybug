using Assets.Scripts.Util.Shorthands;
using UnityEngine;
using Util.Shorthands;

namespace Assets.Scripts.Util.Misc
{
    /**
     * provides ability to define a curve with array
     * of points and move an object alongside this curve
     */
    public class Spline
    {
        private readonly Vector3[] keyPoints;
        private readonly float tension = 0.5f;

        private Spline(Vector3[] keyPoints, float tension)
        {
            this.keyPoints = keyPoints;
            this.tension = tension;
        }

        public static Opt<Spline> N(Vector3[] keyPoints, float tension)
        {
            return keyPoints.Length > 0
                ? S.Opt(new Spline(keyPoints, tension))
                : S.Opt<Spline>();
        }

        private static Vector3 GetAtS(float mu, float tension, T4<Vector3> points)
        {
            var t = mu;
            var t2 = t * t;
            var t3 = t2 * t;

            var P0 = points.a;
            var P1 = points.b;
            var P2 = points.c;
            var P3 = points.d;

            var T1 = tension * (P2 - P0);
            var T2 = tension * (P3 - P1);

            var Blend1 = 2 * t3 - 3 * t2 + 1;
            var Blend2 = -2 * t3 + 3 * t2;
            var Blend3 = t3 - 2 * t2 + t;
            var Blend4 = t3 - t2;

            return Blend1 * P1 + Blend2 * P2 + Blend3 * T1 + Blend4 * T2;
        }

        private Vector3 AtLim(int n)
        {
            return keyPoints[Mathf.Min(keyPoints.Length - 1, Mathf.Max(0, n))];
        }

        /**
         * @param t - progress, value in range [0..1)
         */
        public Vector3 GetAt(float progress)
        {
            progress = Mathf.Max(0, progress);
            progress = Mathf.Min(1, progress);
            progress = progress * (keyPoints.Length - 1);
            var left = (int)progress;
            var mu = progress % 1;

            return GetAtS(mu, tension, S.T4(
                AtLim(left - 1),
                AtLim(left),
                AtLim(left + 1),
                AtLim(left + 2)
            ));
        }
    }
}