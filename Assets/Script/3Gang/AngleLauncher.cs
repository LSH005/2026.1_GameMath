using TMPro;
using UnityEngine;

public class AngleLauncher : MonoBehaviour
{
    public TMP_InputField input;
    public CoolBall ballPrefab;
    public Transform firePoint;
    public float force = 15f;

    public void FireThatBall()
    {
        CoolBall ball = Instantiate(ballPrefab, firePoint.position, Quaternion.identity);
        ball.power = force;
        ball.angle = float.Parse(input.text);
    }
    
}
