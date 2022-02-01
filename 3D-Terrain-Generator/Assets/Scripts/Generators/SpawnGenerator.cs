using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SpawnGenerator
{
    public static List<Vector3> PlaceEntities(MeshData meshData, float minHeight, float maxHeight, int seed, int possibilityForSpawn)
    {
        List<Vector3> entitiesPosition = new List<Vector3>();
        System.Random randomNumbereGenerator = new System.Random(seed);
        for (int i = 0; i < meshData.GetVertices().Length; i++)
        {
            if (meshData.GetVertices()[i].y > minHeight && meshData.GetVertices()[i].y < maxHeight)
            {
                if (randomNumbereGenerator.Next(0, 100) <= possibilityForSpawn)
                {
                    entitiesPosition.Add(meshData.GetVertices()[i]);
                    //Debug.Log($"Spawn entity at: {i}"); // For Debugging
                }
            }
        }
        return entitiesPosition;
    }
}
