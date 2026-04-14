using UnityEngine;

public class Geometric : MonoBehaviour
{
    int GeometricDistribution(float chance)
    {
        int trials = 1;
        while (Random.value >= chance)
        {
            trials++;
        }
        return trials;
    }

    private void Start()
    {
        int result = GeometricDistribution(0.1f); // 10% 확률로 성공하는 경우까지의 시도 횟수
        Debug.Log($"Number of trials until first success: {result}");
    }
}
