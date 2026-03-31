using UnityEngine;

[System.Serializable]
public class ObjectGroup
{
    public Transform[] objects;
    public float posY = 0.0f;
    public float scaleY = 1.0f;
}

public class ObjectSetup : MonoBehaviour
{
    public ObjectGroup[] allObj;

    void Start()
    {
        foreach (var objArr in allObj)
        {
            foreach (var obj in objArr.objects)
            {
                obj.localScale = new Vector3(obj.localScale.x, objArr.scaleY, obj.localScale.z);
                obj.position = new Vector3(obj.position.x, objArr.posY, obj.position.z);
            }
        }
    }
}