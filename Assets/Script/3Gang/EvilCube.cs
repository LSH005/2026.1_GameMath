using Unity.Hierarchy;
using Unity.Mathematics;
using UnityEngine;

public class EvilCube : MonoBehaviour
{
    [Header("player")]
    public Transform player;
    [Header("시야각/범위")]
    public float viewAngle = 45f;
    public float viewDistance = 5.0f;


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
        if (GetDistance(transform.position, player.position) > viewDistance)
        {
            transform.localScale = Vector3.one;
            Debug.Log("플레이어는 범위 밖에 있음!");
            return;
        }

        Vector3 toPlayer = player.position - transform.position;
        Vector3 forward = transform.forward;

        toPlayer.y = forward.y = 0;

        toPlayer = GetNormalizedVector3(toPlayer);
        forward = GetNormalizedVector3(forward);

        Vector3 crossProduct = GetCrossProduct(forward, toPlayer);
        if (crossProduct.y > 0) Debug.Log("플레이어는 오른쪽에 있음");
        else if (crossProduct.y < 0) Debug.Log("플레이어는 왼쪽에 있음");

        float dot = GetDotProduct(forward, toPlayer);
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

    float GetDotProduct(Vector3 A, Vector3 B)
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

    float GetDistance(Vector3 pos1, Vector3 pos2)
    {
        if (pos1 == pos2)
        {
            return 0f;
        }
        else
        {
            float x = Mathf.Pow(pos1.x - pos2.x, 2);
            float y = Mathf.Pow(pos1.y - pos2.y, 2);
            float z = Mathf.Pow(pos1.z - pos2.z, 2);
            return Mathf.Sqrt(x + y + z);
        }
    }

    Vector3 GetCrossProduct(Vector3 A, Vector3 B)
    {
        return new Vector3(
            A.y * B.z - A.z * B.y,
            A.z * B.x - A.x * B.z,
            A.x * B.y - A.y * B.x
            );
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 forward = transform.forward * viewDistance;

        Vector3 left = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Vector3 right = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.DrawRay(transform.position, left);
        Gizmos.DrawRay(transform.position, right);
        Gizmos.DrawRay(transform.position, forward);
    }

}


