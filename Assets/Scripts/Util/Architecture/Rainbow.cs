using UnityEngine;
using System.Collections;
using Assets.Scripts.Interfaces;

[ExecuteInEditMode]
public class Rainbow : MonoBehaviour 
{
    public Renderer materialHolder;
    public Color color = new Color(0,1,0);

    void Awake ()
    {
        if (materialHolder != null) {
            // http://answers.unity3d.com/questions/283271/material-leak-in-editor.html
            var tempMaterial = new Material (materialHolder.sharedMaterial);
            tempMaterial.color = color;
            materialHolder.sharedMaterial = tempMaterial;
        }
    }

    void Update ()
    {
        if (materialHolder != null) {
            if (materialHolder.sharedMaterial.color != color) {
                materialHolder.sharedMaterial.color = color;
                foreach (var vdator in gameObject.GetComponents<IValidator>()) {
                    vdator.OnValidate();
                }
            }
        }
    }
}
