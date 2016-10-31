using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using Assets.Scripts.Util.Architecture;

/** 
 * this script generates sequence of flying 
 * platforms moving in random directions with 
 * configurable distance and max amplietude
 */
public class ShardedBridge : MonoBehaviour 
{
    public Transform endPoint;
    public GameObject shardRef;

    public int lineCount = 1;
    public float lineSpacing = 1;
    public float spacing = 1;
    public float minAmplietudeX = 1;
    public float maxAmplietudeX = 4;
    public float minAmplietudeY = 1;
    public float maxAmplietudeY = 4;
    public float minPeriod = 2;
    public float maxPeriod = 4;
    public int randomSeed = 13;

    delegate void DCreateObj (Vector3 pos, Quaternion rot, float amplX, float amplY, float period);

    void Awake ()
    {

        PlaceShards ((pos, rot, amplX, amplY, period) => {
            var shard = (GameObject)UnityEngine.GameObject.Instantiate(shardRef, pos, rot);
            var ferrisWheel = (FerrisWheel)shard.AddComponent(typeof(FerrisWheel));
            ferrisWheel.amplZ = amplX;
            ferrisWheel.amplY = amplY;
            ferrisWheel.periodDenominator = period > 0 ? 1 / period : 0;
        });
    }

    void OnDrawGizmos () {
        PlaceShards ((pos, rot, amplX, amplY, period) => {
            Gizmos.DrawWireCube (pos, new Vector3 (1,1,1));
        });
    }

    void Update ()
    {
    }

    void PlaceShards (DCreateObj makeObj)
    {
        var random = new System.Random (randomSeed);

        for (float j = 0; j < lineCount; ++j) {
            Vector3 startPos = transform.position + j * transform.right * lineSpacing;
            Vector3 endPos = endPoint.position + j * transform.right * lineSpacing;

            int objc = (int)(Vector3.Distance (endPos, startPos) / spacing);
            for (float i = 0; i < objc; i++) {
                var drawPos = Vector3.Lerp (startPos, endPos, i / objc);
                var drawRot = Quaternion.Euler (0, 360 * (float)random.NextDouble(), 0);
                var amplX = minAmplietudeX + (maxAmplietudeX - minAmplietudeX) * random.NextDouble ();
                var amplY = minAmplietudeY + (maxAmplietudeY - minAmplietudeY) * random.NextDouble ();
                var period = minPeriod + (maxPeriod - minPeriod) * random.NextDouble ();
                makeObj (drawPos, drawRot, (float)amplX, (float)amplY, (float)period);
            }
        }
    }
}
