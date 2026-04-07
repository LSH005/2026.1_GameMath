using System.Linq;
using Unity.Hierarchy;
using UnityEngine;

public class StandardDeviation : MonoBehaviour
{
    public int sampleCount = 1000;
    public float randomMin = 0;
    public float randomMax = 100;

    void Start()
    {
        int n = sampleCount;
        float[] samples = new float[n];

        for (int i = 0; i < n; i++)
        {
            samples[i] = Random.Range(randomMin, randomMax);
        }

        float mean = samples.Average();
        float sumOfSquares = samples.Sum(x => Mathf.Pow(x - mean, 2));
        float stdDev = Mathf.Sqrt(sumOfSquares / n);
        print($"평균 : {mean}, 표준 편차 : {stdDev}, 균등 분포 : {GetGaussian(mean, stdDev)}");
    }

    float GetGaussian(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);

        return mean + stdDev * randStdNormal;
    }

    void TestStandardDeviation()
    {

        int n = sampleCount;
        float[] samples = new float[n];

        for (int i = 0; i < n; i++)
        {
            samples[i] = Random.Range(randomMin, randomMax);
        }

        float mean = samples.Average();
        float sumOfSquares = samples.Sum(x => Mathf.Pow(x - mean, 2));
        float stdDev = Mathf.Sqrt(sumOfSquares / n);
        print($"평균 : {mean}, 표준 편차 : {stdDev}");
    }
}
