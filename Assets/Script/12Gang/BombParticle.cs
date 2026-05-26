using UnityEngine;

public class BombParticle : MonoBehaviour
{
    public float flingPower = 15.0f;
    public Rigidbody rb;
    float t = 0f;

    Vector3 startScale;
    Vector3 endScale;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    void Start()
    {
        transform.localScale *= Random.Range(0.8f, 1.2f);
        startScale = transform.localScale;

        Vector3 velocity = Random.onUnitSphere;
        velocity.y = Mathf.Abs(velocity.y);

        rb.linearVelocity = velocity * Random.Range(flingPower / 2, flingPower);
        rb.angularVelocity = velocity * Random.Range(flingPower / 2, flingPower);
    }

    private void Update()
    {
        t += Time.deltaTime / 3f;
        if (t < 1f)
        {
            transform.localScale = Vector3.Lerp(startScale, endScale, t);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
