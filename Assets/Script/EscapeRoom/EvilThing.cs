using UnityEngine;
using UnityEngine.SceneManagement;

public class EvilThing : MonoBehaviour
{
    [Header("player")]
    public Transform player;
    [Header("시야각/범위")]
    public float viewAngle = 45f;
    public float viewDistance = 5.0f;
    [Header("속도 / 회전속도")]
    public float moveSpeed = 5f;
    public float rotateSpeed = 45f;
    [Header("공격 범위")]
    public float attackRange;

    bool isAngry = false;


    private void Awake()
    {
        if (player == null)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (!isAngry) FindPlayer();
        else TrackPlayer();
    }

    void FindPlayer()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);

        if (GetDistance(transform.position, player.position) > viewDistance) return;

        Vector3 toPlayer = player.position - transform.position;
        Vector3 forward = transform.forward;

        toPlayer.y = forward.y = 0;

        toPlayer = GetNormalizedVector3(toPlayer);
        forward = GetNormalizedVector3(forward);

        float dot = GetDotProduct(forward, toPlayer);
        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        isAngry = angle < viewAngle / 2;
    }

    void TrackPlayer()
    {
        transform.LookAt(player);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);

        if (GetDistance(transform.position, player.position) < attackRange)
        {
            EscapeRoomPlayer playerScript = player.gameObject.GetComponent<EscapeRoomPlayer>();
            if (!playerScript.parryingLeft && !playerScript.parryingRight)
            {
                Kill();
                return;
            }

            Vector3 toMe = transform.position - player.position;
            Vector3 forward = player.forward;

            toMe.y = forward.y = 0;

            toMe = GetNormalizedVector3(toMe);
            forward = GetNormalizedVector3(forward);

            Vector3 crossProduct = GetCrossProduct(forward, toMe);
            bool attackingRight = crossProduct.y > 0;

            if ((playerScript.parryingRight && attackingRight) || (playerScript.parryingLeft && !attackingRight))
            {
                Destroy(gameObject);
            }
            else
            {
                Kill();
            }
        }
    }

    void Kill()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

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


