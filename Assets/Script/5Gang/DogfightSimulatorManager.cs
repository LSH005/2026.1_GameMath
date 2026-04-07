using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DogfightSimulatorManager : MonoBehaviour
{
    public DogfightEnemy enemy;
    [Header("크리티컬")]
    public TextMeshProUGUI critText;
    public float targetCritRate = 0.3f;
    [Header("전리품 확률")]
    public TextMeshProUGUI lootPercentText;
    [Header("인벤토리")]
    public TextMeshProUGUI inventoryText;

    bool isAutoAttacking;
    int totalHit = 0;
    int totalCrit = 0;

    readonly Dictionary<string, float> defaultLootTable = new()
    {
        { "일반", 50f }, { "고급", 30f }, { "희귀", 15f }, { "전설", 5f }
    };

    Dictionary<string, float> lootTable = new();

    Dictionary<string, int> inventory = new()
    {
        { "일반", 0 }, { "고급", 0 }, { "희귀", 0 }, { "전설", 0 }
    };


    private void Awake()
    {
        enemy.manager = this;
    }

    private void Start()
    {
        enemy.SpawnNewEnemy();
        ResetLootTable();
        ReloadText();
    }

    private void Update()
    {
        if (isAutoAttacking)
        {
            for (int i = 0; i < 10; i++)
            {
                Attack();
            }
        }
    }

    public void Attack()
    {
        int damage = 30;
        totalHit++;
        if (RollCrit())
        {
            damage *= 2;
            totalCrit++;
        }

        enemy.GetDamage(damage);
        ReloadText();
    }

    public void InstaKill()
    {
        totalHit++;
        if (RollCrit()) totalCrit++;

        enemy.GetDamage(int.MaxValue);
        ReloadText();
    }

    public void ToggleAutoAttack() => isAutoAttacking = !isAutoAttacking;

    public void DropLoot()
    {
        string key = GetRandomKey(lootTable);
        inventory[key]++;

        if (key == "전설") ResetLootTable();
        else BuffLegendaryRate();

        ReloadText();
    }

    void ReloadText()
    {
        critText.text = $"전체 공격 횟수 : {totalHit}\n전체 치명타 횟수 : {totalCrit}\n" +
            $"목표 치명타 확률 : {((float)targetCritRate * 100):F3}%\n실제 치명타 확률 : {((float)totalCrit / totalHit * 100):F3}%";

        inventoryText.text = $"현재 소유한 아이템\n" +
            $"일반 : {inventory["일반"]}\n고급 : {inventory["고급"]}\n희귀 : {inventory["희귀"]}\n전설 : {inventory["전설"]}";

        lootPercentText.text = $"현재 아이템 확률\n" +
                $"일반 : {lootTable["일반"]:F1}%\n고급 : {lootTable["고급"]:F1}%\n희귀 : {lootTable["희귀"]:F1}%\n전설 : {lootTable["전설"]:F1}%";
    }

    bool RollCrit()
    {
        float currentRate = 0f;
        if (totalCrit > 0) currentRate = (float)totalCrit / totalHit;

        if (currentRate < targetCritRate && (float)(totalCrit + 1) / totalHit <= targetCritRate) return true;
        if (currentRate > targetCritRate && (float)(totalCrit) / totalHit >= targetCritRate) return false;

        return Random.value < targetCritRate;
    }

    string GetRandomKey(Dictionary<string, float> table)
    {
        float total = table.Values.Sum();
        float roll = Random.Range(0, total);
        float accumulator = 0;

        foreach (KeyValuePair<string, float> pair in table)
        {
            accumulator += pair.Value;
            if (roll <= accumulator) return pair.Key;
        }

        return string.Empty;
    }

    void ResetLootTable()
    {
        lootTable.Clear();

        foreach (var item in defaultLootTable)
        {
            lootTable.Add(item.Key, item.Value);
        }
    }

    public void BuffLegendaryRate()
    {
        string buffKey = "전설";
        string[] nerfKeys = { "일반", "고급", "희귀" };
        foreach (var key in nerfKeys)
        {
            if (lootTable[key] > 0.25f)
            {
                lootTable[key] -= 0.5f;
                lootTable[buffKey] += 0.5f;
            }
        }
    }
}
