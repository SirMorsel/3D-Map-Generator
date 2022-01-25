using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] DrawMode drawMode;

    [SerializeField] int mapWidth;
    [SerializeField] int mapHeight;
    [SerializeField] float noiseScale;

    [SerializeField] int numberOfOctaves;
    [SerializeField] [Range(0, 1)] float persistance;
    [SerializeField] float lacunarity;

    [SerializeField] int seed;
    [SerializeField] Vector2 offset;

    [SerializeField] float meshHeightMultiplier;
    [SerializeField] AnimationCurve meshHeightCurve;

    [SerializeField] TerrainTypes[] terrainTypes;

    [SerializeField] bool autoUpdate;

    // Start is called before the first frame update
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, numberOfOctaves, persistance, lacunarity, offset);
        Color[] colorMap = new Color[mapWidth * mapHeight];
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < terrainTypes.Length; i++)
                {
                    if (currentHeight <= terrainTypes[i].GetTerrainHeight())
                    {
                        colorMap[y * mapWidth + x] = terrainTypes[i].GetTerrainColor();
                        break;
                    }
                }
            }
        }

        MapDisplay display = FindObjectOfType<MapDisplay>();
        switch (drawMode)
        {
            case DrawMode.NOISEMAP:
                display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
                break;
            case DrawMode.COLORMAP:
                display.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
                break;
            case DrawMode.MESH:
                display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve), TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
                break;
            default:
                break;
        }

    }

    private void OnValidate()
    {
        if (mapWidth < 1)
        {
            mapWidth = 1;
        }
        if (mapHeight < 1)
        {
            mapHeight = 1;
        }
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (numberOfOctaves < 0)
        {
            numberOfOctaves = 0;
        }
    }

    public bool GetAutoUpdate()
    {
        return autoUpdate;
    }

    [System.Serializable]
    public struct TerrainTypes
    {
        [SerializeField] private string terrainTypeName;
        [SerializeField] private float height;
        [SerializeField] private Color color;

        // [SerializeField] private ScriptableTerrainType terrainTypesData;
        public float GetTerrainHeight()
        {
            return height;
            // return terrainTypesData.Height;
        }

        public Color GetTerrainColor()
        {
            return color;
            // return terrainTypesData.TerrainColor;
        }
    }
}
