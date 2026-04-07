using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageSimulator : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI logText;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI rangeText;

    int level = 1, attackCount, missCount, weaknessAttackCount, critCount;
    float totalDamage = 0, baseDamage = 20, stdDevMult, critRate, critMult;
    string weaponName;

    private void Start()
    {
        SetWeapon(0);
    }


    public void Attack()
    {
        attackCount++;
        float sd = baseDamage * stdDevMult;
        float normalDamage = GetNormalStdDev(baseDamage, sd);
        float finalDamage = normalDamage;
        string prefix = string.Empty;

        if (normalDamage < baseDamage - (sd * 2))
        {
            prefix = "<color=purple>[빗나감!]</color> ";
            missCount++;
            finalDamage = 0;
        }
        else
        {
            bool isWeaknessAttack = normalDamage > baseDamage + (sd * 2);
            bool isCritical = Random.value < critRate;

            if (isWeaknessAttack && isCritical)
            {
                prefix = "<color=red>[크리티컬 + 약점]</color> ";
                finalDamage *= critMult * 2f;
                critCount++;
                weaknessAttackCount++;
            }
            else if (isWeaknessAttack)
            {
                finalDamage *= 2f;
                prefix = "<color=red>[약점]</color> ";
                weaknessAttackCount++;
            }
            else if (isCritical)
            {
                finalDamage *= critMult;
                prefix = "<color=red>[크리티컬]</color> ";
                critCount++;
            }

            totalDamage += finalDamage;
        }

        logText.text = $"{prefix}대미지 : {finalDamage}";
        UpdateUI();
    }

    public void ManyAttack(int manyAttackCount)
    {
        if (manyAttackCount <= 0) return;

        float maxDamage = -Mathf.Infinity;
        int totalCritAttackCount = 0, totalWeaknessAttackCount = 0, totalMissCount = 0;

        for (int i = 0; i < manyAttackCount; i++)
        {
            float sd = baseDamage * stdDevMult;
            float normalDamage = GetNormalStdDev(baseDamage, sd);
            float finalDamage = normalDamage;

            if (normalDamage < baseDamage - (sd * 2))
            {
                missCount++;
                totalMissCount++;
                continue;
            }
            else
            {
                bool isWeaknessAttack = normalDamage > baseDamage + (sd * 2);
                bool isCritical = Random.value < critRate;

                if (isWeaknessAttack && isCritical)
                {
                    finalDamage *= critMult * 2f;
                    critCount++;
                    weaknessAttackCount++;
                    totalCritAttackCount++;
                    totalWeaknessAttackCount++;
                }
                else if (isWeaknessAttack)
                {
                    finalDamage *= 2f;
                    weaknessAttackCount++;
                    totalWeaknessAttackCount++;
                }
                else if (isCritical)
                {
                    finalDamage *= critMult;
                    critCount++;
                    totalCritAttackCount++;
                }
            }
            maxDamage = Mathf.Max(maxDamage, finalDamage);
            totalDamage += finalDamage;
        }

        attackCount += manyAttackCount;
        logText.text = $"[{manyAttackCount}번 연속 공격함]\n최대 {maxDamage}대미지 // 크리티컬 : {totalCritAttackCount}\n약점 공격 : {totalWeaknessAttackCount} // 빗나감 : {totalMissCount}";
        UpdateUI();
    }


    public void SetWeapon(int id)
    {
        ResetData();

        switch (id)
        {
            case 1:
                SetState("장검", 0.2f, 0.3f, 2.0f);
                break;
            case 2:
                SetState("도끼", 0.3f, 0.2f, 3.0f);
                break;
            default:
                SetState("단검", 0.1f, 0.4f, 1.5f);
                break;
        }

        logText.text = $"{weaponName} 장착됨";
        UpdateUI();
    }

    public void LevelUp()
    {
        totalDamage = attackCount = 0;
        level++;
        baseDamage = level * 20f;

        logText.text = $"레벨 업 : {level}레벨";
        UpdateUI();
    }

    void SetState(string _name, float _stdDev, float _critRate, float _critMult)
    {
        weaponName = _name;
        stdDevMult = _stdDev;
        critRate = _critRate;
        critMult = _critMult;
    }

    void ResetData()
    {
        totalDamage = attackCount = 0;
        level = 1;
        baseDamage = 20;
    }

    void UpdateUI()
    {
        statusText.text = $"레벨 : {level} // 무기 : {weaponName}\n기본 대미지 : {baseDamage} // 치명타 확률 : {critRate * 100}% (×{critMult})";
        rangeText.text = string.Format("예상 대미지 범위 : [{0:F3} ~ {1:F3}]", baseDamage - (3 * baseDamage * stdDevMult), baseDamage + (3 * baseDamage * stdDevMult));
        float dpa = attackCount > 0 ? totalDamage / attackCount : 0;
        resultText.text = $"누적 대미지 : {totalDamage} // 공격 횟수 : {attackCount}\nDPA 평균 : {dpa}\n빗나감 : {missCount} // 크리티컬 : {critCount} // 약점 : {weaknessAttackCount}";
    }

    float GetNormalStdDev(float mean, float stdDev)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return mean + stdDev * randStdNormal;
    }
}
