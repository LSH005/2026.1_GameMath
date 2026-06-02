using DG.Tweening;
using TMPro;
using UnityEngine;

public class CoinUI : MonoBehaviour
{
    private TextMeshProUGUI coinText;

    [SerializeField] private int coin = 0;
    [SerializeField] private int testAmount = 100;
    [SerializeField] private float countDuration = 0.6f;

    private Tween countTween;

    private void Awake()
    {
        coinText = GetComponent<TextMeshProUGUI>();
        RefreshText(coin);
    }

    public void AddCoin()
    {
        AddCoin(testAmount);
    }

    public void AddCoin(int amount)
    {
        int startValue = coin;
        int targetValue = coin + amount;

        coin = targetValue;

        countTween?.Kill();

        countTween = DOVirtual.Int(
            startValue,
            targetValue,
            countDuration,
            RefreshText
        );
    }

    private void RefreshText(int value)
    {
        coinText.text = value.ToString();
    }

    private void OnDestroy()
    {
        countTween?.Kill();
    }
}
