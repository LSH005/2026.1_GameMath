using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    public float maxHP = 100;
    public float fillDuration = 0.25f;
    public float colorDuration = 0.1f;
    public Color normalColor;
    public Color damageColor;

    Image hpFillImage;
    float currentHP = 0;

    Tween fillTween;
    Sequence colorSequence;

    private void Awake()
    {
        hpFillImage = GetComponent<Image>();
    }

    private void Start()
    {
        currentHP = maxHP;

        hpFillImage.type = Image.Type.Filled;
        hpFillImage.fillMethod = Image.FillMethod.Horizontal;
        hpFillImage.color = normalColor;
        hpFillImage.fillAmount = currentHP / maxHP;

    }
    public void SetHP(float hp)
    {
        currentHP = Mathf.Clamp(hp, 0f, maxHP);

        float ratio = currentHP / maxHP;
        ratio = Mathf.Clamp01(ratio);

        fillTween?.Kill();

        fillTween = hpFillImage
            .DOFillAmount(ratio, fillDuration)
            .SetEase(Ease.OutQuad);
    }

    public void Heal()
    {
        SetHP(currentHP + (maxHP * 0.1f));
    }

    public void Damage()
    {
        SetHP(currentHP - (maxHP * 0.1f));
        PlayDamageEffect();
    }

    private void PlayDamageEffect()
    {
        colorSequence?.Kill();

        hpFillImage.color = normalColor;

        colorSequence = DOTween.Sequence();

        colorSequence.Append(
            hpFillImage.DOColor(damageColor, colorDuration)
        );

        colorSequence.Append(
            hpFillImage.DOColor(normalColor, colorDuration)
        );
    }

    private void OnDestroy()
    {
        fillTween?.Kill();
        colorSequence?.Kill();
    }
}
