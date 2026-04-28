using UnityEngine;

public class SLerp : MonoBehaviour
{
    public Transform target;
    public float speed = 2f;

    Quaternion startRotation;
    bool hasTarget = false;

    private void Start()
    {
        startRotation = transform.rotation;
    }

    void Update()
    {
        Quaternion lookRotation = Quaternion.identity;
        if (target == null) hasTarget = false;
        if (!hasTarget) lookRotation = startRotation;
        else lookRotation = Quaternion.LookRotation(target.position - transform.position);

        float t = 1 - Mathf.Exp(-speed * Time.deltaTime);
        transform.rotation = ThatSlerp(transform.rotation, lookRotation, t);
    }


    Quaternion ThatSlerp(Quaternion from, Quaternion to, float t)
    {
        float dot = Quaternion.Dot(from, to);

        if (1 - dot < 0.0001f)
        {
            Quaternion lerp = new Quaternion(
                ThatLerp(from.x, to.x, t),
                ThatLerp(from.y, to.y, t),
                ThatLerp(from.z, to.z, t),
                ThatLerp(from.w, to.w, t)
                );

            return lerp.normalized;
        }

        if (dot < 0)
        {
            to = new Quaternion(-to.x, -to.y, -to.z, -to.w);
            dot = -dot;
        }

        float theta = Mathf.Acos(dot);
        float sinTheta = Mathf.Sin(theta);

        float ratioA = Mathf.Sin((1f - t) * theta) / sinTheta;
        float ratioB = Mathf.Sin(t * theta) / sinTheta;

        Quaternion result = new Quaternion(
            ratioA * from.x + ratioB * to.x,
            ratioA * from.y + ratioB * to.y,
            ratioA * from.z + ratioB * to.z,
            ratioA * from.w + ratioB * to.w
            );

        return result.normalized;
    }

    float ThatLerp(float start, float end, float t) => (1f - t) * start + t * end;

    public void StopTracking()
    {
        hasTarget = false;
    }

    public void StartTracking(Transform newTarget)
    {
        target = newTarget;
        hasTarget = true;
    }
}
