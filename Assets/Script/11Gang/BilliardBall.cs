using UnityEngine;

public class BilliardBall : MonoBehaviour
{
    Vector3 savedPos = Vector3.zero;
    Rigidbody rb;
    public bool WasMove() => Vector3.Distance(transform.position, savedPos) > 0.025f;
    public void SaveCurrentPos() => savedPos = transform.position;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 velo = rb.linearVelocity;
        velo.y = 0;
        rb.linearVelocity = velo;
    }
}
