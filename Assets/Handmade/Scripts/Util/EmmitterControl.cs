using UnityEngine;
using System.Collections;

/** 
 * this class provides control over an array of particle emmitters
 * it allows to start and end emmission whenever you want
 */
public class EmmitterControl : MonoBehaviour 
{
	public ParticleSystem[] emmitters;

	// Use this for initialization
	void Start () 
	{
		StopEmission ();
	}

	public void Emmit()
	{
		foreach (var e in emmitters) {
			e.enableEmission = true;
		}
		Invoke ("StopEmission", 1f);
	}

	private void StopEmission()
	{
		foreach (var e in emmitters) {
			e.enableEmission = false;
		}
	}
}
