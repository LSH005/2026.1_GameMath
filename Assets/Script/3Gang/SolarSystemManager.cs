using UnityEngine;

public class SolarSystemManager : MonoBehaviour
{
    public static SolarSystemManager Instance;
    public float Speed = 1.0f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.W)) Speed += Time.deltaTime;
    //    else if (Input.GetKey(KeyCode.S)) Speed -= Time.deltaTime;
    //}
}
