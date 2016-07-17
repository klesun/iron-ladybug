using UnityEngine;
using System.Collections;


/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation


/// To make an FPS style character:
/// - Create a capsule.
/// - Add a rigid body to the capsule
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSWalker script to the capsule


/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class MouseLook : MonoBehaviour {

	public float sensitivityX = 15F;
	public float sensitivityY = 15F;
	public float minimumX = -360F;
	public float maximumX = 360F;
	public float minimumY = -60F;
	public float maximumY = 60F;
	float rotationX = 0F;
	float rotationY = 0F;
	Quaternion originalRotation;
	void Update ()
	{
		// Read the mouse input axis
		rotationX += Input.GetAxis("Mouse X") * sensitivityX;
		rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
		rotationX = ClampAngle (rotationX, minimumX, maximumX);
		rotationY = ClampAngle (rotationY, minimumY, maximumY);
		Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
		Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, -Vector3.right);

		var newRot = originalRotation * xQuaternion * yQuaternion;
        limitAngle(newRot, 0.2f, 0.3f, 0.2f);

        transform.localRotation = newRot;
	}
	void Start ()
	{
		originalRotation = transform.localRotation;
	}
	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
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
