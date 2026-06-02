using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public int width = 30;
    public int depth = 30;

    public int minHight = 1;
    public int maxHight = 8;

    public GameObject cubePrefab;

    void Start()
    {
        TerrainGen();
    }

    void TerrainGen()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                int hight = Random.Range(minHight, maxHight+1);
                for (int y = 0; y < hight; y++)
                {
                    Vector3 pos = new Vector3(x, y, z);
                    Instantiate(cubePrefab, pos, Quaternion.identity).transform.SetParent(this.transform);
                }
            }
        }
    }
}
