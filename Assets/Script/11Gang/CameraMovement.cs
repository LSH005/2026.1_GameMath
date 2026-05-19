using System.Collections;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement Instance;
    
    Transform target;
    Vector3 currentRotationVelocity;
    bool isTrackingTarget = false;
    Coroutine goToCoroutine;
    Coroutine rotateToPosCoroutine;
    BilliardsGameManager manager;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void LateUpdate()
    {
        if (currentRotationVelocity != Vector3.zero)
        {
            transform.Rotate(currentRotationVelocity * Time.deltaTime * 60f, Space.World);
        }

        if (isTrackingTarget)
        {
            if (target == null)
            {
                isTrackingTarget = false;
                return;
            }

            transform.LookAt(target);
        }
    }

    public void LookTransform(Transform transform)
    {
        target = transform;
        isTrackingTarget = true;
    }

    public void StopLookTransform()
    {
        target = null;
        isTrackingTarget = false;
    }

    public void GoTo(Vector3 pos, float duration)
    {
        if (goToCoroutine != null) StopCoroutine(goToCoroutine);
        goToCoroutine = StartCoroutine(GoToCoroutine(pos, duration));
    }

    IEnumerator GoToCoroutine(Vector3 targetPos, float duration)
    {
        float time = 0;
        Vector3 startPos = transform.position;

        while (time < duration)
        {
            time += Time.deltaTime;

            float localProgress = Mathf.Clamp01(time / duration);
            float t = Mathf.Sin(localProgress * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        transform.position = targetPos;
        goToCoroutine = null;
    }

    public void LinerGoTo(Vector3 pos, float duration)
    {
        if (goToCoroutine != null) StopCoroutine(goToCoroutine);
        goToCoroutine = StartCoroutine(LinerGoToCoroutine(pos, duration));
    }

    IEnumerator LinerGoToCoroutine(Vector3 targetPos, float duration)
    {
        float time = 0;
        Vector3 startPos = transform.position;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = Mathf.Clamp01(time / duration);
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            yield return null;
        }

        transform.position = targetPos;
        goToCoroutine = null;
    }

    public void Look(Vector3 pos, float duration = 0)
    {
        if (rotateToPosCoroutine != null) StopCoroutine(rotateToPosCoroutine);

        if (duration <= float.Epsilon)
        {
            transform.LookAt(pos);
            return;
        }

        rotateToPosCoroutine = StartCoroutine(LookCoroutine(pos, duration));
    }

    IEnumerator LookCoroutine(Vector3 pos, float duration)
    {
        float time = 0;
        Quaternion start = transform.rotation;
        Quaternion end = Quaternion.LookRotation(pos);

        while (time < duration)
        {
            time += Time.deltaTime;
            transform.rotation = Quaternion.Slerp(start, end, time / duration);

            yield return null;
        }

        transform.rotation = end;
        goToCoroutine = null;
    }

    public void RotateToPos(Quaternion target, float duration)
    {
        if (rotateToPosCoroutine != null) StopCoroutine(rotateToPosCoroutine);
        rotateToPosCoroutine = StartCoroutine(RotateToPosCoroutine(target, duration));
    }

    IEnumerator RotateToPosCoroutine(Quaternion target, float duration)
    {
        float time = 0;
        Quaternion start = transform.rotation;

        while (time < duration)
        {
            time += Time.deltaTime;

            float localProgress = Mathf.Clamp01(time / duration);
            float t = Mathf.Sin(localProgress * Mathf.PI * 0.5f);
            transform.rotation = Quaternion.Slerp(start, target, t);

            yield return null;
        }

        transform.rotation = target;
        goToCoroutine = null;
    }

    public void RotateBy(Vector3 rotationAngles)
    {
        currentRotationVelocity = rotationAngles;
    }
    public void AddManager(BilliardsGameManager newManager)
    {
        if (manager != null) manager = newManager;
    }
}
