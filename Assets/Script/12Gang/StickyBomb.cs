using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickyBomb : MonoBehaviour
{
    [Header("Explosion")]
    public GameObject particle;
    public float force = 50f;
    public float radius = 5f;
    public float upwardsModifier = 1f;
    public float explosionWaitTime = 2;


    Vector3 originalScale;
    Quaternion originalRotation;


    void Start()
    {
        StartCoroutine(Operation());
    }

    IEnumerator Operation()
    {
        Vector3 start = transform.position;
        Vector3 end = transform.forward * 5 + transform.position;
        float time = 0;

        while (time <= 1)
        {
            time += Time.deltaTime * 2;
            float t = Mathf.Sin(time * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(start, end, t);

            yield return null;
        }
        transform.position = end;

        time = 0.01f;
        float multiple = 1.1f;
        float totalWaitTime = 0;
        Stack<float> stack = new Stack<float>();
        while (totalWaitTime <= explosionWaitTime)
        {
            stack.Push(time);
            totalWaitTime += time;
            time *= multiple;
        }

        originalRotation = transform.rotation;
        originalScale = transform.localScale;

        while (true)
        {
            if (stack.TryPop(out float value))
            {
                float waitTime = value / 2;
                transform.localScale = GetRandomScale3D();
                transform.rotation = Quaternion.LookRotation(GetRandomDirection());
                yield return new WaitForSeconds(waitTime);
                transform.localScale = originalScale;
                transform.rotation = originalRotation;
                yield return new WaitForSeconds(waitTime);
            }
            else
            {
                break;
            }
        }

        Explosion();
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

    Vector3 GetRandomScale3D()
    {
        float randomX = Random.Range(0.5f, 1.5f);
        float randomY = Random.Range(0.5f, 1.5f);
        float randomZ = Random.Range(0.5f, 1.5f);

        return new Vector3(randomX, randomY, randomZ);
    }

    Vector3 GetRandomDirection()
    {
        return Random.onUnitSphere;
    }
}
