using Assets.Scripts.Interfaces;
using UnityEngine;
using Util;

namespace Assets.Scripts.Util.Logic
{
    /**
     * provides ability to hang a callback on it's position change
     */
    [ExecuteInEditMode]
    public class TransformListener : MonoBehaviour, IValidator
    {
        public D.Cb onChange = null;
        public TransformListener parent = null;

        Vector3 lastPosition;
        Vector3 lastScale;
        Quaternion lastRotation;

        void Update()
        {
            if (lastPosition != transform.localPosition ||
                lastScale != transform.localScale ||
                lastRotation != transform.localRotation
            ) {
                lastPosition = transform.localPosition;
                lastScale = transform.localScale;
                lastRotation = transform.localRotation;

                OnValidate ();
            }
        }

        public void OnValidate()
        {
#if UNITY_EDITOR
            if (onChange != null) {
                UnityEditor.EditorApplication.delayCall += () => onChange();
            }
#endif
            if (parent != null) {
                parent.OnValidate ();
            }
        }
    }
}