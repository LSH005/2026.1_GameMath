using TMPro;
using UnityEngine;

public class DiceSimulator : MonoBehaviour
{
    public int tryCount = 1;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI tryCountText;
    public TextMeshProUGUI[] Texts;


    int[] count = new int[6];
    int attempts = 0;

    private void Start()
    {
        ClearData();
        AddTryCount(0);
    }

    public void RollTheDice()
    {
        for (int i = 0; i < tryCount; i++)
        {
            int diceValue = Random.Range(0, 6);
            count[diceValue]++;
        }
        attempts += tryCount;
        SetText();
    }

    public void ClearData()
    {
        count = new int[6];
        attempts = 0;

        SetText();
    }

    public void SetText()
    {
        infoText.text = $"{attempts}회 시도함";

        for (int i = 0; i < count.Length; i++)
        {
            float percent = attempts > 0 ? (float)count[i] / attempts * 100f : 0;
            Texts[i].text = $"[{i + 1}] : {count[i]} ({percent:F5}%)";
        }
    }

    public void AddTryCount(int value)
    {
        tryCount += value;
        if (tryCount < 1) tryCount = 1;

        tryCountText.text = $"한 번에 {tryCount}회 시도함";
    }
}
