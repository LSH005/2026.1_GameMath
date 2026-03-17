using UnityEngine;

public class SomeStupidScript : MonoBehaviour
{
    void Start()
    {
        float thatDegree = 45f;
        float d2r = DegToRad(thatDegree);
        float r2d = RadToDeg(d2r);

        print($"{d2r} {r2d}");
    }


    float RadToDeg(float rad) => rad * (180 / Mathf.PI);
    float DegToRad(float deg) => deg * (Mathf.PI / 180);

}
