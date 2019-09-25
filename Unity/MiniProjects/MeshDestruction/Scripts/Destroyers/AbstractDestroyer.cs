using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum DestroyType { Surface, Chunks, Crack }
public abstract class AbstractDestroyer : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private bool addDestroyerForChildren = false;
    [SerializeField]
    private int RegenerationCount = 1;

    [SerializeField]
    private bool isExplosion = true;

    private MeshDestroyerSettings meshSettings;

    protected GameObject rootGO;
    protected Mesh mesh;
    protected List<GameObject> goList;

    protected Vector3[] myVertices;
    protected int[] myTriangles;
    protected Vector2[] myUV;

    protected Material[] materials;
    protected Vector3 hitPoint;
    protected Vector3 hitBarycentric;
    protected int triangleIndex;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (addDestroyerForChildren)
        {
            AddDestroyerForChildren(typeof(MeshRenderer));
            AddDestroyerForChildren(typeof(SkinnedMeshRenderer));
        }

        if (GetComponent<MeshFilter>() == null && GetComponent<SkinnedMeshRenderer>() == null)
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        var settings = AssetDatabase.FindAssets("MeshDestroyerSettings");
        string path = AssetDatabase.GUIDToAssetPath(settings[0]);
        meshSettings = (MeshDestroyerSettings)AssetDatabase.LoadAllAssetsAtPath(path)[0];
    }
    #endregion

    public void DestroyMesh(Vector3 hitPoint, int triangleIndex, Vector3 hitBarycentric)
    {
        this.hitPoint = hitPoint;
        this.triangleIndex = triangleIndex;
        this.hitBarycentric = hitBarycentric;

        InitializeMesh();
        InitializeMaterials();

        rootGO = new GameObject("Root of " + name);
        rootGO.transform.position = transform.position;

        gameObject.SetActive(false);
        CreateSeparateObjects();
    }

    #region Private Methods
    private void AddDestroyerForChildren(System.Type type)
    {
        var children = GetComponentsInChildren(type);
        if (children.Length > 0)
        {
            foreach (var item in children)
            {
                AbstractDestroyer destr = (AbstractDestroyer)item.gameObject.AddComponent(this.GetType());
                destr.RegenerationCount = RegenerationCount;
                destr.addDestroyerForChildren = false;
            }
        }
    }

    private void InitializeMesh()
    {
        if (GetComponent<MeshFilter>())
        {
            mesh = GetComponent<MeshFilter>().mesh;
        }

        if (mesh == null)
        {
            mesh = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        if (RegenerationCount >= 0)
        {
            for (int i = 0; i < RegenerationCount; i++)
            {
                mesh = MeshGenerator.RegenerateMesh(mesh);
            }
        }
        else
        {
            for (int i = RegenerationCount; i < 0; i++)
            {
                mesh = MeshGenerator.GeneralizeMesh(mesh);
            }
        }

        myVertices = mesh.vertices;
        myTriangles = mesh.triangles;
        myUV = mesh.uv;
    }

    private void InitializeMaterials()
    {
        Material[] temp = null;
        if (GetComponent<MeshRenderer>())
        {
            temp = GetComponent<MeshRenderer>().materials;
        }

        if (GetComponent<SkinnedMeshRenderer>())
        {
            temp = GetComponent<SkinnedMeshRenderer>().materials;
        }

        materials = new Material[temp.Length + meshSettings.materials.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            materials[i] = temp[i];
        }

        for (int i = 0; i < meshSettings.materials.Length; i++)
        {
            materials[i + temp.Length] = meshSettings.materials[i];
        }
    }
    #endregion

    #region Protected Methods
    protected abstract void CreateSeparateObjects();
    protected abstract Mesh CreateMesh(Vector3[] vertices, Vector2[] uvs);

    protected GameObject CreateChildGO(GameObject parent, string name)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform);
        go.transform.position = parent.transform.position;

        return go;
    }

    protected void AddMesh(GameObject go, Mesh mesh)
    {
        var filter = go.AddComponent<MeshFilter>();
        filter.sharedMesh = mesh;

        var rend = go.AddComponent<MeshRenderer>();
        rend.materials = materials;
    }

    protected void SendSeparatedObjects()
    {
        gameObject.SetActive(false);
        FindObjectOfType<MeshExplosioner>().AddExplosionForce(goList, hitPoint, ExplosionEnded);
    }

    private void ExplosionEnded()
    {
        gameObject.SetActive(true);
        Destroy(goList[0].transform.parent.gameObject);
    }
    #endregion
}
