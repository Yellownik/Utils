using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator
{
    private Vector2[] limitAreaPoints;
    private float yDisplacement = 0f;

    private Mesh GenerateMesh()
    {
        // Use the triangulator to get indices for creating triangles
        Triangulator triangulator = new Triangulator(limitAreaPoints);
        int[] indices = triangulator.Triangulate();

        // Create the Vector3 vertices
        Vector3[] vertices = new Vector3[limitAreaPoints.Length];
        Vector2[] uv = new Vector2[limitAreaPoints.Length];

        float minX = 0;
        float minZ = 0;
        float maxX = 0;
        float maxZ = 0;

        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(limitAreaPoints[i].x, yDisplacement, limitAreaPoints[i].y);
            if (vertices[i].x < minX)
            {
                minX = vertices[i].x;
            }
            if (vertices[i].z < minZ)
            {
                minZ = vertices[i].z;
            }

            if (vertices[i].x > maxX)
            {
                maxX = vertices[i].x;
            }
            if (vertices[i].z > maxZ)
            {
                maxZ = vertices[i].z;
            }
        }

        for (int i = 0; i < uv.Length; i++)
        {
            uv[i] = new Vector2((vertices[i].x - minX) / (maxX - minX), (vertices[i].z - minZ) / (maxZ - minZ));
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = indices;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    public Mesh GenerateMeshFromPoints(Vector3[] points)
    {
        limitAreaPoints = new Vector2[points.Length];
        float yAverageValue = 0;

        for (int i = 0; i < limitAreaPoints.Length; i++)
        {
            yAverageValue += points[i].y;
            this.limitAreaPoints[i] = new Vector2(points[i].x, points[i].z);
        }

        yAverageValue /= points.Length;
        yDisplacement = yAverageValue;

        return GenerateMesh();
    }
}
