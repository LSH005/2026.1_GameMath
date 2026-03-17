using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 5f;
    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.25f;
    public float dashCooldown = 0.2f;

    [HideInInspector] public bool canMove = true;
    [HideInInspector] public Vector2 moveInput;
    [HideInInspector] public bool isDashing;
    [HideInInspector] public Vector2 lastMoveVector = Vector2.right;

    bool canDash = true;
    bool isSprinting = false;
    Coroutine dashCoroutine;

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = value.isPressed;
    }

    public void OnJump(InputValue value)
    {
        if (!canDash) return;

        if (value.isPressed)
        {
            canDash = false;
            isDashing = true;
            if (dashCoroutine != null) StopCoroutine(dashCoroutine);
            dashCoroutine = StartCoroutine(DashCoroutine());
        }
    }

    void Update()
    {
        if (canMove && !isDashing)
        {
            Vector2 direction = new Vector2(moveInput.x, moveInput.y);
            float speed = isSprinting ? moveSpeed * 2 : moveSpeed;
            transform.Translate(GetNormalizedVector2(direction) * speed * Time.deltaTime);
            if (GetMagnitudeVector2(direction) > 0) lastMoveVector = direction;
        }
    }

    IEnumerator DashCoroutine()
    {
        float t = 0;

        while (t < dashDuration)
        {
            transform.Translate(GetNormalizedVector2(lastMoveVector) * dashSpeed * Time.deltaTime);
            t += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    float GetMagnitudeVector2(Vector2 vector) => Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);

    public Vector2 GetNormalizedVector2(Vector2 vector)
    {
        float magnitude = GetMagnitudeVector2(vector);

        if (magnitude > 0)
        {
            return vector / magnitude;
        }
        else
        {
            return Vector2.zero;
        }
    }
}