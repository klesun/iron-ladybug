using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Util.Architecture
{
    public class FerrisWheel : MonoBehaviour
    {
        public float amplY = 5;
        public float amplZ = 5;
        public float initialOffset = 0; // 1 = whole lap
        // when tempo is 120, time equals the second multiplied by these guys
        public float periodNumerator = 0.5f;
        [FormerlySerializedAs("frequence")]
        public float periodDenominator = 0.125f;

        float startTime = 0;
        Vector3 startPosition;

        void Awake ()
        {
            startPosition = transform.localPosition;
        }

        /**
         * @return float - 0 if wheel is at the start,
         * 0.33 if it is at 1/3 of the way and so on
         */
        public float GetOffset()
        {
            return (initialOffset + (Bgm.Bgm.Inst().GetProgress() - startTime) * periodDenominator / periodNumerator) % 1;
        }

        void FixedUpdate ()
        {
            transform.localPosition = GetLocalPositionAt (2 * Mathf.PI * GetOffset());
        }

        void OnDrawGizmos()
        {
            var markCnt = 12;
            for (var i = 0; i < markCnt; ++i) {
                var dPos = GetLocalPositionAt (2 * Mathf.PI * i / markCnt);
                if (transform.parent != null) {
                    dPos =
                        dPos.z * transform.parent.transform.forward +
                        dPos.y * transform.parent.transform.up;
                }
                Gizmos.DrawWireSphere (transform.position + dPos, 0.1f);
            }
        }

        Vector3 GetLocalPositionAt(float radians)
        {
            var dy = Mathf.Sin(radians) * amplY;
            var dz = Mathf.Cos(radians) * amplZ;
            return startPosition + Vector3.forward * dz + Vector3.up * dy;
            //return startPosition + transform.forward * dz + transform.up * dy;
        }

        public void SetFrequence(float value)
        {
            initialOffset = GetOffset ();
            startTime = Bgm.Bgm.Inst().GetProgress();
            periodDenominator = value;
        }
    }
}
