using UnityEngine;
using DG.Tweening;

public class DotweenTemp : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform popupBox;


    private void Awake()
    {
        ClosePanelInstante();
    }

    public void OpenPanel()
    {
        canvasGroup.DOKill();
        popupBox.DOKill();

        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = true;

        popupBox.localScale = Vector3.zero;
        canvasGroup.DOFade(1f, 0.25f);
        popupBox.DOScale(1f, 0.35f).SetEase(Ease.OutBack);
    }

    public void ClosePanel()
    {
        canvasGroup.DOKill();
        popupBox.DOKill();

        Sequence sequence = DOTween.Sequence();

        sequence.Append(popupBox.DOScale(0f, 0.2f).SetEase(Ease.InBack));
        sequence.Join(canvasGroup.DOFade(0f, 0.2f));

        sequence.OnComplete(() =>
        {
            canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
        });
    }

    void ClosePanelInstante()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = canvasGroup.blocksRaycasts = false;
        popupBox.localScale = Vector3.zero;
    }
}
