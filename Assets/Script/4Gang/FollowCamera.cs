using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3 (0, 4, -5);
    public float smoothSpeed = 5.0f;


    private void LateUpdate()
    {
        //Vector3 latePos = Quaternion.Euler(0f, target.eulerAngles.y, 0) * offset;
        Vector3 latePos = target.position + Quaternion.Euler(0f, target.eulerAngles.y, 0) * offset;
        transform.position = latePos;

        transform.LookAt(target);
    }
}
