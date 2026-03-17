using UnityEngine;

public class CoolBall : MonoBehaviour
{
    [Header("setting")]
    public float angle;
    public float power;

    [Header("Component")]
    public Rigidbody rb;

    void Start()
    {
        float rad = DegToRad(angle);
        Vector3 dir = new Vector3(Mathf.Cos(rad), 0.0f, Mathf.Sin(rad));

        rb.AddForce(dir.normalized * power);
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -5f) Destroy(this.gameObject);
    }

    float DegToRad(float deg) => deg * (Mathf.PI / 180);
}
