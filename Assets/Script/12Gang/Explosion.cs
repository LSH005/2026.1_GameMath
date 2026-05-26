using UnityEngine;

public class Explosion : MonoBehaviour
{
    public float force = 300f;
    public float radius = 5f;
    public float upwardsModifier = 1f;

    void Start()
    {
        Invoke(nameof(Explode), 2f);
    }

    //void RunExplode()
    //{
    //    Vector3 explosionPos = transform.position;
    //    Collider[] hitColliders = Physics.OverlapSphere(explosionPos, radius);
    //    foreach (var col in hitColliders)
    //    {
    //        Rigidbody rb = col.attachedRigidbody;
    //        if (rb != null)
    //        {
    //            rb.AddExplosionForce(force, explosionPos, radius);
    //        }
    //    }
    //    Destroy(gameObject);
    //}

    void Explode()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (var col in colliders)
        {
            Rigidbody rb = col.attachedRigidbody;
            if (rb == null) continue;
            Vector3 toTarget = rb.position - explosionPos;
            float distance = toTarget.magnitude;
            Vector3 dir = toTarget.normalized;
            float attenuation = 1f - Mathf.Clamp01(distance / radius);
            dir += Vector3.up * upwardsModifier;
            dir = dir.normalized;
            Vector3 impulse = dir * force * attenuation;
            rb.AddForce(impulse, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
