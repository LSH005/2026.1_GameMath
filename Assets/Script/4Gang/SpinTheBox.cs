using UnityEngine;
using UnityEngine.InputSystem;

public class SpinTheBox : MonoBehaviour
{
    public float rotateSpeed = 45f;

    Vector2 moveInput;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void Update()
    {
        //float rotation = moveInput.x * rotateSpeed * Time.deltaTime;
        //transform.Rotate(0, rotation, 0);

        Quaternion rotation = Quaternion.Euler(0f, moveInput.x * rotateSpeed * Time.deltaTime, 0);
        //transform.localRotation = transform.localRotation * rotation;   // 로컬
        //transform.rotation = transform.rotation * rotation;   // 로컬
        transform.rotation = rotation * transform.rotation; // 월드
    }
}
