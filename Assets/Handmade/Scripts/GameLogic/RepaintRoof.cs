using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class RepaintRoof : MonoBehaviour 
{
	public SpaceTrigger trigger;
	public Rainbow roof;
	public Dropdown colorOptions;

	void Start () 
	{
		trigger.callback = PromptToRepaint;
	}

	void PromptToRepaint(Collider collider)
	{
		foreach (var hero in collider.GetComponents<HeroControl>()) {
			Time.timeScale = 0;
			colorOptions.gameObject.SetActive (true);
			colorOptions.value = -1;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			colorOptions.onValueChanged.AddListener ((i) => {
				colorOptions.gameObject.SetActive(false);
				roof.color = ColorFromText(colorOptions.captionText.text);
				Time.timeScale = 1;
				Cursor.lockState = CursorLockMode.Locked;
			});
		}
	}

	Color ColorFromText(string colorName)
	{
		if (colorName.ToLower () == "red") {
			return Color.red;
		} else if (colorName.ToLower () == "green") {
			return Color.green;
		} else if (colorName.ToLower () == "blue") {
			return Color.blue;
		} else if (colorName.ToLower () == "cyan") {
			return Color.cyan;
		} else if (colorName.ToLower () == "magenta") {
			return Color.magenta;
		} else if (colorName.ToLower () == "yellow") {
			return Color.yellow;
		} else {
			return new Color (0.7f, 0.5f, 0.2f);
		}
	}
}
