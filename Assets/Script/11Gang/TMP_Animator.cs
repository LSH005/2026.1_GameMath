using System.Collections;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TMP_Animator : MonoBehaviour
{
    private TextMeshProUGUI tmpText;

    private Coroutine colorChangeCoroutine;
    private Coroutine sizeChangeCoroutine;
    private Coroutine textChangeCoroutine;

    void Awake()
    {
        tmpText = GetComponent<TextMeshProUGUI>();
        if (tmpText == null)
        {
            Debug.LogError($"TextMeshProUGUI 없음 : [{gameObject.name}]");
            enabled = false;
        }
    }

    public void SetColor(Color targetColor, float duration = 0)
    {
        if (colorChangeCoroutine != null)
        {
            StopCoroutine(colorChangeCoroutine);
        }

        colorChangeCoroutine = StartCoroutine(ChangeColorCoroutine(targetColor, duration));
    }

    IEnumerator ChangeColorCoroutine(Color targetColor, float duration)
    {
        if (duration > 0)
        {
            Color startColor = tmpText.color;
            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                tmpText.color = Color.Lerp(startColor, targetColor, timeElapsed / duration);

                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }

        tmpText.color = targetColor;
        colorChangeCoroutine = null;
    }

    public void SetSize(float targetSize, float duration = 0)
    {
        if (sizeChangeCoroutine != null)
        {
            StopCoroutine(sizeChangeCoroutine);
        }

        sizeChangeCoroutine = StartCoroutine(ChangeSizeCoroutine(targetSize, duration));
    }

    public float GetSize() => tmpText.fontSize;

    IEnumerator ChangeSizeCoroutine(float targetSize, float duration)
    {
        if (duration > 0)
        {
            float startSize = tmpText.fontSize;
            float timeElapsed = 0f;

            while (timeElapsed < duration)
            {
                tmpText.fontSize = Mathf.Lerp(startSize, targetSize, timeElapsed / duration);

                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }

        tmpText.fontSize = targetSize;
        sizeChangeCoroutine = null;
    }

    public void SetText(string newText, bool textType = false, float textInterval = 0.05f)
    {
        if (textChangeCoroutine != null) StopCoroutine(textChangeCoroutine);

        if (!textType) tmpText.text = newText;
        else StartCoroutine(TextTypeCoroutine(newText, textInterval));
    }

    IEnumerator TextTypeCoroutine(string newText, float textInterval)
    {
        string currentText = string.Empty;

        for (int i = 0; i < newText.Length; i++)
        {
            currentText += newText[i];
            tmpText.text = currentText;
            yield return new WaitForSeconds(textInterval);
        }

        textChangeCoroutine = null;
    }

    public void AddText(string text)
    {
        tmpText.text += text;
    }
}
