using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkDestroyer : AbstractDestroyer
{
    protected override void CreateSeparateObjects()
    {
        goList = new List<GameObject>();

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        Vector3[] mas = new Vector3[3];
        Vector2[] uvs = new Vector2[3];

        for (int i = 0; i < triangles.Length; i += 3)
        {
            GameObject part = CreateChildGO(rootGO, "Go_" + i / 3);
            goList.Add(part);

            mas[0] = vertices[triangles[i + 0]];
            mas[1] = vertices[triangles[i + 1]];
            mas[2] = vertices[triangles[i + 2]];

            Vector3 center = (mas[0] + mas[1] + mas[2]) / 3f;
            mas[0] -= center;
            mas[1] -= center;
            mas[2] -= center;

            part.transform.position += center;


            uvs[0] = myUV[triangles[i + 0]];
            uvs[1] = myUV[triangles[i + 1]];
            uvs[2] = myUV[triangles[i + 2]];

            AddMesh(part, CreateMesh(mas, uvs));
        }

        SendSeparatedObjects();
    }

    protected override Mesh CreateMesh(Vector3[] vertices, Vector2[] uvs)
    {
        Mesh mesh = new Mesh();
        var list = new List<Vector3>();
        list.AddRange(vertices);
        Vector3 upPoint = Vector3.Cross(vertices[0], vertices[1]);
        list.AddRange(new Vector3[] { -upPoint });

        int[] triangles1 = new int[vertices.Length];
        int[] triangles2 = new int[vertices.Length * 3];

        triangles1[0] = 0;
        triangles1[1] = 1;
        triangles1[2] = 2;

        triangles2[0] = 3;
        triangles2[1] = 1;
        triangles2[2] = 0;

        triangles2[3] = 3;
        triangles2[4] = 2;
        triangles2[5] = 1;

        triangles2[6] = 3;
        triangles2[7] = 0;
        triangles2[8] = 2;

        mesh.subMeshCount = 2;
        mesh.SetVertices(list);
        mesh.SetTriangles(triangles1, 0);
        mesh.SetTriangles(triangles2, 1);
        mesh.RecalculateNormals();

        return mesh;
    }
}
