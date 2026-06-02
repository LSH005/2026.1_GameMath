using UnityEngine;

public class PerlinTerrain : MonoBehaviour
{
    public int width = 30;
    public int depth = 30;
    public float scale = 0.1f;

    public int maxHight = 8;
    public int addHight = 2;
    public int waterHight = 3;

    public GameObject dirtCubePrefab;
    public GameObject waterCubePrefab;
    public GameObject grassCubePrefab;
    public GameObject gravelCubePrefab;

    float xOffset = 0f;
    float yOffset = 0f;

    enum CubeType
    {
        Dirt,
        Water,
        Grass,
        Gravel
    }

    void Start()
    {
        xOffset = Random.Range(-9999f, 9999f);
        yOffset = Random.Range(-9999f, 9999f);
        TerrainGen();
    }

    void TerrainGen()
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < depth; z++)
            {
                float perlinValue = ThatPerlin.Noise((x + xOffset) * scale, (z + yOffset) * scale);
                int hight = Mathf.FloorToInt(perlinValue * maxHight + addHight);
                int waterHight = this.waterHight + addHight;
                if (hight < 0) hight = 0;

                if (hight >= waterHight)
                {
                    for (int y = 0; y < hight - 1; y++)
                    {
                        GenCube(x, y, z, CubeType.Dirt);
                    }

                    GenCube(x, hight - 1, z, CubeType.Grass);
                }
                else
                {
                    for (int y = 0; y < hight - 1; y++)
                    {
                        GenCube(x, y, z, CubeType.Dirt);
                    }

                    GenCube(x, hight - 1, z, CubeType.Gravel);

                    for (int y = hight; y < waterHight; y++)
                    {
                        GenCube(x, y, z, CubeType.Water);
                    }
                }
            }
        }
    }

    void GenCube(int x, int y, int z, CubeType type)
    {
        GameObject cubePrefab = null;
        switch (type)
        {
            case CubeType.Dirt:
                cubePrefab = dirtCubePrefab;
                break;
            case CubeType.Water:
                cubePrefab = waterCubePrefab;
                break;
            case CubeType.Grass:
                cubePrefab = grassCubePrefab;
                break;
            case CubeType.Gravel:
                cubePrefab = gravelCubePrefab;
                break;
        }

        Vector3 pos = new Vector3(x, y, z);
        Instantiate(cubePrefab, pos, Quaternion.identity).transform.SetParent(this.transform);
    }
}
