using UnityEngine;

public class EvilCube : MonoBehaviour
{
    public Transform player;

    void Update()
    {
        Vector3 toPlayer = player.position - transform.position;
        Vector3 forward = transform.forward;

        toPlayer.y = forward.y = 0;

        toPlayer = GetNormalizedVector3(toPlayer);
        forward = GetNormalizedVector3(forward);

        float dot = DotProduct(forward,toPlayer);

        if (dot > 0.5f)
        {
            Vector3 targetScale = Vector3.one;
            targetScale.y = 10;
            transform.localScale = targetScale;
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    float DotProduct(Vector3 A, Vector3 B)
    {
        return A.x * B.x + A.y * B.y + A.z * B.z;
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
