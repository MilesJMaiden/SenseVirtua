using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    [Header("Materials")]
    public Material normalMaterial;
    public Material hoveredMaterial;

    private Renderer renderer;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        SetNormalState();
    }

    public void SetNormalState()
    {
        if (renderer != null && normalMaterial)
        {
            renderer.material = normalMaterial;
        }
    }

    public void SetHoveredState()
    {
        if (renderer != null && hoveredMaterial != null)
        {
            renderer.material = hoveredMaterial;
        }
    }
}
