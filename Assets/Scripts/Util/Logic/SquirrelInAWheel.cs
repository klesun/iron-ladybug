using UnityEngine;

namespace Assets.Scripts.Util.Logic
{
    /**
     * this script represents an object that is bound to
     * another object so, that it is always on top of it
     * in other words: preserves vertical distance and rotation
     */
    [ExecuteInEditMode]
    public class SquirrelInAWheel : MonoBehaviour
    {
        public Transform wheel;

        private float distance;
        private float degree;

        void Awake()
        {
            distance = transform.position.y - wheel.position.y;
            degree = transform.eulerAngles.y - wheel.eulerAngles.y;
        }

        void Renew()
        {
            transform.position = wheel.position + Vector3.up * distance;
            transform.rotation = Quaternion.Euler(0, wheel.eulerAngles.y + degree, 0);
        }

        void Update()
        {
            if (!Application.isPlaying) {
                Renew();
            }
        }

        void FixedUpdate ()
        {
            Renew();
        }
    }
}
