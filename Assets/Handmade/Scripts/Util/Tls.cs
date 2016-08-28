using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Timers;

namespace AssemblyCSharp
{
	/** singletone for doing stuff */
	public class Tls : MonoBehaviour
	{
		private static Tls instance;
		private readonly new GameObject  gameObject = new GameObject ();
		private readonly Dropdown dropdown;
		public readonly Nark mainThreadBridge;

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

			gameObject.AddComponent (typeof(Nark));
			mainThreadBridge = gameObject.GetComponent<Nark>();
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

		public DCallback Pause()
		{
			Time.timeScale = 0;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			return () => {
				Time.timeScale = 1;
				Cursor.lockState = CursorLockMode.Locked;
			};
		}

		public void AskForChoice<T>(IDictionary<string, T> options, DMono<T> cb)
		{
			var unpause = Pause ();
			dropdown.options.Clear ();
			dropdown.options.Add (new Dropdown.OptionData("none"));
			foreach (var key in options.Keys) {
				dropdown.options.Add (new Dropdown.OptionData(key));
			}

			dropdown.gameObject.SetActive (true);
			var tmp = dropdown.onValueChanged;
			dropdown.value = 0;
			dropdown.onValueChanged = tmp;
			dropdown.Hide ();

			dropdown.onValueChanged.AddListener ((i) => {
				dropdown.gameObject.SetActive(false);
				unpause();
				cb(options[dropdown.captionText.text]);
				dropdown.onValueChanged.RemoveAllListeners();
			});
		}

		/** 
		 * @return DCallback - call it to trigger the action before timeout
		 */
		public DCallback SetTimeout(float seconds, DCallback callback)
		{
			var alreadyPerformed = false;
			DCallback perform = () => {
				if (!alreadyPerformed) {
					mainThreadBridge.RunInMainThread (callback);
					alreadyPerformed = true;
				}
			};

			if (seconds > 0) {
				Timer timer = new Timer (seconds * 1000);
				timer.AutoReset = false;
				timer.Enabled = true;
				timer.Elapsed += new ElapsedEventHandler ((_, __) => perform());
			} else {
				perform();
			}

			return perform;
		}

		public bool IsPaused()
		{
			return Time.timeScale == 0;
		}
	}
}

