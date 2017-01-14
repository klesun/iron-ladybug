using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum eOrientationMode { NODE = 0, TANGENT }

[AddComponentMenu("Splines/Spline Controller")]
[RequireComponent(typeof(SplineInterpolator))]
public class SplineController : MonoBehaviour
{
    public GameObject SplineRoot;
    public float Duration = 10;
    public eOrientationMode OrientationMode = eOrientationMode.NODE;
    public eWrapMode WrapMode = eWrapMode.ONCE;
    public bool AutoStart = true;
    public bool AutoClose = true;
    public bool HideOnExecute = true;

    SplineInterpolator mSplineInterp;
    Transform[] mTransforms;

    void OnDrawGizmos()
    {
        Transform[] trans = GetTransforms();
        if (trans.Length < 2) return;

        var interp = GetComponent<SplineInterpolator>();
        SetupSplineInterpolator(interp, trans);
        interp.StartInterpolation(null, false, WrapMode);

        var prevPos = trans[0].position;
        for (var c = 1; c <= 100; c++)
        {
            var currTime = c * Duration / 100;
            var currPos = interp.GetHermiteAtTime(currTime);
            var mag = (currPos-prevPos).magnitude * 2;
            Gizmos.color = new Color(mag, 0, 0, 1);
            Gizmos.DrawLine(prevPos, currPos);
            prevPos = currPos;
        }
    }

    void Start()
    {
        mSplineInterp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;
        mTransforms = GetTransforms();

        if (HideOnExecute) {
            DisableTransforms();
        }

        if (AutoStart) {
            FollowSpline();
        }
    }

    void SetupSplineInterpolator(SplineInterpolator interp, Transform[] trans)
    {
        interp.Reset();

        var step = AutoClose
            ? Duration / trans.Length
            : Duration / (trans.Length - 1);

        var i = -1;
        while (++i < trans.Length) {
            if (OrientationMode == eOrientationMode.NODE) {
                interp.AddPoint(trans[i].position, trans[i].rotation, step * i, new Vector2(0, 1));
            } else if (OrientationMode == eOrientationMode.TANGENT) {
                Quaternion rot;
                if (i != trans.Length - 1) {
                    rot = Quaternion.LookRotation(trans[i + 1].position - trans[i].position, trans[i].up);
                } else if (AutoClose) {
                    rot = Quaternion.LookRotation(trans[0].position - trans[i].position, trans[i].up);
                } else {
                    rot = trans[i].rotation;
                }

                interp.AddPoint(trans[i].position, rot, step * i, new Vector2(0, 1));
            }
        }

        if (AutoClose) {
            interp.SetAutoCloseMode(step * i);
        }
    }


    // Returns children transforms, sorted by name.
    Transform[] GetTransforms()
    {
        if (SplineRoot != null) {
            var transforms = SplineRoot.GetComponentsInChildren<Transform>().ToList();
            transforms.Remove(SplineRoot.transform);
            transforms.Sort((a, b) => a.name.CompareTo(b.name));

            return transforms.ToArray();
        } else {
            return new Transform[] { };
        }
    }

    // Disables the spline objects, we don't need them outside design-time.
    void DisableTransforms()
    {
        if (SplineRoot != null)
        {
            SplineRoot.SetActiveRecursively(false);
        }
    }


    // Starts the interpolation
    void FollowSpline()
    {
        if (mTransforms.Length > 0)
        {
            SetupSplineInterpolator(mSplineInterp, mTransforms);
            mSplineInterp.StartInterpolation(null, true, WrapMode);
        }
    }
}