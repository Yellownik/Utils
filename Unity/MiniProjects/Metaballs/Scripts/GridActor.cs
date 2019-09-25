using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GridActor : MonoBehaviour
{
    #region Fields
    [Range(10, 50)]
    public float areaSize = 10;
    [Range(10, 200)]
    public int dimension = 10;

    [HideInInspector]
    public float cellSize = 1;
    #endregion

    void Awake()
    {
        cellSize = areaSize / dimension;
    }

    public abstract void ValueAtPoint(int x, int y, float value);
}
