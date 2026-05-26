using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkill : MonoBehaviour
{
    [Header("Skill 1")]
    public BouncyBomb bomb1;
    [Header("Skill 2")]
    public StickyBomb bomb2;

    public void OnClick(InputValue value)
    {
        if (value.isPressed) Skill1Bomb();
    }

    public void OnRightClick(InputValue value)
    {
        if (value.isPressed) Skill2Bomb();
    }

    void Skill1Bomb()
    {
        BouncyBomb newBomb = Instantiate(bomb1, transform.position + (Vector3.up * 2f), Quaternion.identity);
        newBomb.velocity = (transform.forward + (-transform.up)).normalized * 12;
    }

    void Skill2Bomb()
    {
        StickyBomb newBomb = Instantiate(bomb2, transform.position + transform.forward, transform.rotation);
    }

}
