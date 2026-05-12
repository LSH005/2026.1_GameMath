using System.Collections.Generic;
using UnityEngine;

public class DeCasteljau : MonoBehaviour
{
    public List<Transform> points = new List<Transform>();

    [Header("Object")]
    public float spawnInterval = 0.2f;
    public DeCasteljauLine line;

    float time;

    void Update()
    {
        time += Time.deltaTime / spawnInterval;
        if (time > 1)
        {
            time = 0;
            DeCasteljauLine newLine = Instantiate(line, points[0].position, Quaternion.identity);

            newLine.points = points;
        }
    }

}
