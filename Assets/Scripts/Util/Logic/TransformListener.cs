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

		Vector3 lastPosition;
		Vector3 lastScale;
		Quaternion lastRotation;

		void Awake ()
		{
			lastPosition = transform.position;
		}

		void Update()
		{
			if (onChange != null) {
				if (lastPosition != transform.position ||
					lastScale != transform.lossyScale ||
					lastRotation != transform.rotation
				) {
					lastPosition = transform.position;
					lastScale = transform.lossyScale;
					lastRotation = transform.rotation;
					onChange();
				}
			}
		}
	}
}