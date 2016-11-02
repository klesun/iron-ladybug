using Interfaces;
using UnityEngine;
using Util;

namespace Assets.Scripts.Util.Architecture
{
    /**
     * may become standable or not standable depending
     * on color of the assigned traffic light
     * last block hero stood on during green light stays standable,
     * whereas all rest disappear when red light is on
     */
    public class TrafficLightBlock : MonoBehaviour
    {
        public TrafficLight trafficLight;
        public Rainbow colorController;

        void Update ()
        {
            colorController.color = trafficLight.GetColor(this);
        }

        void OnCollisionEnter(Collision col)
        {
            var maybeNpc = col.gameObject.GetComponent<INpc>();
            if (maybeNpc != null) {
                trafficLight.ReportInteraction(this, maybeNpc);
            }
        }

        void OnCollisionExit(Collision col)
        {
            var maybeNpc = col.gameObject.GetComponent<INpc>();
            if (maybeNpc != null) {
                trafficLight.ReportOutteraction(maybeNpc);
            }
        }
    }
}
