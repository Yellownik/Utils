using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator
{
    private Vector2[] limitAreaPoints;
    private float yDisplacement = 0f;

    public Vector3[] CornerPoints
    {
        get; private set;
    }

    private Mesh GenerateMesh()
    {
        Triangulator triangulator = new Triangulator(limitAreaPoints);
        int[] indices = triangulator.Triangulate();

        Vector3[] vertices = new Vector3[limitAreaPoints.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = new Vector3(limitAreaPoints[i].x, yDisplacement, limitAreaPoints[i].y);
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = indices;

        mesh = RegenerateMesh(mesh);
        mesh = RegenerateMesh(mesh);
        mesh = RegenerateMesh(mesh);
        mesh = RegenerateMesh(mesh);

        mesh = DeleteVertices(mesh);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }

    public static Mesh RegenerateMesh(Mesh mesh)
    {
        Vector3[] oldVertices = mesh.vertices;
        int[] oldIndices = mesh.GetIndices(0);
        Vector2[] oldUVs = mesh.uv;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        int newI = 0;
        for (int i = 0; i < oldIndices.Length; i += 3)
        {
            Vector3 a = oldVertices[oldIndices[i]];
            Vector3 b = oldVertices[oldIndices[i + 1]];
            Vector3 c = oldVertices[oldIndices[i + 2]];

            Vector3 ab = (a + b) * 0.5f;
            Vector3 bc = (b + c) * 0.5f;
            Vector3 ca = (c + a) * 0.5f;
                                           // 0  1  2  3   4   5
            vertices.AddRange(new Vector3[] { a, b, c, ab, bc, ca });

            // a, ab, ca
            triangles.AddRange(new int[] { newI + 0, newI + 3, newI + 5 });
            // ab, b, bc                 
            triangles.AddRange(new int[] { newI + 3, newI + 1, newI + 4 });
            // bc, c, ca                 
            triangles.AddRange(new int[] { newI + 4, newI + 2, newI + 5 });
            // ca, ab, bc
            triangles.AddRange(new int[] { newI + 5, newI + 3, newI + 4 });
            newI += 6;

            Vector2 aUV = oldUVs[oldIndices[i]];
            Vector2 bUV = oldUVs[oldIndices[i + 1]];
            Vector2 cUV = oldUVs[oldIndices[i + 2]];
            uvs.AddRange(new Vector2[] { aUV, bUV, cUV, (aUV + bUV) / 2f, (bUV + cUV) / 2f, (cUV + aUV) / 2f });
        }

        Mesh resultMesh = new Mesh();
        resultMesh.vertices = vertices.ToArray();
        resultMesh.triangles = triangles.ToArray();
        resultMesh.uv = uvs.ToArray();

        return resultMesh;
    }

    public static Mesh GeneralizeMesh(Mesh mesh)
    {
        return new Mesh();
    }

    private Mesh DeleteVertices(Mesh mesh)
    {
        List<Vector3> oldVertices = new List<Vector3>();
        mesh.GetVertices(oldVertices);
        int[] oldIndices = mesh.GetIndices(0);

        List<int> indices = new List<int>();

        Dictionary<Vector3, List<int>> dict = new Dictionary<Vector3, List<int>>();
        for (int i = 0; i < oldVertices.Count; i++)
        {
            if (dict.ContainsKey(oldVertices[i]) == false)
            {
                dict.Add(oldVertices[i], new List<int>() { i });
            }
            else
            {
                dict[oldVertices[i]].Add(i);
            }
        }

        List<Vector3> vertices = new List<Vector3>();
        vertices.AddRange(dict.Keys);

        for (int i = 0; i < oldIndices.Length; i++)
        {
            Vector3 vec = oldVertices[oldIndices[i]];
            indices.Add(vertices.IndexOf(vec));
        }

        Mesh resultMesh = new Mesh();
        resultMesh.vertices = vertices.ToArray();
        resultMesh.triangles = indices.ToArray();
        return resultMesh;
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
