using UnityEngine;

public class MouseLook : MonoBehaviour 
{
	float mouseYFactor = 0;

	void Update ()
	{
		mouseYFactor += Input.GetAxis("Mouse Y");
		float maxY = 10.0f, minY = -10.0f;
		var limitedMouseYFactor = Mathf.Min(maxY * 0.75f, Mathf.Max (minY, mouseYFactor)) / maxY;
		Rotate (limitedMouseYFactor);
		Zoom (limitedMouseYFactor);
	}

	private void Rotate(float mouseYFactor)
	{
		var degrees = mouseYFactor * 85;
		transform.localRotation = Quaternion.AngleAxis (degrees, -Vector3.right);
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

    public static void limitAngle(Quaternion source, float maxDx, float maxDy, float maxDz)
    {
        source.x = Mathf.Max(source.x, -maxDx);
        source.x = Mathf.Min(source.x, +maxDx);
        source.y = Mathf.Max(source.y, -maxDy);
        source.y = Mathf.Min(source.y, +maxDy);
        source.z = Mathf.Max(source.z, -maxDz);
        source.z = Mathf.Min(source.z, +maxDz);
        source.w = 1.2F;
    }
}
