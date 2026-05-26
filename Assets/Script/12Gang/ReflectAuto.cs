using UnityEngine;

public class ReflectAuto : MonoBehaviour
{
    public Vector3 velocity = new Vector3(2f, -3f, 0f);
    public Vector3 gravity = new Vector3(0, -9.81f, 0f);
    public float damping = 0.9f;

    void Update()
    {
        velocity += gravity * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }

    void OnCollisionEnter(Collision col)
    {
        Vector3 normal = col.contacts[0].normal.normalized; // 충돌 지점의 법선 벡터
        NotStupidReflect(normal);
    }

    void NotStupidReflect(Vector3 normal)
    {
        float dot = Vector3.Dot(velocity, normal);
        Vector3 reflect = velocity - 2f * dot * normal;
        velocity = reflect * damping;
    }

    void StupidReflect(Vector3 normal)
    {
        Vector3 reflect = Vector3.Reflect(velocity, normal); // 반사 벡터 계산
        velocity = reflect;
    }
}
