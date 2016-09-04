using System;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AssemblyCSharp
{
	/** retrieves sample file and parameters from soundfont (pitch/loopTimes/attenuation/stuff) */
	public class Adapter
	{
		private static string samplesDir = "Dropbox/fluid/samples";
		private static JsonDefinition soundFontInfo = ReadJson (Application.dataPath + "/Resources/Dropbox/fluid/presets.pretty.json");

		private static Dictionary<string, AudioClip> samplesByName = new Dictionary<string, AudioClip>();

		public readonly AudioClip audioClip;
		public readonly float frequencyFactor;
		public readonly float volumeFactor;
		public readonly float loopStart;
		public readonly float loopEnd;

		private Adapter(int semitone, int presetN)
		{
			var presetInfo = soundFontInfo.presets[presetN];
			var sampleInfo = FindClosestSample (semitone, presetInfo.instrument.samples);

			var generator = CombineGenerators (
				OverrideGenerator(presetInfo.generatorApplyToAll, presetInfo.instrument.generator),
				OverrideGenerator(presetInfo.instrument.generatorApplyToAll, sampleInfo.generator)
			);

			var sampleSemitone = generator.overridingRootKey ?? sampleInfo.originalPitch;
			var correctionCents = DetermineCorrectionCents (semitone - sampleSemitone, generator);

			frequencyFactor = Mathf.Pow (2, correctionCents / 1200.0f);
			loopStart = 1.0f * (sampleInfo.startLoop + (generator.startloopAddrsOffset ?? 0)) / sampleInfo.sampleRate;
			loopEnd = 1.0f * (sampleInfo.endLoop + (generator.endloopAddrsOffset ?? 0)) / sampleInfo.sampleRate;
			audioClip = samplesByName.ContainsKey(sampleInfo.sampleName)
				? samplesByName[sampleInfo.sampleName]
				: samplesByName[sampleInfo.sampleName] = GetSample(sampleInfo.sampleName);
		}

		public static Adapter Get(int semitone, int presetN)
		{
			return new Adapter(semitone, presetN);
		}

		static JsonDefinition ReadJson(string path)
		{
			string text;
			using (StreamReader sr = new StreamReader(path))
			{
				text = sr.ReadToEnd();
			}

			return JsonConvert.DeserializeObject<JsonDefinition> ("{\"presets\": " + text + "}");
		}

		static SampleInfo FindClosestSample(int semitone, SampleInfo[] options)
		{
			var minDistance = 9999;
			SampleInfo result = null;

			foreach (var opt in options) {
				if (opt.generator.keyRange != null) {
					var distance = Mathf.Min (
						minDistance, 
						Mathf.Abs(opt.generator.keyRange.lo - semitone),
						Mathf.Abs(opt.generator.keyRange.hi - semitone)
					);
					if (distance < minDistance) {
						minDistance = distance;
						result = opt;
					}
				} else {
					return opt;
				}
			}

			return result;
		}

		static Generator CombineGenerators (Generator globalGen, Generator localGen)
		{
			Generator result;
			using (var ms = new MemoryStream())
			{
				var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
				formatter.Serialize(ms, localGen);
				ms.Position = 0;

				result = (Generator) formatter.Deserialize(ms);
			}

			result.fineTune = (result.fineTune ?? 0) + (globalGen.fineTune ?? 0);
			result.coarseTune = (result.coarseTune ?? 0) + (globalGen.coarseTune ?? 0);
			result.initialAttenuation = (result.initialAttenuation ?? 0) + (globalGen.initialAttenuation ?? 0);

			return result;
		}

		static Generator OverrideGenerator(Generator globalGen, Generator localGen)
		{
			Generator result = new Generator();

			result.keyRange = localGen.keyRange ?? globalGen.keyRange;
			result.overridingRootKey = localGen.overridingRootKey ?? globalGen.overridingRootKey;
			result.fineTune = localGen.fineTune ?? globalGen.fineTune;
			result.coarseTune = localGen.coarseTune ?? globalGen.coarseTune;
			result.startloopAddrsOffset = localGen.startloopAddrsOffset ?? globalGen.startloopAddrsOffset;
			result.endloopAddrsOffset = localGen.endloopAddrsOffset ?? globalGen.endloopAddrsOffset; 
			result.initialAttenuation = localGen.initialAttenuation ?? globalGen.initialAttenuation;
			result.initialFilterQ = localGen.initialFilterQ ?? globalGen.initialFilterQ;
			result.initialFilterFc = localGen.initialFilterFc ?? globalGen.initialFilterFc;
			result.sampleModes = localGen.sampleModes ?? globalGen.sampleModes;

			return result;
		}

		static float DetermineCorrectionCents(int delta, Generator generator)
		{
			var result = delta * 100;

			result += generator.fineTune ?? 0;
			result += (generator.coarseTune ?? 0) * 100;

			return result;
		}

		static AudioClip GetSample(string fileName)
		{
			var path = samplesDir + "/" + fileName;
			var clip = Resources.Load<AudioClip> (path);
			if (clip == null) {
				throw new Exception("no such file in resources: " + path);
			}

			return clip;
		}
	}
}

