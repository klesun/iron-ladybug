using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Timers;
using GameLogic;
using Interfaces;

namespace Util
{
	/** singletone for doing stuff */
	public class Tls
	{
		public readonly Nark mainThreadBridge;

		private static Tls instance;
		private readonly GameObject dullGameObject = new GameObject ("_nark");
		private readonly AudioSource audioSource;
		private IHero hero;

		private Tls ()
		{
			dullGameObject.AddComponent (typeof(Nark));
			mainThreadBridge = dullGameObject.GetComponent<Nark>();

			var audioSourceEl = new GameObject ("_staticAudio", typeof(AudioSource));
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

		public D.Cb Pause()
		{
			Time.timeScale = 0;
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;

			return () => {
				Time.timeScale = 1;
				Cursor.lockState = CursorLockMode.Locked;
			};
		}

		/** 
		 * @return DCallback - call it to trigger the action before timeout
		 */
		public D.Cb SetTimeout(float seconds, D.Cb callback)
		{
			var alreadyPerformed = false;
			D.Cb perform = () => {
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

		public IHero GetHero()
		{
			return hero ?? (hero = UnityEngine.Object.FindObjectOfType<IHeroMb>());
		}
	}
}

