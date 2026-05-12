using System.Collections.Generic;
using UnityEngine;

public class DeCasteljauLine : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();
    float time;
    void Update()
    {
        time += Time.deltaTime / 3;
        transform.position = GetDeCasteljau(points, time);

        if (time > 1) Destroy(gameObject);
    }

    Vector3 GetDeCasteljau(List<Transform> originalList, float t)
    {
        List<Vector3> p = new List<Vector3>();
        foreach (var point in originalList) p.Add(point.position);

        while (p.Count > 1)
        {
            int last = p.Count - 1;

            var next = new List<Vector3>();
            for (int i = 0; i < last; i++)
                next.Add(Vector3.Lerp(p[i], p[i + 1], t));
            p = next;
        }
        
        return p[0];
    }
}
