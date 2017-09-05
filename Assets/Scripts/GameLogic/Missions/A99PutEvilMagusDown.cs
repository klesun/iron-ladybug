using System;
using System.Linq;
using Assets.Scripts.Util.Architecture;
using Assets.Scripts.Util.Bgm;
using Assets.Scripts.Util.Shorthands;
using UnityEngine;
using Util;
using Util.Bgm;

namespace Assets.Scripts.GameLogic.Missions
{
    /** 
     * the idea is simple - you have to step on few platforms 
     * to open path to the magus that throws some missiles at you
     * the nice part is that the more you stand in one place, the more dangerous 
     * these missiles become whereas you have to stand on a platform for some time 
     */
    public class A99PutEvilMagusDown: MonoBehaviour
    {
        public StayStillButton[] platforms;

        // optional
        public MidJsFile successJingle = null;
        
        private bool done = false;
        
        void Update ()
        {
            if (done) {
                return;
            } else if (platforms.All(p => p.wasPressed)) {
                done = true;
                U.Opt(successJingle).Map(f => f.getParsed())
                    .get = jingle => Bgm.Inst().SetBgm(jingle);
            }
        }
    }
}