using UnityEngine;

public class Bezier : MonoBehaviour
{
    [Header("Bezier Curve Settings")]
    public Transform startPoint;
    public Transform endPoint;
    public Transform startHandlePoint;
    public Transform endHandlePoint;
    public bool isThirdOrder;

    [Header("Object")]
    public float spawnInterval = 0.2f;
    public BezierLine line;

    float time;

    void Update()
    {
        time += Time.deltaTime / spawnInterval;
        if (time > 1)
        {
            time = 0;
            BezierLine newLine = Instantiate(line, startPoint.position, Quaternion.identity);
            newLine.startPoint = startPoint;
            newLine.endPoint = endPoint;
            newLine.startHandlePoint = startHandlePoint;
            if (isThirdOrder)
            {
                newLine.endHandlePoint = endHandlePoint;
                newLine.isThirdOrder = true;
            }
            
        }
    }
}
