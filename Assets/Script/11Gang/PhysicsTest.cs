using UnityEngine;

public class PhysicsTest : MonoBehaviour
{
    public float forcePower = 1.0f;
    [SerializeField] private float speed;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) return;

        rb.AddForce(Vector3.forward * forcePower, ForceMode.Impulse);
    }

    private void Update()
    {
        speed = rb.linearVelocity.magnitude;
    }
}
