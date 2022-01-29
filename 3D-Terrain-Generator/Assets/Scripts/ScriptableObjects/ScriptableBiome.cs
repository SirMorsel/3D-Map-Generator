using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "ScriptableObjects/Biome")]
public class ScriptableBiome : ScriptableObject
{
    public TerrainType[] TerrainConfigurations;
/*
    [System.Serializable]
    public struct TerrainType
    {
        public string TerrainTypeName;
        public float Heights;
        public Color Color;
    }*/
}
