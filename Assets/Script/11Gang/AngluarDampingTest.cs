using UnityEngine;

public class AngluarDampingTest : MonoBehaviour
{
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) return;

        rb.angularVelocity = Vector3.forward * 100;
    }
}
