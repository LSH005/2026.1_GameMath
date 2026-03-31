using TMPro;
using UnityEngine;

public class CriticalRandom : MonoBehaviour
{
    public float targetCritRate = 0.1f;
    public TextMeshProUGUI totalHitText;
    public TextMeshProUGUI totalCritText;
    public TextMeshProUGUI currentCritPercentText;

    int totalHit = 0;
    int totalCrit = 0;

    public void Attack()
    {
        totalHit++;
        if (RollCrit()) totalCrit++;
        ResetText();
    }

    bool RollCrit()
    {
        float currentRate = 0f;
        if (totalCrit > 0) currentRate = (float)totalCrit / totalHit;

        if (currentRate < targetCritRate && (float)(totalCrit + 1) / totalHit <= targetCritRate) return true;
        if (currentRate > targetCritRate && (float)(totalCrit) / totalHit >= targetCritRate) return false;

        return Random.value < targetCritRate;
    }

    void ResetText()
    {
        totalHitText.text = $"공격 횟수 : {totalHit}";
        totalCritText.text = $"크리티컬 : {totalCrit}";
        currentCritPercentText.text = $"현재 크리티컬 확률 : {((float)totalCrit / totalHit * 100):F3}%";
    }
}
