using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    //Mesh Extensions

    public static void RemoveUnusedVertices(this Mesh mesh)
    {
        Dictionary<int, int> verticesToChange = new Dictionary<int, int>();

        List<Vector3> vertices = mesh.vertices.ToList();
        List<Vector2> uv = mesh.uv.ToList();
        List<int> triangles = mesh.triangles.ToList();

        for (int i = 0, k = 0; i < mesh.vertices.Length; i++)
        {
            if (triangles.Contains(i))
            {
                verticesToChange.Add(i, i - k);
            }
            else
            {
                vertices.RemoveAt(i - k);
                uv.RemoveAt(i - k);
                k++;
            }
        }

        for (int i = 0; i < triangles.Count; i++)
        {
            triangles[i] = verticesToChange[triangles[i]];
        }

        Debug.Log((mesh.vertices.Length - vertices.Count) + " unused vertices was deleted");

        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public static void RemoveVertice(this Mesh mesh, int verticeIndex)
    {
        List<int> trianglesToRemove = new List<int>();

        for (int i = 0; i < mesh.triangles.Length; i++)
        {
            if (mesh.triangles[i] == verticeIndex)
            {
                trianglesToRemove.Add(Mathf.FloorToInt(i / 3));
            }
        }

        for (int i = trianglesToRemove.Count - 1; i >= 0; i--)
        {
            RemoveTriangle(mesh, trianglesToRemove[i]);
        }
    }

    public static void RemoveTriangle(this Mesh mesh, int triangleIndex)
    {
        if (triangleIndex > mesh.triangles.Length / 3 - 1 || triangleIndex < 0)
        {
            Debug.Log("Triangle index for this Mesh must be between 0 and " + (mesh.triangles.Length / 3 - 1));
            return;
        }

        List<int> newTriangles = mesh.triangles.ToList();

        for (int i = 0; i < 3; i++)
        {
            newTriangles.RemoveAt(triangleIndex * 3);
        }

        mesh.triangles = newTriangles.ToArray();
    }

    public static void DivideTriangle(this Mesh mesh, int triangleIndex, Vector3 newPoint)
    {
        if (triangleIndex > mesh.triangles.Length / 3 - 1 || triangleIndex < 0)
        {
            Debug.Log("Triangle index for this Mesh must be between 0 and " + (mesh.triangles.Length / 3 - 1));
            return;
        }

        Vector3 triangleVerticeA = mesh.vertices[mesh.triangles[triangleIndex * 3 + 0]];
        Vector3 triangleVerticeB = mesh.vertices[mesh.triangles[triangleIndex * 3 + 1]];
        Vector3 triangleVerticeC = mesh.vertices[mesh.triangles[triangleIndex * 3 + 2]];

        Vector2 triangleUVA = mesh.uv[mesh.triangles[triangleIndex * 3 + 0]];
        Vector2 triangleUVB = mesh.uv[mesh.triangles[triangleIndex * 3 + 1]];
        Vector2 triangleUVC = mesh.uv[mesh.triangles[triangleIndex * 3 + 2]];

        List<Vector3> vertices = mesh.vertices.ToList();
        List<Vector2> uv = mesh.uv.ToList();
        List<int> triangles = mesh.triangles.ToList();

        vertices.Add(triangleVerticeA);
        vertices.Add(triangleVerticeB);
        vertices.Add(newPoint);

        vertices.Add(triangleVerticeB);
        vertices.Add(triangleVerticeC);
        vertices.Add(newPoint);

        vertices.Add(triangleVerticeC);
        vertices.Add(triangleVerticeA);
        vertices.Add(newPoint);

        for (int i = mesh.vertices.Length; i < vertices.Count; i++)
        {
            triangles.Add(i);
        }

        float uvX = (triangleUVA.x + triangleUVB.x + triangleUVC.x) / 3;
        float uvY = (triangleUVA.y + triangleUVB.y + triangleUVC.y) / 3;

        Vector2 newUV = new Vector2(uvX, uvY);

        uv.Add(triangleUVA);
        uv.Add(triangleUVB);
        uv.Add(newUV);

        uv.Add(triangleUVB);
        uv.Add(triangleUVC);
        uv.Add(newUV);

        uv.Add(triangleUVC);
        uv.Add(triangleUVA);
        uv.Add(newUV);

        for (int i = 0; i < 3; i++)
        {
            triangles.RemoveAt(triangleIndex * 3);
        }

        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public static void DivideTriangle(this Mesh mesh, int triangleIndex)
    { 
        if (triangleIndex > mesh.triangles.Length / 3 - 1 || triangleIndex < 0)
        {
            Debug.Log("Triangle index for this Mesh must be between 0 and " + (mesh.triangles.Length / 3 - 1));
            return;
        }

        Vector3 triangleVerticeA = mesh.vertices[mesh.triangles[triangleIndex * 3 + 0]];
        Vector3 triangleVerticeB = mesh.vertices[mesh.triangles[triangleIndex * 3 + 1]];
        Vector3 triangleVerticeC = mesh.vertices[mesh.triangles[triangleIndex * 3 + 2]];

        float vX = (triangleVerticeA.x + triangleVerticeB.x + triangleVerticeC.x) / 3;
        float vY = (triangleVerticeA.y + triangleVerticeB.y + triangleVerticeC.y) / 3;
        float vZ = (triangleVerticeA.z + triangleVerticeB.z + triangleVerticeC.z) / 3;

        Vector3 newPoint = new Vector3(vX, vY, vZ);

        DivideTriangle(mesh, triangleIndex, newPoint);
    }

    //Collection Extensions

    public static List<T> ToList<T>(this IEnumerable<T> enumerable)
    {
        List<T> tempList = new List<T>();

        foreach (T temp in enumerable)
        {
            tempList.Add(temp);
        }

        return tempList;
    }

    public static bool AddNonExisting<T>(this IList<T> list, T objectToAdd)
    {
        if (!list.Contains(objectToAdd))
        {
            list.Add(objectToAdd);

            return true;
        }

        return false;
    }

    public static List<Vector2> ConvertToVector2(this List<Vector3> v3List)
    {
        List<Vector2> v2List = new List<Vector2>();

        foreach (Vector3 v3 in v3List)
        {
            v2List.Add(new Vector2(v3.x, v3.y));
        }

        return v2List;
    }
}