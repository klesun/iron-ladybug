using System;
using Assets.Scripts.Util.Logic;
using Assets.Scripts.Util.Shorthands;
using Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Util.Architecture.Animated {
    /**
     * constantly move forward
     */
    public class Move: MonoBehaviour {
        public SpaceTrigger trigger = null;
        public float xStep = 0;
        public float yStep = 0;
        public float zStep = 0.1f;

        private bool started = false;
        private float startedAt = 0;
        
        private void Awake()
        {
            var createdAt = Time.fixedTime;
            U.Opt(trigger).get = 
                trigger => trigger.onIn =
                col => U.Opt(col.GetComponent<IHeroMb>()).get =
                hero => U.If(!started).then =
                () => U.If(!started).then =
                () => U.If(Time.fixedTime - createdAt > 1f).then =
                () => {
                    started = true;
                    startedAt = Time.fixedTime;
                    var copy = Object.Instantiate(gameObject, transform.position, transform.rotation);
                    copy.transform.parent = transform.parent;
                };
        }
        
        private void FixedUpdate()
        {
            if (started) {
                if (Time.fixedTime - startedAt > 120) {
                    GameObject.Destroy(gameObject);
                } else {
                    transform.position += 
                        transform.right * xStep 
                        + transform.up * yStep
                        + transform.forward * zStep
                        ;                    
                }
            }
        }
    }
}