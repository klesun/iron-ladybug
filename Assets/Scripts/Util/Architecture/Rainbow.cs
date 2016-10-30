using UnityEngine;
using System.Collections;

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
            materialHolder.sharedMaterial.color = color;
        }
    }
}
