using UnityEngine;

public class BouncyBomb : MonoBehaviour
{
    [Header("Move")]
    public Vector3 velocity = new Vector3(2f, -3f, 0f);
    public Vector3 gravity = new Vector3(0, -9.81f, 0f);

    public float damping = 0.5f;
    [Header("Explosion")]
    public GameObject particle;
    public float force = 50f;
    public float radius = 5f;
    public float upwardsModifier = 1f;


    int boingCount = 0;

    void Update()
    {
        velocity += gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Explosion();
            return;
        }

        Vector3 normal = col.contacts[0].normal.normalized; // 충돌 지점의 법선 벡터
        Reflect(normal);

        if (++boingCount == 3) Explosion();
    }

    void Reflect(Vector3 normal)
    {
        float dot = Vector3.Dot(velocity, normal);
        Vector3 reflect = velocity - 2f * dot * normal;
        velocity = reflect * damping;
    }

    void Explosion()
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
        SummonParticle(50);
        Destroy(gameObject);
    }

    void SummonParticle(int amount)
    {
        for (int i = 0; i < amount; ++i)
        {
            Instantiate(particle, transform.position, Quaternion.identity);
        }
    }
}
