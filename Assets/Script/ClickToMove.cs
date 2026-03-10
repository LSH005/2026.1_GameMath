using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickToMove : MonoBehaviour
{
    public PlayerMovement moveScript;
    public float moveSpeed = 5f;
    [Header("TP")]
    public float teleportDuration = 0.3f;

    Vector2 mousePos;
    Vector2 targetPos;

    Coroutine movingCoroutine;
    Coroutine teleportCoroutine;
    bool isClickMoving;
    bool isTeleporting;

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

                    if (movingCoroutine != null)
                    {
                        StopCoroutine(movingCoroutine);
                    }
                    isClickMoving = true;
                    movingCoroutine = StartCoroutine(MoveCoroutine());

                    break;
                }
            }
        }
    }

    public void OnRightClick(InputValue value)
    {
        if (isTeleporting) return;

        if (isClickMoving)
        {
            isClickMoving = false;
            moveScript.canMove = true;

            if (movingCoroutine != null)
            {
                StopCoroutine(movingCoroutine);
            }
        }

        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit[] hits = Physics.RaycastAll(ray, 50);

            foreach (var hit in hits)
            {
                if (hit.collider.gameObject != gameObject)
                {
                    targetPos = hit.point;

                    if (teleportCoroutine != null)
                    {
                        StopCoroutine(teleportCoroutine);
                    }
                    isTeleporting = true;
                    teleportCoroutine = StartCoroutine(TeleportCoroutine());

                    break;
                }
            }
        }
    }

    IEnumerator TeleportCoroutine()
    {
        float halfDuration = teleportDuration / 2f;
        Vector3 originScale = transform.localScale;

        float t = 0;
        while (t < halfDuration)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, t / halfDuration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = Vector3.zero;
        transform.position = targetPos;
        t = 0;
        while (t < halfDuration)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, originScale, t / halfDuration);
            t += Time.deltaTime;
            yield return null;
        }
        transform.localScale = originScale;
        isTeleporting = false;
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

        isClickMoving = false;
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
