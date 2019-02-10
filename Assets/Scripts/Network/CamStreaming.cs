using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Network {

	public class CamStreaming : MonoBehaviour {
		public RenderTexture render;
		private Texture2D tex2d;

		private void Start()
		{
			tex2d = new Texture2D(render.width, render.height, TextureFormat.RGB24, false);
		}

		public byte[] GetFrameImgBytes()
		{
			RenderTexture.active = render;
			tex2d.ReadPixels(new Rect(0, 0, render.width, render.height), 0, 0);
			tex2d.Apply();

			// it would probably be much more efficient to encode the video with H.264 at some point...
			return tex2d.EncodeToJPG();
			//File.WriteAllBytes(Application.dataPath + "/zhopa.png", bytes);
		}
	}
}

