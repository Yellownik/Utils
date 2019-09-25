using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalarFieldCalculator : MonoBehaviour
{
    public CircleMover circleMover;
    public GridActor grid;

    private float[] circleRadiuses;
    private Vector2[] circleCenters;

    private float[,] scalarField;

    #region Unity Methods
    void Start()
    {
        circleRadiuses = circleMover.radiuses;
        scalarField = new float[grid.dimension, grid.dimension];
    }
 
    void Update()
    {
        UpdateScalarField();
    }
    #endregion

    private void UpdateScalarField()
    {
        circleCenters = circleMover.centers;
        float r;
        float dx, dy;
        for (int x = 0; x < grid.dimension; x++)
        {
            for (int y = 0; y < grid.dimension; y++)
            {
                scalarField[x, y] = 0;
                for (int i = 0; i < circleCenters.Length; i++)
                {
                    r = circleRadiuses[i] * circleRadiuses[i];
                    dx = x * grid.cellSize - circleCenters[i].x;
                    dx *= dx;
                    dy = y * grid.cellSize - circleCenters[i].y;
                    dy *= dy;
                    float val = r / (dx + dy);
                    /*float val = Mathf.Pow(circleRadiuses[i], 2)
                        / (Mathf.Pow(x * grid.cellSize - circleCenters[i].x, 2) + Mathf.Pow(y * grid.cellSize - circleCenters[i].y, 2));*/
                    if (i % 3 == 2)
                        scalarField[x, y] -= val;
                    else
                        scalarField[x, y] += val;
                }

                grid.ValueAtPoint(x, y, scalarField[x, y]);
            }
        }
    }
}
