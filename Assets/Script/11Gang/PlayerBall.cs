using UnityEngine;

public class PlayerBall : MonoBehaviour
{
    public bool myTurn = false;

    Rigidbody rb;
    Vector3 savedPos = Vector3.zero;

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

    public void Launch(Vector3 dir)
    {
        rb.AddForce(dir/10f, ForceMode.Impulse);
    }

    public bool WasMove() => Vector3.Distance(transform.position, savedPos) > 0.025f;
    public void SaveCurrentPos() => savedPos = transform.position;
}
