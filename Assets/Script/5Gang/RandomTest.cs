using UnityEngine;

public class RandomTest : MonoBehaviour
{
    public bool doit = false;
    void Start()
    {

    }

    private void Update()
    {
        if (!doit) return;
        doit = false;
        UnityRandomSeed();
        //SystemRandomSeed();
    }

    void JustRandom()
    {
        float chance = Random.value;
        int dice = Random.Range(1, 7);
        int sysRandom = new System.Random().Next(1, 7);

        Debug.Log($"{chance} / {dice} / {sysRandom}");
    }

    void UnityRandomSeed()
    {
        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"{Random.Range(1, 7)}");
        }
        Random.InitState(0);
    }

    void SystemRandomSeed()
    {
        System.Random random = new System.Random(1234);
        for (int i = 0; i < 5; i++)
        {
            Debug.Log($"{random.Next(1, 7)}");
        }
    }
}
