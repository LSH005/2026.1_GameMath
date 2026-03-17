using UnityEngine;

public class MoveWithAngle : MonoBehaviour
{
    public float angle;
    public float speed;

    void Update()
    {
        float rad = DegToRad(angle);

        Vector3 direction = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad));
        transform.position += direction * speed * Time.deltaTime;
    }

    float RadToDeg(float rad) => rad * (180 / Mathf.PI);
    float DegToRad(float deg) => deg * (Mathf.PI / 180);
}
