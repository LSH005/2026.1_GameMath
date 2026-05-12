using UnityEngine;

public class BezierLine : MonoBehaviour
{
    [Header("Bezier Curve Settings")]
    public Transform startPoint;
    public Transform endPoint;
    public Transform startHandlePoint;
    public Transform endHandlePoint;
    public bool isThirdOrder;

    float time;

    void Update()
    {
        time += Time.deltaTime / 3;
        if (isThirdOrder) 
            transform.position = GetOptimizedThirdOrderBezierPoint(startPoint.position, endPoint.position, startHandlePoint.position, endHandlePoint.position, time);
        else
            transform.position = GetBezierPoint(startPoint.position, endPoint.position, startHandlePoint.position, time);

        if (time > 1) Destroy(gameObject);
    }

    public Vector3 GetBezierPoint(Vector3 start, Vector3 end, Vector3 handle, float t)
    {
        Vector3 a = Vector3.Lerp(start, handle, t);
        Vector3 b = Vector3.Lerp(handle, end, t);
        return Vector3.Lerp(a, b, t);
    }

    public Vector3 GetOptimizedBezierPoint(Vector3 start, Vector3 end, Vector3 handle, float t)
    {
        float u = 1 - t;
        return u * u * start + 2 * u * t * handle + t * t * end;
    }

    public Vector3 GetThirdOrderBezierPoint(Vector3 start, Vector3 end, Vector3 handle1, Vector3 handle2, float t)
    {
        Vector3 a = GetOptimizedBezierPoint(start, handle1, handle2, t);
        Vector3 b = GetOptimizedBezierPoint(handle1, handle2, end, t);
        return Vector3.Lerp(a, b, t);
    }

    public Vector3 GetOptimizedThirdOrderBezierPoint(Vector3 start, Vector3 end, Vector3 handle1, Vector3 handle2, float t)
    {
        float u = 1 - t;
        return Mathf.Pow(u, 3) * start + 3 * Mathf.Pow(u, 2) * t * handle1 + 3 * u * Mathf.Pow(t, 2) * handle2 + Mathf.Pow(t, 3) * end;
    }
}
