using TMPro;
using UnityEngine;
using DG.Tweening;

public class ToastUI : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private RectTransform toastRect;
    private TextMeshProUGUI messageText;

    [SerializeField] private string testMessage = "Item Get!";
    [SerializeField] private float startY = 0f;
    [SerializeField] private float showY = 200f;
    [SerializeField] private float moveDuration = 0.2f;
    [SerializeField] private float showTime = 1.2f;

    private Sequence currentSequence;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        toastRect = GetComponent<RectTransform>();
        messageText = GetComponentInChildren<TextMeshProUGUI>();

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowTestMessage()
    {
        Show(testMessage);
    }

    public void Show(string message)
    {
        if (messageText == null)
            return;

        currentSequence?.Kill();

        messageText.text = message;

        canvasGroup.alpha = 0f;
        toastRect.anchoredPosition = new Vector2(
            toastRect.anchoredPosition.x,
            startY
        );

        currentSequence = DOTween.Sequence();

        currentSequence.Append(
            canvasGroup.DOFade(1f, moveDuration)
        );

        currentSequence.Join(
            toastRect.DOAnchorPosY(showY, moveDuration)
                .SetEase(Ease.OutQuad)
        );

        currentSequence.AppendInterval(showTime);

        currentSequence.Append(
            canvasGroup.DOFade(0f, moveDuration)
        );

        currentSequence.Join(
            toastRect.DOAnchorPosY(startY, moveDuration)
                .SetEase(Ease.InQuad)
        );
    }

    private void OnDestroy()
    {
        currentSequence?.Kill();
    }
}