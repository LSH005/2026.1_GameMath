using System.Collections.Generic;
using UnityEngine;

public class BombMovement : MonoBehaviour
{
    public float lifeTime = 3;
    public List<Vector3> points = new List<Vector3>();
    float time;

    bool isDestroyed = false;
    TrailRenderer tr;

    private void Awake()
    {
        tr = GetComponent<TrailRenderer>();
        tr.enabled = false;
    }

    void Update()
    {
        if (time > lifeTime && !isDestroyed)
        {
            Destroy(gameObject, lifeTime);
            isDestroyed = true;
        }

        if (isDestroyed) return;

        time += Time.deltaTime;
        float t = CalculateNonLinearT(time, lifeTime);

        if (!tr.enabled && time > 0.5f)
        {
            tr.enabled = true;
            tr.time = lifeTime / 2;
        }

        transform.position = GetDeCasteljau(points, t);
    }

    Vector3 GetDeCasteljau(List<Vector3> p, float t)
    {
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

    float CalculateNonLinearT(float current, float total)
    {
        float pauseDuration = total / 15f;
        float moveDuration = (total - pauseDuration) / 2f;

        if (current < moveDuration)
        {
            float localProgress = current / moveDuration;
            return Mathf.Sin(localProgress * Mathf.PI * 0.5f) * 0.5f;
        }
        else if (current < moveDuration + pauseDuration) return 0.5f;
        else
        {
            float localProgress = (current - moveDuration - pauseDuration) / moveDuration;
            localProgress = Mathf.Clamp01(localProgress);
            return 0.5f + (1f - Mathf.Cos(localProgress * Mathf.PI * 0.5f)) * 0.5f;
        }
    }
}
