using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabGrid : GridActor
{
    public GameObject cellPrefab;
    public Material mat;
    private Dictionary<float, Material> matDict;

    private MeshRenderer[,] meshRenderers;

    private void Start()
    {
        matDict = new Dictionary<float, Material>();
        matDict[-1] = Instantiate<Material>(mat);
        matDict[-1].color = Color.cyan;

        matDict[-0.3f] = Instantiate<Material>(mat);
        matDict[-0.3f].color = Color.blue;

        matDict[1] = Instantiate<Material>(mat);
        matDict[1].color = Color.grey;

        matDict[2] = Instantiate<Material>(mat);
        matDict[2].color = Color.yellow;

        matDict[3] = Instantiate<Material>(mat);
        matDict[3].color = Color.red;

        CreateCells();
    }

    private void CreateCells()
    {
        meshRenderers = new MeshRenderer[dimension, dimension];

        GameObject go;
        for (int x = 0; x < dimension; x++)
        {
            for (int y = 0; y < dimension; y++)
            {
                go = Instantiate(cellPrefab, new Vector3(x, y, 0) * cellSize, Quaternion.identity, gameObject.transform);
                go.transform.localScale = Vector3.one * cellSize;
                meshRenderers[x, y] = go.GetComponent<MeshRenderer>();
            }
        }
    }

    public override void ValueAtPoint(int x, int y, float value)
    {
        foreach(var pair in matDict)
        {
            if(value < pair.Key)
            {
                meshRenderers[x, y].sharedMaterial = pair.Value;
                return;
            }
        }
        meshRenderers[x, y].sharedMaterial = mat;
    }  
}
