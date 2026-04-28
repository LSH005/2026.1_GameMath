using UnityEngine;

public class PingPong : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;
    public float moveDuration = 3f;

    float t = 0;

    void Update()
    {
        t = Mathf.PingPong(Time.time / moveDuration, 1f);
        transform.position = ThatLerp(startPos.position, endPos.position, t);
    }

    float ThatLerp(float start, float end, float t) => (1f - t) * start + t * end;
    Vector3 ThatLerp(Vector3 start, Vector3 end, float t) => (1f - t) * start + t * end;
}
