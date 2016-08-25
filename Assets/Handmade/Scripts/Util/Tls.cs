using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace AssemblyCSharp
{
	/** singletone for doing stuff */
	public class Tls : MonoBehaviour
	{
		private static Tls instance;
		private readonly new GameObject  gameObject = new GameObject ();
		private readonly Dropdown dropdown;

		private Tls ()
		{
			var canvasEl = new GameObject ("huj", typeof(Canvas), typeof(GraphicRaycaster));
			var dropdownEl = (GameObject)Instantiate (Resources.Load("Dropdown"));
			dropdown = dropdownEl.GetComponent<Dropdown> ();
			dropdown .options.Add (new Dropdown.OptionData("zalupa"));
			dropdown .options.Add (new Dropdown.OptionData("dzigurda"));
			dropdown .options.Add (new Dropdown.OptionData("pidor"));
			dropdown .options.Add (new Dropdown.OptionData("lox"));
			dropdownEl.transform.SetParent(canvasEl.transform);
			canvasEl.GetComponent<Canvas> ().renderMode = RenderMode.ScreenSpaceOverlay;
		}

		public static Tls inst()
		{
			return instance 
				?? (instance = new Tls());
		}

		/* 
		 * get transform of a fake game object for access to 
		 * methods like "lookAt" to get rotation between two dots 
		*/
		public Transform DullTransform(Vector3 pos)
		{
			gameObject.transform.position = pos;
			gameObject.transform.rotation = Quaternion.Euler (new Vector3(0,0,0));
			return gameObject.transform;
		}



		public void AskForChoice<T>(IDictionary<string, T> options, DMono<T> cb)
		{
			dropdown.options.Clear ();
			dropdown.options.Add (new Dropdown.OptionData("none"));
			foreach (var key in options.Keys) {
				dropdown.options.Add (new Dropdown.OptionData(key));
			}

			Time.timeScale = 0;
			dropdown.gameObject.SetActive (true);
			var tmp = dropdown.onValueChanged;
			dropdown.onValueChanged = new Dropdown.DropdownEvent ();
			dropdown.value = 0;
			dropdown.onValueChanged = tmp;
			dropdown.Hide ();
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
			// TODO: i likely should remove it afterwards amn't i?
			dropdown.onValueChanged.AddListener ((i) => {
				dropdown.gameObject.SetActive(false);
				Time.timeScale = 1;
				Cursor.lockState = CursorLockMode.Locked;
				cb(options[dropdown.captionText.text]);
			});

		}
	}
}

