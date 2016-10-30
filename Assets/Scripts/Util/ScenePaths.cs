using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Util
{
    /**
     * please don't change this script fields manually
     * it is supposed to be filled automagically from SceneLoader.cs
     */
    public class ScenePaths : MonoBehaviour
    {
        public List<string> scenePaths;
        /*
         * it conflicts with simulatenously opened scenes in editor
         * so it would make sense to enabled this flag only before build
         */
        public bool isEnabled = true;

        void Awake ()
        {
            if (isEnabled) {
                foreach (var path in scenePaths) {
                    SceneManager.LoadScene (path, LoadSceneMode.Additive);
                }
            }
        }
    }
}