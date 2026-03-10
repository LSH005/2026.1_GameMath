using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickToMove : MonoBehaviour
{
    public PlayerMovement moveScript;
    public float moveSpeed = 5f;
    Vector2 mousePos;
    Vector2 targetPos;

    Coroutine movingCoroutine;

    public void OnPoint(InputValue value)
    {
        mousePos = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if (moveScript.isDashing) return;

        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit[] hits = Physics.RaycastAll(ray, 50);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject != gameObject)
                {
                    targetPos = hit.point;
                    StartMove();
                    break;
                }
            }
        }
    }

    void StartMove()
    {
        if (movingCoroutine != null)
        {
            StopCoroutine(movingCoroutine);
        }

        movingCoroutine = StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        moveScript.canMove = false;
        Vector2 direction = targetPos - (Vector2)transform.position;
        moveScript.lastMoveVector = direction;

        while (GetDistance(transform.position, targetPos) > 0.05f && moveScript.moveInput == Vector2.zero && !moveScript.isDashing)
        {
            transform.Translate(GetNormalizedVector2(direction) * moveSpeed * Time.deltaTime);
            yield return null;
        }

        moveScript.canMove = true;
    }

    float GetDistance(Vector2 pos1, Vector2 pos2)
    {
        if (pos1 == pos2)
        {
            return 0f;
        }
        else
        {
            float x = Mathf.Pow(pos1.x - pos2.x,2);
            float y = Mathf.Pow(pos1.y - pos2.y,2);
            return Mathf.Sqrt(x + y);
        }
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
