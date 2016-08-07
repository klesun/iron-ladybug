using UnityEngine;
using System.Collections;

public class MeleeSystem : MonoBehaviour {

    public static int Damage = 50;
    public float Distance;
    public float MaxDistance = 2.1F;

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit)) {
                Distance = hit.distance;
                if (Distance < MaxDistance){
                    hit.transform.SendMessage("ApplyDamage", Damage, SendMessageOptions.DontRequireReceiver);
                }
            }
        }
    }

}
