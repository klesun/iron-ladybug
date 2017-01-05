using UnityEngine;
using System.Collections;
using AssemblyCSharp;
using Assets.Scripts.Util.Logic;
using Interfaces;

public class Blade : MonoBehaviour 
{
    public delegate void DOnClash();

    public DOnClash onClash;
    public AudioClip bladeClashSound;
    public SpaceTrigger epeeTip;
    public bool isParrying = false;

    bool isDisarmed = false;
    float lastClashedOn;

    const float CLASH_DURATION = 1;

    void Awake ()
    {
        epeeTip.OnIn(OnPierce);
        lastClashedOn = Time.fixedTime;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (!isDisarmed) {
            foreach (var trigger in collider.gameObject.GetComponents<Blade>()) {
                AudioSource.PlayClipAtPoint(bladeClashSound, transform.position);
                if (!isParrying || trigger.isParrying) {
                    lastClashedOn = Time.fixedTime;
                    onClash();
                }
            }
        }
    }

    void OnPierce(Collider prey)
    {
        if (!isDisarmed && Time.fixedTime - lastClashedOn  > CLASH_DURATION) {
            foreach (var preyScript in prey.gameObject.GetComponents<IPiercable>()) {
                preyScript.GetPierced ();
            }
        }
    }

    public void Disarm()
    {
        isDisarmed = true;
    }

}
