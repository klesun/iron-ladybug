using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using System.Timers;
using GameLogic;
using Interfaces;
using System.Collections;

namespace Util
{
	/** singletone for doing stuff */
	public class Tls
	{
		public readonly Timeout timeout;

		private static Tls instance;
		private GameObject dullGameObject = new GameObject ("_Timeout");
		private readonly AudioSource audioSource;
		private IHero hero;

		private Tls ()
		{
			dullGameObject.AddComponent (typeof(Timeout));
			timeout = dullGameObject.GetComponent<Timeout>();

			var audioSourceEl = new GameObject ("_staticAudio", typeof(AudioSource));
			audioSource = audioSourceEl.GetComponent<AudioSource> ();
		}

		public static Tls Inst()
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
			dullGameObject = dullGameObject ?? new GameObject ("_Timeout");
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
		public D.Cb SetGameTimeout(float seconds, D.Cb callback)
		{
			return timeout.Game (seconds, callback);
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

