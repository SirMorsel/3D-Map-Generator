using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] DrawMode drawMode;

    [SerializeField] ScriptableMap mapData;
    [SerializeField] ScriptableBiome biomeData;

    [SerializeField] string mapName;
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

    [SerializeField] string biomeName;
    [SerializeField] TerrainType[] terrainTypes;

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
                    if (currentHeight <= terrainTypes[i].Height)
                    {
                        colorMap[y * mapWidth + x] = terrainTypes[i].Color;
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

    public ScriptableMap PrepareMapDataForSave()
    {
        ScriptableMap tempMapData = new ScriptableMap();
        tempMapData.MapName = mapName;
        tempMapData.MapWidth = mapWidth;
        tempMapData.MapHeight = mapHeight;
        tempMapData.NoiseScale = noiseScale;
        tempMapData.NumberOfOctaves = numberOfOctaves;
        tempMapData.Persistance = persistance;
        tempMapData.Lacunarity = lacunarity;
        tempMapData.Seed = seed;
        tempMapData.Offset = offset;
        tempMapData.MeshHeightMultiplier = meshHeightMultiplier;
        tempMapData.MeshHeightCurve = meshHeightCurve;


        return tempMapData;
    }

    public string GetBiomeNameFromEditor()
    {
        return biomeName;
    }
    public TerrainType[] GetBiomeData()
    {
        return terrainTypes;
    }

    public void LoadMapData()
    {
        if (mapData != null)
        {
            mapName = mapData.MapName;

            mapWidth = mapData.MapWidth;
            mapHeight = mapData.MapHeight;

            noiseScale = mapData.NoiseScale;
            numberOfOctaves = mapData.NumberOfOctaves;
            persistance = mapData.Persistance;
            lacunarity = mapData.Lacunarity;

            seed = mapData.Seed = seed;

            offset = mapData.Offset = offset;

            meshHeightMultiplier = mapData.MeshHeightMultiplier;
            meshHeightCurve = mapData.MeshHeightCurve;

            biomeName = mapData.Biome.name;
            terrainTypes = mapData.Biome.TerrainConfigurations;

            biomeData = mapData.Biome;
        }
        else
        {
            print("No Map found! Please select the map you want to load.");
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
    /*
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
        }

        public Color GetTerrainColor()
        {
            return color;
        }
    }*/
}
