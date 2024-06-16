using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2 (2048, 2048);

    void Start()
    {
        var renderer = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        renderer.material.mainTexture = texture;
    }
}