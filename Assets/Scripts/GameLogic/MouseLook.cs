using UnityEngine;

public class MouseLook : MonoBehaviour 
{
	void Update ()
	{
		Rotate (Input.GetAxis("Mouse Y"));
	}

	private void Rotate(float mouseYFactor)
	{
		var degrees = mouseYFactor * 10;
		transform.localRotation *= Quaternion.AngleAxis (-degrees, Vector3.right);
		var e = transform.localRotation.eulerAngles;
		transform.localRotation = Quaternion.Euler (
			e.x < 180 ? Mathf.Min(60, e.x) : Mathf.Max(285, e.x), //Mathf.Max(-75, Mathf.Min(60, 360 + e.x)),
			e.y,
			e.z
		);
		Zoom (transform.localRotation.x);
	}

	private void Zoom(float mouseYFactor)
	{
		var zFactor = Mathf.Pow (0.5f, mouseYFactor);
		transform.localScale = new Vector3 (
			transform.localScale.x,
			transform.localScale.y,
			zFactor
		);
	}
}
