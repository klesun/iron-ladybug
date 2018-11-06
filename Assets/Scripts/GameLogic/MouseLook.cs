using UnityEngine;
using Util;

public class MouseLook : MonoBehaviour 
{
    public float minAngle = -75;
    public float maxAngle = +60;
    public float sensitivity = 0.0005f;

    public void Rotate(float yAxis)
    {
        var mouseYFactor = yAxis * sensitivity;
        var e = transform.rotation.eulerAngles;
        var xRot = e.x - mouseYFactor * 10;
        xRot = xRot > 180 ? xRot - 360 : xRot;
        xRot = Mathf.Min (maxAngle, Mathf.Max(minAngle, xRot));
        transform.rotation = Quaternion.Euler (xRot, e.y, e.z);
        Zoom (xRot);
    }

    private void Zoom(float angle)
    {
        var zFactor = Mathf.Pow (2f, angle / 60f);
        transform.localScale = new Vector3 (
            transform.localScale.x,
            transform.localScale.y,
            zFactor
        );
    }
}
