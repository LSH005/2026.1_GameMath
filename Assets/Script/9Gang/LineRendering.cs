using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LineRendering : MonoBehaviour
{
    public Transform endPos;

    [Range(1f,5f)] public float extend = 1.5f;

    LineRenderer lr;
    bool randering = false;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.widthMultiplier = 0.02f;
        lr.material = new Material(Shader.Find("Unlit/Color"))
        {
            color = Color.red
        };
    }

    void Update()
    {
        if (!randering) return;

        if (!endPos)
        {
            StopRendering();
            return;
        }
        Vector3 prediction = ThatLerp(transform.position, endPos.position, extend);
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, prediction);
    }

    Vector3 ThatLerp(Vector3 start, Vector3 end, float t) => (1f - t) * start + t * end;

    public void StopRendering()
    {
        lr.positionCount = 0;
        randering = false;
    }

    public void StartRendering(Transform target)
    {
        endPos = target;
        lr.positionCount = 2;
        randering = true;
    }
}
