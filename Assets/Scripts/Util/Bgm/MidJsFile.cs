using System;
using Newtonsoft.Json;
using UnityEngine;
using Util.Midi;

namespace Util.Bgm
{
    public class MidJsFile: MonoBehaviour
    {
        public TextAsset midJsFile;

        private bool isParsed = false;
        private MidJsDefinition parsed;

        public MidJsDefinition getParsed()
        {
            if (isParsed == false) {
                isParsed = true;
                parsed = JsonConvert.DeserializeObject<MidJsDefinition>(midJsFile.text);
            }
            Console.WriteLine(parsed);
            return parsed;
       }
    }
}