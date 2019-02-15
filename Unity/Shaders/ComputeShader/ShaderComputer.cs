using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderComputer : MonoBehaviour
{
    public GameObject prefab;

    [Space]
    public int count = 4;
    public int radius = 10;
    public float centerScale = 30;

    private Rigidbody[] particles;
    private Vector3[] positions;
    private Vector3[] parameters;
    private Vector3[] forces;

    private Vector3[] forceMatrix;

    //private float elektronMass = 1f / 1836.1527f;
    private float protonMass = 0.1f;
    private float elektronMass = 0.01f;
    private ComputeShader shader;

    private int kernelCalcRelativeForces = 0;
    private int kernelSumForces = 0;

    ComputeBuffer positionBuffer;
    ComputeBuffer chargeBuffer;
    ComputeBuffer matrixBuffer;
    ComputeBuffer forceBuffer;

    private void Start()
    {
        shader = Resources.Load<ComputeShader>("SampleComputeShader");
        kernelCalcRelativeForces = shader.FindKernel("CalcRelativeForces");
        kernelSumForces = shader.FindKernel("SumForces");

        positions = new Vector3[count];
        parameters = new Vector3[count];
        forces = new Vector3[count];

        forceMatrix = new Vector3[count * count];

        particles = new Rigidbody[count];

        CreatePrafabs();
        //CreateCustomPrafabs();
    }

    private void CreatePrafabs()
    {
        Color[] colors = new Color[] { Color.red, Color.yellow };
        Vector3[] centers = new Vector3[]
        {
             new Vector3(0, 0, 0),
             new Vector3(centerScale * radius, 0, 0),
             new Vector3(centerScale * radius / 2, centerScale * radius * 0.87f, 0)
        };
        int groupCount = centers.Length;

        Random.InitState(0);

        for (int i = 0; i < count; i++)
        {
            var go = Instantiate(prefab, Random.insideUnitSphere * radius, Quaternion.identity);
            go.transform.SetParent(transform);
            particles[i] = go.GetComponent<Rigidbody>();
            //particles[i].AddForce(Random.insideUnitSphere * 0.1f, ForceMode.Impulse);

            particles[i].position += centers[i % groupCount];

            if (i % 2 == 0)
            {
                parameters[i].x = protonMass;
                parameters[i].y = 1;

                particles[i].GetComponent<Renderer>().material.color = colors[0];
            }
            else
            {
                parameters[i].x = elektronMass;
                parameters[i].y = -1f;

                particles[i].GetComponent<Renderer>().material.color = colors[1];
            }

            particles[i].mass = parameters[i].x;
        }
    }

    private void CreateCustomPrafabs()
    {
        Color[] colors = new Color[] { Color.red, Color.yellow };
        int groupCount = colors.Length;
        Vector3[] centers = new Vector3[]
        {
             new Vector3(0.5f, 0.87f * 0.334f, 0),
             new Vector3(0, 0, 0),
             new Vector3(1, 0, 0),
             new Vector3(0.5f, 0.87f, 0),
        };


        CreatePrefab(colors, centers, 0, 1);
        CreatePrefab(colors, centers, 1, -1);
        CreatePrefab(colors, centers, 2, -1);
        CreatePrefab(colors, centers, 3, -1);
    }

    private void CreatePrefab(Color[] colors, Vector3[] centers, int i, int charge)
    {
        var go = Instantiate(prefab, radius * centers[i], Quaternion.identity);
        go.transform.SetParent(transform);
        particles[i] = go.GetComponent<Rigidbody>();

        if (charge == 1)
        {
            parameters[i].x = protonMass;
            parameters[i].y = 1;

            particles[i].GetComponent<Renderer>().material.color = colors[0];
        }
        else
        {
            parameters[i].x = elektronMass;
            parameters[i].y = -1f;

            particles[i].GetComponent<Renderer>().material.color = colors[1];
        }

        particles[i].mass = parameters[i].x;
    }

    private void InitData()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            positions[i] = particles[i].position;
            forces[i] = Vector3.zero;
        }
    }

    private void Update()
    {
        InitData();
        CalcData();
        MoveItems();
    }

    private void CalcData()
    {
        int steps = 32 * 2;

        positionBuffer = new ComputeBuffer(particles.Length, 12);
        chargeBuffer = new ComputeBuffer(particles.Length, 12);
        matrixBuffer = new ComputeBuffer(particles.Length * particles.Length, 12);
        forceBuffer = new ComputeBuffer(particles.Length, 12);

        positionBuffer.SetData(positions);
        chargeBuffer.SetData(parameters);
        forceBuffer.SetData(forces);

        shader.SetBuffer(kernelCalcRelativeForces, "positions", positionBuffer);
        shader.SetBuffer(kernelCalcRelativeForces, "forceMatrix", matrixBuffer);
        shader.SetBuffer(kernelCalcRelativeForces, "charges", chargeBuffer);
        shader.Dispatch(kernelCalcRelativeForces, steps, steps, 1);

        shader.SetBuffer(kernelSumForces, "forces", forceBuffer);
        shader.SetBuffer(kernelSumForces, "forceMatrix", matrixBuffer);
        shader.Dispatch(kernelSumForces, steps, 1, 1);

        //matrixBuffer.GetData(forceMatrix);
        forceBuffer.GetData(forces);

        positionBuffer.Release();
        chargeBuffer.Release();
        matrixBuffer.Release();
        forceBuffer.Release();
    }

    private void MoveItems()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].AddForce(1f *forces[i]);
        }
    }

    [ContextMenu("StopItems")]
    private void StopItems()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].velocity = Vector3.zero;
        }
    }
}
