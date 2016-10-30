using UnityEngine;
using System.Collections;
using Util;
using Interfaces;
using System.Collections.Generic;

public class SpaceTrigger : MonoBehaviour
{
    private List<D.Cu<Collider>> callbacks = new List<D.Cu<Collider>>();
    private List<D.Cu<Collider>> exitCallbacks = new List<D.Cu<Collider>>();

    void OnTriggerEnter(Collider collider)
    {
        callbacks.ForEach (cb => cb(collider));
    }

    void OnTriggerExit(Collider collider)
    {
        exitCallbacks.ForEach (cb => cb(collider));
    }

    public void OnIn(D.Cu<Collider> callback)
    {
        this.callbacks.Add(callback);
    }

    public void OnOut(D.Cu<Collider> callback)
    {
        this.exitCallbacks.Add(callback);
    }
}
