using UnityEngine;

public class Binomial : MonoBehaviour
{
    int BinomialDistribution(int trials, float chance)
    {
        int successes = 0;
        for (int i = 0; i < trials; i++)
        {
            if (Random.value < chance)
                successes++;
        }
        return successes;
    }



    void Start()
    {
        // 10번 시도 중 30% 확률로 성공하는 경우의 수를 시뮬레이션
        int trials = 10;
        float chance = 0.3f;
        
        int successes = BinomialDistribution(trials, chance);
        Debug.Log($"{trials} trials, {successes} successes.");
    }
}
