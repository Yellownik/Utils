using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceDestroyer : AbstractDestroyer
{
    protected override void CreateSeparateObjects()
    {
        goList = new List<GameObject>();

        Vector3[] mas = new Vector3[3];
        Vector2[] uvs = new Vector2[3];

        Vector3 scale = gameObject.transform.localScale;
        Quaternion rotation = transform.rotation;

        for (int i = 0; i < myTriangles.Length; i += 3)
        {
            GameObject part = CreateChildGO(rootGO, "Go_" + i / 3);
            goList.Add(part);

            mas[0] = myVertices[myTriangles[i + 0]];
            mas[1] = myVertices[myTriangles[i + 1]];
            mas[2] = myVertices[myTriangles[i + 2]];

            mas[0].Scale(scale);
            mas[1].Scale(scale);
            mas[2].Scale(scale);

            mas[0] = rotation * mas[0];
            mas[1] = rotation * mas[1];
            mas[2] = rotation * mas[2];

            Vector3 center = (mas[0] + mas[1] + mas[2]) / 3f;
            mas[0] -= center;
            mas[1] -= center;
            mas[2] -= center;

            part.transform.position += center;


            uvs[0] = myUV[myTriangles[i + 0]];
            uvs[1] = myUV[myTriangles[i + 1]];
            uvs[2] = myUV[myTriangles[i + 2]];

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
}
