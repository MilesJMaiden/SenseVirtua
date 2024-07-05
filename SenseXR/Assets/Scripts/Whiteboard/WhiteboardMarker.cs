using OVR.OpenVR;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WhiteboardMarker : MonoBehaviour
{
    //Marker
    [SerializeField] private Transform _tip;
    [SerializeField] private int _penSize = 32;
    [SerializeField] private int _brushEdges = 8; // Number of edges for the brush (more edges = more circular)

    private Renderer _renderer;
    private Color[] _colours;
    private float _tipHeight;

    private RaycastHit _touch;
    private Whiteboard _whiteboard;
    private Vector2 _touchPos, _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;

    // Start is called before the first frame update
    void Start()
    {
        //Set-up
        _renderer = _tip.GetComponent<Renderer>();
        _tipHeight = _tip.GetComponent<MeshCollider>().bounds.size.y;

        // Generate circular brush texture
        _colours = GenerateCircularBrush(_penSize, _brushEdges, _renderer.material.color);
    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if (Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight))
        {
            //Check if touching whiteboard
            if (_touch.transform.CompareTag("Whiteboard"))
            {
                //If whiteboard has been cached, skip otherwise set
                if (_whiteboard == null)
                {
                    _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                //convert touch pos to whiteboard texture size (pixels)
                var x = (int)(_touchPos.x * _whiteboard.textureSize.x - (_penSize / 2));
                var y = (int)(_touchPos.y * _whiteboard.textureSize.y - (_penSize / 2));

                //out of bounds check
                if (y < 0 || y > _whiteboard.textureSize.y || x < 0 || x > _whiteboard.textureSize.x)
                {
                    return;
                }

                //If whiteboard was touched last frame, colour pixels
                if (_touchedLastFrame)
                {
                    _whiteboard.texture.SetPixels(x, y, _penSize, _penSize, _colours);

                    for (float f = 0.01f; f < 1.00f; f += 0.01f)
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                        _whiteboard.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, _colours);
                    }

                    //prevent abnormal pen rotation when drawing on whiteboard
                    transform.rotation = _lastTouchRot;

                    //update texture
                    _whiteboard.texture.Apply();
                }

                //set vals from last frame to access in current frame
                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;

                return;
            }
        }

        _whiteboard = null;
        _touchedLastFrame = false;
    }

    private Color[] GenerateCircularBrush(int size, int edges, Color color)
    {
        Color[] brush = new Color[size * size];
        float radius = size / 2f;
        Vector2 center = new Vector2(radius, radius);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                Vector2 point = new Vector2(x, y);
                float distance = Vector2.Distance(center, point);
                if (distance <= radius)
                {
                    float angle = Mathf.Atan2(point.y - center.y, point.x - center.x) * Mathf.Rad2Deg;
                    angle = (angle < 0) ? angle + 360 : angle;
                    float segment = 360f / edges;
                    if (Mathf.Floor(angle / segment) == angle / segment)
                    {
                        brush[x + y * size] = color;
                    }
                    else if (edges >= 16 && distance <= radius * 0.9f)
                    {
                        brush[x + y * size] = color;
                    }
                }
                else
                {
                    brush[x + y * size] = new Color(0, 0, 0, 0);
                }
            }
        }
        return brush;
    }
}
