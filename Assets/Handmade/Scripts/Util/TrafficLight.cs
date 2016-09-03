using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class TrafficLight : IInOutTrigger 
{
	public Rainbow red;
	public Rainbow yellow;
	public Rainbow green;

	public float period = 6;

	private DCallback onIn = () => {};
	private DCallback onOut = () => {};

	void Update ()  
	{
		Darken (yellow, 0.2f);

		// red/yellow/green = 4/1/4
		var offset = 1.0f * Time.fixedTime % period;
		if (offset / period < 2.0f/6.0f) {
			if (red.color != Color.red) {
				red.color = Color.red;
				onOut ();
				Darken (green, 0.2f);
			}
		} else if (offset / period < 3.0f/6.0f || offset / period > 5.0f/6.0f) {
			yellow.color = Color.yellow;
			Darken (red, 0.2f);
			Darken (green, 0.2f);
		} else {
			if (green.color != Color.green) {
				green.color = Color.green;
				onIn ();
				Darken (red, 0.2f);
			}
		}
	}

	static void Darken(Rainbow rainbow, float lightFactor)
	{
		var newColor = new Color (
			Mathf.Min(rainbow.color.r, lightFactor),
			Mathf.Min(rainbow.color.g, lightFactor),
			Mathf.Min(rainbow.color.b, lightFactor)
		);
		if (rainbow.color != newColor) {
			rainbow.color = newColor;
		}
	}

	override public void OnIn(DCallback callback)
	{
		onIn = callback;
	}

	override public void OnOut(DCallback callback)
	{
		onOut = callback;
	}
}
