using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class EscapeRoomPlayer : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 80f;
    [Header("DeathY")]
    public float deathY;

    [Header("parrying?")]
    public bool parryingRight;
    public bool parryingLeft;

    Vector3 moveInput;
    float rotate;
    Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnMove(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        moveInput = new Vector3(0, 0, input.y);
        rotate = input.x;
    }

    public void OnRightParry(InputValue value)
    {
        parryingRight = value.isPressed;
    }

    public void OnLeftParry(InputValue value)
    {
        parryingLeft = value.isPressed;
    }

    private void Update()
    {
        if (transform.position.y < deathY)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        rb.AddForce(transform.rotation * GetNormalizedVector3(moveInput) * moveSpeed * Time.deltaTime, ForceMode.Force);
        Quaternion rotation = Quaternion.Euler(0f, rotate * rotateSpeed * Time.deltaTime, 0f);
        transform.rotation = rotation * transform.rotation;
    }

    public Vector3 GetNormalizedVector3(Vector3 vector)
    {
        float magnitude = GetMagnitudeVector3(vector);

        if (magnitude > 0)
        {
            return vector / magnitude;
        }
        else
        {
            return Vector3.zero;
        }
    }
    float GetMagnitudeVector3(Vector3 vector) => Mathf.Sqrt((vector.x * vector.x) + (vector.y * vector.y) + (vector.z * vector.z));
}
