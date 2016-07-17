using UnityEngine;
using System.Collections;

public class flashOnOff : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F)) {
            GetComponent<Light>().enabled = !GetComponent<Light>().enabled;
        }
   }
}
