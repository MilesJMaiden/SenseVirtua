using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Whiteboard : MonoBehaviour
{
    public Texture2D texture;
    public Vector2 textureSize = new Vector2(2048, 2048);

    private Color[] initialPixels;

    void Start()
    {
        var renderer = GetComponent<Renderer>();
        texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
        renderer.material.mainTexture = texture;
        ClearWhiteboard();
    }

    public void ClearWhiteboard()
    {
        initialPixels = texture.GetPixels();
        texture.SetPixels(initialPixels);
        texture.Apply();
    }

    public void ResetTexture()
    {
        texture.SetPixels(initialPixels);
        texture.Apply();
    }
}
