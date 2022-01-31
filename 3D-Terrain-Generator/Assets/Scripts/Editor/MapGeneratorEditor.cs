using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MapGenerator))]
public class MapGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGenerator mapGenerator = (MapGenerator)target;

        if (DrawDefaultInspector())
        {
            if (mapGenerator.GetAutoUpdate())
            {
                mapGenerator.GenerateMap();
            }
        }
        if (GUILayout.Button("Generate"))
        {
            mapGenerator.GenerateMap();
        }
        if (GUILayout.Button("Save"))
        {
            SaveBiomeToScriptableObject(mapGenerator.GetBiomeNameFromEditor(), mapGenerator.GetBiomeData());
            SaveMapDataToScriptableObject(mapGenerator.PrepareMapDataForSave(), GetSavedBiome(mapGenerator.GetBiomeNameFromEditor()));
        }
        if (GUILayout.Button("Load"))
        {
            mapGenerator.LoadMapData();
            mapGenerator.GenerateMap();
        }
    }

    private void SaveMapDataToScriptableObject(ScriptableMap mapData, ScriptableBiome biomeData)
    {
        // create biome or return existing
        //SaveBiomeToScriptableObject(mapData);
        string path = $"Assets/Prefab-Data/Maps/{mapData.MapName}.asset";
        ScriptableMap newMap = new ScriptableMap();
        newMap = mapData;
        newMap.Biome = biomeData;
        AssetDatabase.CreateAsset(newMap, path); 
        AssetDatabase.SaveAssets();
    }

    private void SaveBiomeToScriptableObject(string biomeName, TerrainType[] biomeData) {
        Debug.Log(biomeData);
        string path = $"Assets/Prefab-Data/Biomes/{biomeName}.asset";
        ScriptableBiome newBiome = new ScriptableBiome();
        newBiome.TerrainConfigurations = biomeData;
        AssetDatabase.CreateAsset(newBiome, path);
        AssetDatabase.SaveAssets();

        
    }

    private ScriptableBiome GetSavedBiome(string biomeName)
    {
        string path = $"Assets/Prefab-Data/Biomes/{biomeName}.asset";

        return (ScriptableBiome)AssetDatabase.LoadAssetAtPath(path, typeof(ScriptableBiome));
    }
}
