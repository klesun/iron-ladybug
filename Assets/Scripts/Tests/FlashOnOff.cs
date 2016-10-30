using UnityEngine;
using System.Collections;

public class FlashOnOff : MonoBehaviour {

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.F)) {
            GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
        }
   }
}
