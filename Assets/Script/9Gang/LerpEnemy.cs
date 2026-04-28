using UnityEngine;

public class LerpEnemy : MonoBehaviour
{
    public Transform[] Poses;
    public LerpGunManager manager;
    
    int hp = 10;
    Transform targetPos;
    Transform previousPos;
    float moveDuration;
    float t = 0;
    int currentPosIndex;

    void Start()
    {
        hp = Random.Range(1, 10);
        Shuffle(Poses);
        moveDuration = Random.Range(2.0f, 5.0f);

        targetPos = Poses[0];
        previousPos = Poses[Poses.Length - 1];
    }

    public void GetDamage()
    {
        hp--;
        if (hp <= 0)
        {
            if (manager != null) manager.KilledEnemy();
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        t += Time.deltaTime / moveDuration;
        print(t);
        if (t > 1f)
        {
            currentPosIndex++;
            currentPosIndex %= Poses.Length;
            previousPos = targetPos;
            targetPos = Poses[currentPosIndex];
            t = 0;
        }
        else
        {
            transform.position = ThatLerp(previousPos.position, targetPos.position, t);
        }
    }

    void Shuffle(Transform[] array)
    {
        int n = array.Length;
        for (int i = n - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            Transform temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    Vector3 ThatLerp(Vector3 start, Vector3 end, float t) => (1f - t) * start + t * end;
}
