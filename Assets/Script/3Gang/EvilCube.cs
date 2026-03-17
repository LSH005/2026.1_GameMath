using UnityEngine;

public class EvilCube : MonoBehaviour
{
    [Header("player")]
    public Transform player;
    [Header("½Ã¾ß°¢")]
    public float viewAngle = 45f;


    private void Awake()
    {
        if (player == null)
        {
            this.enabled = false;
            return;
        }
    }

    void Update()
    {
        Vector3 toPlayer = player.position - transform.position;
        Vector3 forward = transform.forward;

        toPlayer.y = forward.y = 0;

        toPlayer = GetNormalizedVector3(toPlayer);
        forward = GetNormalizedVector3(forward);

        float dot = DotProduct(forward,toPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        if (angle < viewAngle / 2)
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

    float GetDotViewAngle(float DegreeViewAngle) => 1 - (DegreeViewAngle / 180);

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
