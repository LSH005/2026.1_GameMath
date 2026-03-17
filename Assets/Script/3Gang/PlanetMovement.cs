using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    [Header("중심")]
    public Transform centerPlanet;

    [Header("거리, 자전, 공전")]
    public float distance;
    public float rotationSpeed; // 자전 속도
    public float revolutionSpeed;   // 공전 속도

    float currentRevolutionAngle;
    float currentRotationAngle;

    void Update()
    {
        Revolution();
        Rotation();
    }

    void Revolution()
    {
        currentRevolutionAngle += revolutionSpeed * Time.deltaTime;
        float rad = DegToRad(currentRevolutionAngle);
        Vector3 dir = new Vector3(Mathf.Cos(rad), 0.0f, Mathf.Sin(rad));
        transform.position = centerPlanet.position + (dir * distance);
    }

    void Rotation()
    {
        currentRotationAngle += rotationSpeed * Time.deltaTime;
        Vector3 currentEuler = transform.eulerAngles;
        currentEuler.y = currentRotationAngle;
        transform.eulerAngles = currentEuler;
    }

    float DegToRad(float deg) => deg * (Mathf.PI / 180);
}
