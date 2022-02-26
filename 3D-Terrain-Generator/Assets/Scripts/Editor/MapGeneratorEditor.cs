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
            mapGenerator.GenerateMap();
            mapGenerator.LoadMapData();
        }
    }

    private void SaveMapDataToScriptableObject(ScriptableMap mapData, ScriptableBiome biomeData)
    {
        string path = $"Assets/Prefab-Data/Maps/{mapData.MapName}.asset";
        ScriptableMap newMap = ObjectFactory.CreateInstance<ScriptableMap>();
        newMap = mapData;
        newMap.Biome = biomeData;
        AssetDatabase.CreateAsset(newMap, path); 
        AssetDatabase.SaveAssets();
    }

    private void SaveBiomeToScriptableObject(string biomeName, TerrainType[] biomeData) {
        string path = $"Assets/Prefab-Data/Biomes/{biomeName}.asset";
        ScriptableBiome newBiome = ObjectFactory.CreateInstance<ScriptableBiome>();
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
