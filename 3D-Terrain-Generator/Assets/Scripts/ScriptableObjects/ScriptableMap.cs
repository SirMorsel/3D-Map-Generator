using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Map", menuName = "ScriptableObjects/Map")]
public class ScriptableMap : ScriptableObject
{
    public string MapName = "My new Map";
    public int MapWidth;
    public int MapHeight;
    public float NoiseScale;

    public int NumberOfOctaves;
    public float Persistance;
    public float Lacunarity;

    public int Seed;
    public Vector2 Offset;

    public float MeshHeightMultiplier;
    public AnimationCurve MeshHeightCurve;

    public ScriptableBiome Biome;
    
}
