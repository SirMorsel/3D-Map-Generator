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
            SaveMapDataToScriptableObject(mapGenerator.PrepareMapDataForSave());
        }
        if (GUILayout.Button("Load"))
        {
            mapGenerator.LoadMapData();
        }
    }

    private void SaveMapDataToScriptableObject(ScriptableMap mapData)
    {
        string path = $"Assets/Prefab-Data/Maps/{mapData.MapName}.asset";
        ScriptableMap newMap = new ScriptableMap();
        newMap = mapData;
        AssetDatabase.CreateAsset(newMap, path);
        AssetDatabase.SaveAssets();
    }
}
