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
    float speedM​ultiplier;

    void Update()
    {
        speedM​ultiplier = SolarSystemManager.Instance.Speed;
        Revolution();
        Rotation();
    }

    void Revolution()
    {
        currentRevolutionAngle += revolutionSpeed * speedM​ultiplier * Time.deltaTime;
        float rad = DegToRad(currentRevolutionAngle);
        Vector3 dir = new Vector3(Mathf.Cos(rad), 0.0f, Mathf.Sin(rad));
        transform.position = centerPlanet.position + (dir * distance);
    }

    void Rotation()
    {
        currentRotationAngle += rotationSpeed * speedM​ultiplier * Time.deltaTime;
        Vector3 currentEuler = transform.eulerAngles;
        currentEuler.y = currentRotationAngle;
        transform.eulerAngles = currentEuler;
    }

    float DegToRad(float deg) => deg * (Mathf.PI / 180);
}
