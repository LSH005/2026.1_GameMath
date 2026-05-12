using System.Collections;
using UnityEngine;

public class BombLauncher : MonoBehaviour
{
    public Transform startPos;
    public Transform endPos;

    [Header("범위")]
    public float Point1Range = 10;
    public float Point2Range = 10;

    [Header("폭탄")]
    public BombMovement bombPrefab;
    public float fireInterval = 0.1f;

    bool bombRunning = false;

    public void LaunchManyBomb(int bombCount)
    {
        if (!bombRunning)
        {
            StartCoroutine(LaunchManyBombCoroutine(bombCount));
            bombRunning = true;
        }
    }

    IEnumerator LaunchManyBombCoroutine(int bombCount)
    {
        for (int i = 0; i < bombCount; i++)
        {
            LaunchBomb();
            yield return new WaitForSeconds(fireInterval);
        }
        bombRunning = false;
    }

    public void LaunchBomb()
    {
        BombMovement bomb = Instantiate(bombPrefab, startPos.position, Quaternion.identity);

        Vector3 Point2 = startPos.position + (Random.onUnitSphere * Point1Range);
        Vector3 Point4 = startPos.position + (Random.onUnitSphere * Point1Range * 3f);
        Vector3 Point5 = startPos.position + (Random.onUnitSphere * Point1Range * 1.5f);
        Vector3 lastPoint = endPos.position + (Random.onUnitSphere * Point2Range);

        float distance = Vector3.Distance(Point2, lastPoint);
        Vector3 Point3 = (Point2 + lastPoint) / 2 + (Random.insideUnitSphere * (distance / 2));
        
        bomb.points.Add(startPos.position);
        bomb.points.Add(Point2);
        bomb.points.Add(Point3);
        bomb.points.Add(Point4);
        bomb.points.Add(Point5);
        bomb.points.Add(lastPoint);
        bomb.points.Add(endPos.position);
    }
}
