using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TurnBasedGame : MonoBehaviour
{

    [Header("Texts")]
    public TextMeshProUGUI combatDataText;
    public TextMeshProUGUI itemInventoryText;

    [Header("Values")]
    [SerializeField] float critChance = 0.2f;
    [SerializeField] float meanDamage = 20f;
    [SerializeField] float stdDevDamage = 5f;
    [SerializeField] float enemyHP = 100f;
    [SerializeField] float poissonLambda = 2f;
    [SerializeField] float hitRate = 0.6f;
    [SerializeField] float critDamageRate = 2f;
    [SerializeField] int maxHitsPerTurn = 5;

    int turn = 0;
    bool rareItemObtained = false;
    float rareItemChance = 0.2f;
    const float defaultRareItemChance = 0.2f;

    // 요소 계수기
    int totalEnemyCount = 0;
    int totalKillCount = 0;
    float totalHitRate = 0;
    float totalCritRare = 0;
    float maxDamageDealt = 0;
    float minDamageDealt = 0;



    string[] rewards = { "Gold", "Weapon", "Armor", "Potion" };
    Dictionary<string, int> itemInventory = new Dictionary<string, int>()
    {
        {"Gold", 0},
        {"Weapon", 0},
        {"Armor", 0},
        {"Potion", 0},
        {"Rare_Weapon", 0},
        {"Rare_Armor", 0}
    };

    const string RARE_PREFIX = "Rare_";

    //private void Start()
    //{
    //    StartSimulation();
    //}

    public void StartSimulation()
    {
        rareItemChance = defaultRareItemChance;
        // 기하분포 샘플링: 레어 아이템이 나올 때까지 반복하는 구조
        rareItemObtained = false;

        turn = 0;
        totalEnemyCount = 0;
        totalKillCount = 0;
        ResetInventory();

        while (!rareItemObtained)
        {
            SimulateTurn();
            turn++;
        }

        Debug.Log($"레어 아이템 {turn} 번째 턴에 획득");
        UpdateUI();
    }

    void ResetInventory()
    {
        List<string> keys = new List<string>(itemInventory.Keys);

        foreach (string key in keys)
        {
            itemInventory[key] = 0;
        }
    }

    void SimulateTurn()
    {
        Debug.Log($"--- Turn {turn + 1} ---");

        // 푸아송 샘플링: 적 등장 수
        int enemyCount = SamplePoisson(poissonLambda);
        totalEnemyCount += enemyCount;
        Debug.Log($"적 등장 : {enemyCount}");

        for (int i = 0; i < enemyCount; i++)
        {
            // 이항 샘플링: 명중 횟수
            int hits = SampleBinomial(maxHitsPerTurn, hitRate);
            totalHitRate = hits / (float)maxHitsPerTurn;
            float totalDamage = 0f;
            int criticalHits = 0;

            maxDamageDealt = 0f;
            minDamageDealt = float.MaxValue;

            for (int j = 0; j < hits; j++)
            {
                float damage = SampleNormal(meanDamage, stdDevDamage);
                if (damage > maxDamageDealt) maxDamageDealt = damage;
                if (damage < minDamageDealt) minDamageDealt = damage;

                // 베르누이 분포 샘플링: 확률 기반 치명타 발생
                if (Random.value < critChance)
                {
                    damage *= critDamageRate;
                    criticalHits++;
                    Debug.Log($" 크리티컬 hit! {damage:F1}");
                }
                else
                    Debug.Log($" 일반 hit! {damage:F1}");

                totalDamage += damage;
            }

            totalCritRare = criticalHits / (float)hits;

            if (totalDamage >= enemyHP)
            {
                Debug.Log($"적 {i + 1} 처치! (데미지: {totalDamage:F1})");
                totalKillCount ++;

                // 균등 분포 샘플링: 보상 결정
                string reward = rewards[UnityEngine.Random.Range(0, rewards.Length)];
                Debug.Log($"보상: {reward}");
                itemInventory[reward]++;

                if (reward == "Weapon" && Random.value < 0.2f)
                {
                    rareItemObtained = true;
                    itemInventory[RARE_PREFIX+reward]++;
                    Debug.Log("레어 무기 획득!");
                }
                else if (reward == "Armor" && Random.value < 0.2f)
                {
                    rareItemObtained = true;
                    itemInventory[RARE_PREFIX + reward]++;
                    Debug.Log("레어 방어구 획득");
                }
                else
                {
                    rareItemChance += 0.05f; // 레어 아이템 획득 확률 5% 증가
                    itemInventory[reward]++;
                }
            }
        }
    }

    // --- 분포 샘플 함수들 ---
    int SamplePoisson(float lambda)
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

    int SampleBinomial(int n, float p)
    {
        int success = 0;
        for (int i = 0; i < n; i++)
            if (Random.value < p) success++;
        return success;
    }

    float SampleNormal(float mean, float stdDev)
    {
        float u1 = Random.value;
        float u2 = Random.value;
        float z = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Cos(2.0f * Mathf.PI * u2);
        return mean + stdDev * z;
    }

    private void UpdateUI()
    {
        combatDataText.text = $"[전투 결과]" +
            $"총 턴 수: {turn + 1}\n발생한 적 : {totalEnemyCount}\n처치한 적 : {totalKillCount}\n" +
            $"공격 명중 확률 : {totalHitRate * 100f:F3}%\n치명타 확률 : {totalCritRare * 100f:F3}%\n최대~최소 대미지 : {maxDamageDealt:F1}~{minDamageDealt:F1}";
        
        itemInventoryText .text = $"[흭득한 아이템]" +
            $"포션 : {itemInventory["Potion"]}개 \n골드 : {itemInventory["Gold"]}개\n" +
            $"무기 : {itemInventory["Weapon"]}개\n무기(레어) : {itemInventory["Rare_Weapon"]}개\n방어구 : {itemInventory["Armor"]}개\n방어구(레어) : {itemInventory["Rare_Armor"]}개";

    }
}