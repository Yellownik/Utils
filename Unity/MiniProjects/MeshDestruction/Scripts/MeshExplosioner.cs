using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshExplosioner : MonoBehaviour
{
    [SerializeField]
    private float force = 2f;

    [SerializeField]
    private float Duration = 2f;

    private static MeshExplosioner instance = null;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
        }

        instance = this;
    }

    public void AddExplosionForce(List<GameObject> goList, Vector3 center, System.Action callback)
    {
        StartCoroutine(AddForce(goList, center, callback));
    }

    private IEnumerator AddForce(List<GameObject> list, Vector3 center, System.Action callback)
    {
        float endTime = Time.time + Duration;
        var transformList = list.Select(go => go.transform).ToList();
        var directionList = list.Select(go => (go.transform.position - center).normalized * force).ToList();

        while (Time.time < endTime)
        {
            yield return new WaitForEndOfFrame();

            for (int k = 0; k < list.Count; k++)
            {
                transformList[k].position += (directionList[k] * Time.deltaTime);
            }
        }
        endTime += Duration;
        while (Time.time < endTime)
        {
            yield return new WaitForEndOfFrame();

            for (int k = 0; k < list.Count; k++)
            {
                transformList[k].position -= (directionList[k] * Time.deltaTime);
            }
        }

        callback();
    }
}
