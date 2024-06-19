/* Copyright © 2021 edify
 *
 * Class member region order:
 * Unity Fields > Nested Classes > Enums > Delegates > Events > Fields >
 * Properties > Constructors > Finalisers > Methods > Unity Methods.
 *
 * Access modifier order:
 * Public > Internal > Protected Internal > Protected > Private.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Edify.LessonCreator
{
    [Serializable]
    public struct SegmentSize
    {
        public int index;
        public float size;
        public Color color;
    }

    public class RadialMenuButtons : MonoBehaviour
    {
        #region Unity Fields
        // ========================================================Unity Fields
        //  ............................................................Private
        [SerializeField] private int numberOfSegments = 7;
        [SerializeField] private float innerRadius = 50f;
        [SerializeField] private float outerRadius = 100f;
        [SerializeField] private float spacing = 5f;
        [SerializeField] private float startingAngle = 0f; // Added for specifying the starting angle
        [SerializeField] private float rotationAngle = 0f; // Added for rotating the entire menu
        [SerializeField] private List<SegmentSize> segmentSizes; // List to store specific segment sizes and their indices

        #endregion

        #region Methods
        //  ============================================================Methods
        //  ............................................................Private

        /// <summary>
        /// Generates the radial menu based on the defined parameters.
        /// </summary>
        private void GenerateRadialMenu()
        {
            // Initialize a dictionary to store the specified segment sizes
            Dictionary<int, SegmentSize> specifiedSegmentSizes = new Dictionary<int, SegmentSize>();
            float totalDefinedSize = 0;

            // Populate the dictionary with specified segment sizes and calculate total defined size
            foreach (var segmentSize in segmentSizes)
            {
                specifiedSegmentSizes[segmentSize.index] = segmentSize;
                totalDefinedSize += segmentSize.size;
            }

            // Calculate the remaining size for unspecified segments
            int unspecifiedSegmentCount = numberOfSegments - specifiedSegmentSizes.Count;
            float totalSpacing = spacing * numberOfSegments;
            float remainingSize = 360f - totalDefinedSize - totalSpacing;
            float defaultSize = remainingSize / unspecifiedSegmentCount;

            // Generate each segment of the radial menu
            float currentAngle = startingAngle; // Start from the specified starting angle
            for (int i = 0; i < numberOfSegments; i++)
            {
                float segmentSize = defaultSize;
                if (specifiedSegmentSizes.ContainsKey(i))
                {
                    segmentSize = specifiedSegmentSizes[i].size;
                }

                CreateSegment(i, currentAngle, segmentSize);
                currentAngle += segmentSize + spacing;
            }
        }

        /// <summary>
        /// Creates a segment of the radial menu.
        /// </summary>
        /// <param name="index">The index of the segment.</param>
        /// <param name="startAngle">The starting angle of the segment.</param>
        /// <param name="size">The angular size of the segment.</param>
        private void CreateSegment(int index, float startAngle, float size)
        {
            // Create a new GameObject for the segment
            GameObject segment = new GameObject("Segment" + index);
            segment.transform.parent = this.transform;

            // Add and configure the MeshFilter and MeshRenderer components
            Mesh mesh = new Mesh();
            segment.AddComponent<MeshFilter>().mesh = mesh;
            segment.AddComponent<MeshRenderer>();

            // Create the vertices for the segment
            List<Vector3> vertices = new List<Vector3>();
            List<int> triangles = new List<int>();

            vertices.Add(Vector3.zero); // Center vertex

            // Calculate the vertices for the inner and outer edges of the segment
            float endAngle = startAngle + size;
            float angleIncrement = Mathf.Max(size / 5, 1f); // Ensure a minimum increment to limit vertices
            for (float angle = startAngle; angle <= endAngle; angle += angleIncrement)
            {
                float radian = Mathf.Deg2Rad * angle;
                vertices.Add(new Vector3(Mathf.Cos(radian) * innerRadius, Mathf.Sin(radian) * innerRadius, 0));
                vertices.Add(new Vector3(Mathf.Cos(radian) * outerRadius, Mathf.Sin(radian) * outerRadius, 0));
            }

            // Create the triangles for the segment
            for (int i = 1; i < vertices.Count - 2; i += 2)
            {
                triangles.Add(0); // Center vertex
                triangles.Add(i);
                triangles.Add(i + 2);

                triangles.Add(i);
                triangles.Add(i + 1);
                triangles.Add(i + 2);
            }

            // Apply the vertices and triangles to the mesh
            mesh.vertices = vertices.ToArray();
            mesh.triangles = triangles.ToArray();
            mesh.RecalculateNormals();
        }

        #endregion

        #region Unity Methods
        //  ======================================================Unity Methods

        /// <summary>
        /// Initializes the radial menu on start.
        /// </summary>
        private void Start()
        {
            GenerateRadialMenu();
        }

        /// <summary>
        /// Draws gizmos to visualize the radial menu in the editor.
        /// </summary>
        private void OnDrawGizmos()
        {
            // Initialize a dictionary to store the specified segment sizes
            Dictionary<int, SegmentSize> specifiedSegmentSizes = new Dictionary<int, SegmentSize>();
            float totalDefinedSize = 0;

            // Populate the dictionary with specified segment sizes and calculate total defined size
            foreach (var segmentSize in segmentSizes)
            {
                specifiedSegmentSizes[segmentSize.index] = segmentSize;
                totalDefinedSize += segmentSize.size;
            }

            // Calculate the remaining size for unspecified segments
            int unspecifiedSegmentCount = numberOfSegments - specifiedSegmentSizes.Count;
            float totalSpacing = spacing * numberOfSegments;
            float remainingSize = 360f - totalDefinedSize - totalSpacing;
            float defaultSize = remainingSize / unspecifiedSegmentCount;

            // Draw each segment of the radial menu
            float currentAngle = startingAngle; // Start from the specified starting angle
            for (int i = 0; i < numberOfSegments; i++)
            {
                float segmentSize = defaultSize;
                Color segmentColor = Color.white; // Default color for unspecified segments
                if (specifiedSegmentSizes.ContainsKey(i))
                {
                    segmentSize = specifiedSegmentSizes[i].size;
                    segmentColor = specifiedSegmentSizes[i].color;
                }

                DrawSegmentGizmo(currentAngle, segmentSize, segmentColor);
                currentAngle += segmentSize + spacing;
            }
        }

        /// <summary>
        /// Draws a gizmo representation of a segment.
        /// </summary>
        /// <param name="startAngle">The starting angle of the segment.</param>
        /// <param name="size">The angular size of the segment.</param>
        /// <param name="color">The color of the gizmo for the segment.</param>
        private void DrawSegmentGizmo(float startAngle, float size, Color color)
        {
            Gizmos.color = color;

            float endAngle = startAngle + size;
            float angleIncrement = Mathf.Max(size / 5, 1f); // Ensure a minimum increment to limit vertices
            Vector3 center = transform.position;

            Vector3 previousInner = Vector3.zero;
            Vector3 previousOuter = Vector3.zero;
            bool firstPoint = true;

            for (float angle = startAngle; angle <= endAngle; angle += angleIncrement)
            {
                float radian = Mathf.Deg2Rad * angle;
                Vector3 innerPoint = center + new Vector3(Mathf.Cos(radian) * innerRadius, Mathf.Sin(radian) * innerRadius, 0);
                Vector3 outerPoint = center + new Vector3(Mathf.Cos(radian) * outerRadius, Mathf.Sin(radian) * outerRadius, 0);

                if (!firstPoint)
                {
                    Gizmos.DrawLine(previousInner, innerPoint);
                    Gizmos.DrawLine(previousOuter, outerPoint);
                    Gizmos.DrawLine(innerPoint, outerPoint);
                }
                else
                {
                    firstPoint = false;
                }

                previousInner = innerPoint;
                previousOuter = outerPoint;
            }

            // Connect the last points to close the segment
            if (!firstPoint)
            {
                Gizmos.DrawLine(previousInner, center);
                Gizmos.DrawLine(previousOuter, center);
            }
        }

        #endregion
    }
}
