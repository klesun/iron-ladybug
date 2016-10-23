using UnityEngine;
using System.Collections;
using AssemblyCSharp;

namespace Util
{
	/** 
	 * provides ability to hang a callback on it's position change
	*/
	[ExecuteInEditMode]
	public class TransformListener : MonoBehaviour 
	{
		public D.Cb onChange = null;
		public TransformListener parent = null;

		Vector3 lastPosition;
		Vector3 lastScale;
		Quaternion lastRotation;

		void Awake ()
		{
			lastPosition = transform.position;
		}

		void Update()
		{
			if (lastPosition != transform.position ||
				lastScale != transform.lossyScale ||
				lastRotation != transform.rotation
			) {
				lastPosition = transform.position;
				lastScale = transform.lossyScale;
				lastRotation = transform.rotation;

				OnValidate ();
			}
		}

		#if UNITY_EDITOR
		void OnValidate() 
		{
			if (onChange != null) {
				UnityEditor.EditorApplication.delayCall += () => onChange();
			}
			if (parent != null) {
				parent.OnValidate ();
			}
		}
		#endif
	}
}