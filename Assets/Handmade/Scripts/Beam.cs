using UnityEngine;
using System.Collections;

public class Beam : MonoBehaviour 
{
	public Transform endPoint;
	public LineRenderer beamLine;

	void Update () 
	{
		beamLine.SetWidth(0.2f, 0.2f);
		beamLine.SetPosition (0, transform.position);
		beamLine.SetPosition (1, endPoint.position);
	}
}
