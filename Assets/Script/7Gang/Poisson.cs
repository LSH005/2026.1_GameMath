using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Poisson : MonoBehaviour
{
    int PoissonDistribution(float lambda) // 주어진 평균 이벤트 수(lambda)를 기반으로 포아송 분포에서 무작위로 이벤트 수를 생성.
    {
        int k = 0;
        float p = 1f;
        float L = Mathf.Exp(-lambda);
        while (p > L)
        {
            k++;
            p *= Random.value;
        }
        return k - 1;
    }

    void Start()
    {
        List<int> counts = new List<int>();

        for (int i = 0; i < 100; i++)
        {
            int count = PoissonDistribution(50f);
            Debug.Log($"Minute {i + 1}: {count} events");
            counts.Add(count);
        }

        print($"Average events per minute: {counts.Average()}");
    }
}