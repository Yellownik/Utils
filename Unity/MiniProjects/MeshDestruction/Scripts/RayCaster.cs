using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCaster : MonoBehaviour
{
    private Camera camera;
    private bool onlyOneChild = true;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onlyOneChild = true;
            RayCast();
        }

        if (Input.GetMouseButtonDown(1))
        {
            onlyOneChild = false;
            RayCast();
        }
    }

    private void RayCast()
    {
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.ScreenPointToRay(Input.mousePosition).direction, out hit))
        {
            MeshDestroying(hit);
        }
    }

    private void MeshDestroying(RaycastHit hit)
    {
        GameObject go = hit.collider.gameObject;
        var meshDestroyers = go.GetComponentsInChildren<AbstractDestroyer>();

        if (meshDestroyers.Length > 0)
        {
            if (onlyOneChild)
            {
                int index = Random.Range(0, meshDestroyers.Length);
                meshDestroyers[index].DestroyMesh(hit.point, hit.triangleIndex, hit.barycentricCoordinate);
            }
            else
            {
                foreach (var item in meshDestroyers)
                {
                    item.DestroyMesh(hit.point, hit.triangleIndex, hit.barycentricCoordinate);
                }

                go.SetActive(false);
            }
        }
    }

}
