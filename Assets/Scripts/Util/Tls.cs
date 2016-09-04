using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Timers;

namespace AssemblyCSharp
{
	/** singletone for doing stuff */
	public class Tls
	{
		public readonly Nark mainThreadBridge;

		private static Tls instance;
		private readonly GameObject dullGameObject = new GameObject ();
		private readonly Dropdown dropdown;
		private readonly AudioSource audioSource;

		private Tls ()
		{
			var canvasEl = new GameObject ("staticCanvas", typeof(Canvas), typeof(GraphicRaycaster));
			var dropdownEl = (GameObject)GameObject.Instantiate (Resources.Load("Dropdown"));
			dropdown = dropdownEl.GetComponent<Dropdown> ();
			dropdownEl.transform.SetParent(canvasEl.transform);
			canvasEl.GetComponent<Canvas> ().renderMode = RenderMode.ScreenSpaceOverlay;

			dullGameObject.AddComponent (typeof(Nark));
			mainThreadBridge = dullGameObject.GetComponent<Nark>();

			var audioSourceEl = new GameObject ("staticAudio", typeof(AudioSource));
			audioSource = audioSourceEl.GetComponent<AudioSource> ();
		}

		public static Tls inst()
		{
			return instance 
				?? (instance = new Tls());
		}

		public void PlayAudio(AudioClip audio)
		{
			audioSource.PlayOneShot (audio);
		}

		/* 
		 * get transform of a fake game object for access to 
		 * methods like "lookAt" to get rotation between two dots 
		*/
		public Transform DullTransform(Vector3 pos)
		{
			dullGameObject.transform.position = pos;
			dullGameObject.transform.rotation = Quaternion.Euler (new Vector3(0,0,0));
			return dullGameObject.transform;
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

