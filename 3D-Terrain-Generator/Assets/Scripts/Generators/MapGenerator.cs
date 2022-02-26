using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("Generator Configs")]
    [SerializeField] DrawMode drawMode;
    [SerializeField] bool autoUpdate;

    [Header("Scriptable Objects")]
    [SerializeField] ScriptableMap mapData;
    [SerializeField] ScriptableBiome biomeData;

    [Header("Map Properties")]
    [SerializeField] [Range(1, 100)] int mapWidth;  // X
    [SerializeField] [Range(1, 100)] int mapHeight; // Z
    [SerializeField] float noiseScale;

    [SerializeField] [Range(0, 10)] int numberOfOctaves;
    [SerializeField] [Range(0, 1)] float persistance;
    [SerializeField] float lacunarity;

    [SerializeField] int seed;
    [SerializeField] Vector2 offset;

    [SerializeField] float meshHeightMultiplier;
    [SerializeField] AnimationCurve meshHeightCurve;
    
    [SerializeField] TerrainType[] terrainTypes;

    [Header("Map Entities")]
    [SerializeField] float minHeightForSpawnEntity;
    [SerializeField] float maxHeightForSpawnEntity;
    [SerializeField] [Range(0, 100)] int possibilityOfSpawn;

    [Header("Generate/Load/Save Map")]
    [SerializeField] string mapName;
    [SerializeField] string biomeName;

    private int mapInstanceID;
    private List<Vector3> entitiesPositionList = new List<Vector3>();
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
                MeshData meshData = MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve);
                display.DrawMesh(meshData, TextureGenerator.TextureFromColorMap(colorMap, mapWidth, mapHeight));
                entitiesPositionList = SpawnGenerator.PlaceEntities(meshData, minHeightForSpawnEntity, maxHeightForSpawnEntity, seed, possibilityOfSpawn);
                break;
            default:
                break;
        }

    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < entitiesPositionList.Count; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(new Vector3(entitiesPositionList[i].x * 10, entitiesPositionList[i].y, entitiesPositionList[i].z * 10), 1);
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

            seed = mapData.Seed;
            offset = mapData.Offset;

            meshHeightMultiplier = mapData.MeshHeightMultiplier;
            meshHeightCurve = mapData.MeshHeightCurve;

            if (mapData.Biome != null)
            {
                biomeName = mapData.Biome.name;
                terrainTypes = mapData.Biome.TerrainConfigurations;

                biomeData = Instantiate(mapData.Biome);
            }
            
        }
        else
        {
            print("No Map found! Please select the map you want to load.");
        }
        
    }

    private void OnValidate()
    {
        if (lacunarity < 1)
        {
            lacunarity = 1;
        }
        if (numberOfOctaves < 0)
        {
            numberOfOctaves = 0;
        }
        if (mapData && mapData.GetInstanceID() != mapInstanceID)
        {
            LoadMapData();
            GenerateMap();
            mapInstanceID = mapData.GetInstanceID();
        }
        if (biomeData)
        {
            terrainTypes = biomeData.TerrainConfigurations;
        }
        if (minHeightForSpawnEntity <= 0)
        {
            minHeightForSpawnEntity = 0;
        }
        if (maxHeightForSpawnEntity > meshHeightMultiplier)
        {
            maxHeightForSpawnEntity = meshHeightMultiplier;
        }
    }

    public bool GetAutoUpdate()
    {
        return autoUpdate;
    }
}
