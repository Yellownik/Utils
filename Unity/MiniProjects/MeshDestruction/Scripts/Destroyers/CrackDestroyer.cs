using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackDestroyer : AbstractDestroyer
{
    protected override void CreateSeparateObjects()
    {
        if (triangleIndex != -1)
        {
            AddCrackedTriangle();
        }

        goList = new List<GameObject>();

        Vector3[] mas = new Vector3[3];
        Vector2[] uvs = new Vector2[3];

        Vector3 scale = gameObject.transform.localScale;
        Quaternion rotation = transform.rotation;

        for (int i = 0; i < myTriangles.Length; i += 3)
        {
            GameObject part = CreateChildGO(rootGO, "Go_" + i / 3);
            goList.Add(part);

            for (int j = 0; j < 3; j++)
            {
                mas[j] = myVertices[myTriangles[i + j]];
                mas[j].Scale(scale);
                mas[j] = rotation * mas[j];

                uvs[j] = myUV[myTriangles[i + j]];
            }

            Vector3 center = (mas[0] + mas[1] + mas[2]) / 3f;
            part.transform.position += center;

            mas[0] -= center;
            mas[1] -= center;
            mas[2] -= center;

            AddMesh(part, CreateMesh(mas, uvs));
        }

        SendSeparatedObjects();
    }

    protected override Mesh CreateMesh(Vector3[] vertices, Vector2[] uvs)
    {
        Mesh mesh = new Mesh();
        var verticesList = new List<Vector3>();
        verticesList.AddRange(vertices);
        verticesList.AddRange(vertices);

        var uvList = new List<Vector2>();
        uvList.AddRange(uvs);
        uvList.AddRange(uvs);

        int[] triangles1 = new int[vertices.Length];
        int[] triangles2 = new int[vertices.Length];

        triangles1[0] = 0;
        triangles1[1] = 1;
        triangles1[2] = 2;

        triangles2[0] = 5;
        triangles2[1] = 4;
        triangles2[2] = 3;

        mesh.subMeshCount = 2;
        mesh.SetVertices(verticesList);

        mesh.SetTriangles(triangles1, 0);
        mesh.SetTriangles(triangles2, 1);

        mesh.uv = uvList.ToArray();
        mesh.RecalculateNormals();

        return mesh;
    }

    private void AddCrackedTriangle()
    {
        int aIndex = myTriangles[triangleIndex * 3 + 0];
        int bIndex = myTriangles[triangleIndex * 3 + 1];
        int cIndex = myTriangles[triangleIndex * 3 + 2];
        int newIndex = myVertices.Length;

        Vector3 a = myVertices[aIndex];
        Vector3 b = myVertices[bIndex];
        Vector3 c = myVertices[cIndex];
        Vector3 newPoint = a * hitBarycentric.x + b * hitBarycentric.y + c * hitBarycentric.z;


        Vector2 aUV = myUV[aIndex];
        Vector2 bUV = myUV[bIndex];
        Vector2 cUV = myUV[cIndex];
        Vector2 newPointUV = aUV * hitBarycentric.x + bUV * hitBarycentric.y + cUV * hitBarycentric.z;

        List<Vector3> vertices1 = myVertices.ToList();
        List<Vector2> uv1 = myUV.ToList();
        List<int> triangles1 = myTriangles.ToList();

        vertices1.Add(newPoint);
        uv1.Add(newPointUV);

        triangles1.AddRange(new int[] { aIndex, bIndex, newIndex });
        triangles1.AddRange(new int[] { bIndex, cIndex, newIndex });
        triangles1.AddRange(new int[] { cIndex, aIndex, newIndex });

        for (int i = 0; i < 3; i++)
        {
            triangles1.RemoveAt(triangleIndex * 3);
        }

        myVertices = vertices1.ToArray();
        myUV = uv1.ToArray();
        myTriangles = triangles1.ToArray();
    }
}
