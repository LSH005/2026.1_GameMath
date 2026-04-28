using UnityEngine;

public class Lerp : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public float moveDuration = 3f;

    float t = 0;

    void Update()
    {
        if (t < moveDuration)
        {
            t += Time.deltaTime;
            transform.position = ThatLerp(startPos.position, endPos.position, t / moveDuration);
        }
    }

    float ThatLerp(float start, float end, float t) => (1f - t) * start + t * end;
    Vector3 ThatLerp(Vector3 start, Vector3 end, float t) => (1f - t) * start + t * end;
}
