using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMover : MonoBehaviour
{
    public bool ShowCircles = true;

    public GridActor gridActor;
    [Range(-1, 10)]
    public float movementSpeed = 1;
    public float[] radiuses;

    [HideInInspector]
    public Vector2[] centers;
    private Vector2[] directions;
    private float areaSize = 10;

    #region Unity Methods
    void Start()
    {
        InitializeCircles();
    }
 
    void Update()
    {
        MoveCircles();
    }

    private void OnDrawGizmos()
    {
        if (centers == null || !ShowCircles)
        {
            return;
        }

        Gizmos.color = Color.green;
        for (int i = 0; i < centers.Length; i++)
        {
            Gizmos.DrawSphere(centers[i], radiuses[i]);
        }
    }
    #endregion

    private void InitializeCircles()
    {
        areaSize = gridActor.areaSize;

        centers = new Vector2[radiuses.Length];
        directions = new Vector2[radiuses.Length];
        for (int i = 0; i < centers.Length; i++)
        {
            centers[i].x = Random.Range(0, areaSize - 2 * radiuses[i]);
            centers[i].y = Random.Range(0, areaSize - 2 * radiuses[i]);

            directions[i] = Random.insideUnitSphere;
        }
    }

    private void MoveCircles()
    {
        Vector2 newPos;
        for (int i = 0; i < centers.Length; i++)
        {
            newPos = centers[i] + directions[i] * movementSpeed * Time.deltaTime;

            if(newPos.x < radiuses[i])
            {
                newPos.x = radiuses[i];
                directions[i].x *= -1;
            }
            if (newPos.x > areaSize - radiuses[i])
            {
                newPos.x = areaSize - radiuses[i];
                directions[i].x *= -1;
            }

            if (newPos.y < radiuses[i])
            {
                newPos.y = radiuses[i];
                directions[i].y *= -1;
            }
            if (newPos.y > areaSize - radiuses[i])
            {
                newPos.y = areaSize - radiuses[i];
                directions[i].y *= -1;
            }

            centers[i] = newPos;
        }
    }
}
