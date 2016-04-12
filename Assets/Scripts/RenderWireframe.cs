using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RenderWireframe : MonoBehaviour {

    private enum meshState { Off = 0, On = -1 };
    private meshState state = meshState.Off;
    private List<MeshRenderer> RendererArray = new List<MeshRenderer>();
    public Material WireframeMaterial;
    public Material BasicMaterial;
    private Material[] NoWireframe = new Material[1];
    private Material[] WithWireframe = new Material[2];

    void Start ()
    {
        NoWireframe[0] = BasicMaterial;
        WithWireframe[0] = BasicMaterial;
        WithWireframe[1] = WireframeMaterial;

        var default_child = this.gameObject.transform.GetChild(0);
        foreach (Transform child in default_child)
        {
            RendererArray.Add(child.GetComponent<MeshRenderer>());
        }
    }

    void OnGUI()
    {
        uint buttonWidth = 80;
        uint buttonHeight = 30;
        uint spacing = 20;
        Rect ButtonRect = new Rect(spacing, Screen.height - buttonHeight - spacing, buttonWidth, buttonHeight);
        if (GUI.Button(ButtonRect, "Mesh:" + state.ToString()))
        {
            state = ~state;
            Material[] materialArray = NoWireframe;
            if (state == meshState.On)
            {
                materialArray = WithWireframe;
            }
            foreach(MeshRenderer meshRenderer in RendererArray)
            {
                meshRenderer.materials = materialArray;
            }
        }
    }
}
