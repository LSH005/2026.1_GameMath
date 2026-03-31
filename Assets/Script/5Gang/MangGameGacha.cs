using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class MangGameGacha : MonoBehaviour
{
    public TextMeshProUGUI[] Texts;
    Dictionary<string, float> badGradeTable = new()
    {
        { "C" ,4f},{ "B" ,3f},{ "A" ,2f},{ "S" ,1f }
    };
    Dictionary<string, float> niceGradeTable = new()
    {
        { "A" ,2f},{ "S" ,1f }
    };

    private void Start()
    {
        ResetText();
    }

    public void TrySingleGacha()
    {
        ResetText();
        Texts[0].text = $"Rank : [{GetRandomRank(badGradeTable)}]";
    }

    public void TryManyGacha()
    {
        for (int i = 0; i < Texts.Length - 1; i++)
        {
            Texts[i].text = $"Rank : [{GetRandomRank(badGradeTable)}]";
        }

        Texts[Texts.Length - 1].text = $"Rank : [{GetRandomRank(niceGradeTable)}]";
    }


    string GetRandomRank(Dictionary<string,float> table)
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

    private void ResetText()
    {
        foreach(var text in Texts)
        {
            text.text = "Rank : [...]";
        }
    }
}
