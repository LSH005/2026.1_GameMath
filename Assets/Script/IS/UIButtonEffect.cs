using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonEffect : MonoBehaviour
{
    public float pressedScale = 0.9f;
    public float duration = 0.15f;
    public GameObject clickParticle;

    Button button;
    RectTransform rect;
    Vector3 originalScale;

    private void Awake()
    {
        button = GetComponent<Button>();
        rect = GetComponent<RectTransform>();
        originalScale = rect.localScale;
        button.onClick.AddListener(PlayButtonEffect);
    }

    public void PlayButtonEffect()
    {
        rect.DOKill();

        Sequence seq = DOTween.Sequence();
        seq.Append(rect.DOScale(originalScale * pressedScale, duration));
        seq.Append(rect.DOScale(originalScale, duration));

        PlayParticle();
    }

    public void PlayParticle()
    {
        if (clickParticle == null) return;

        clickParticle.SetActive(false);
        clickParticle.transform.position = transform.position;
        clickParticle.SetActive(true);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(PlayButtonEffect);
    }
}
