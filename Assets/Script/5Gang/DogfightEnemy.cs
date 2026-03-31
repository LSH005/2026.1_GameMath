using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DogfightEnemy : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public Sprite sprite;
        public string name = "Name";
        public int MaxHP = 300;
    }

    public EnemyType[] enemyType;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hpText;
    public Image image;

    [HideInInspector] public DogfightSimulatorManager manager;

    int maxHP;
    int hp;


    public void SpawnNewEnemy()
    {
        EnemyType type = enemyType[Random.Range(0, enemyType.Length)];
        image.sprite = type.sprite;
        nameText.text = type.name;
        hpText.text = $"({type.MaxHP}/{type.MaxHP})";
        hp = maxHP = type.MaxHP;
    }

    public void GetDamage(int value)
    {
        hp -= value;
        if (hp <= 0)
        {
            manager.DropLoot();
            SpawnNewEnemy();
            return;
        }
        hpText.text = $"({hp}/{maxHP})";
    }
}
