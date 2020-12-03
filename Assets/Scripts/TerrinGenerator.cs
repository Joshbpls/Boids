using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrinGenerator : MonoBehaviour {
    
    public int width;
    public int height;
    public int heightIntensity;
    public float noiseScale;
    public int octaves;
    [Range(0, 1)]
    public float persistence;
    public float lacunarity;
    public int seed;
    public GameObject grassPrefab;

    void Start() {
        var terrain = GetComponent<Terrain>();
        terrain.terrainData = GenerateTerrainData(terrain.terrainData);
        AddGrassPatches(terrain.terrainData);
    }

    private TerrainData GenerateTerrainData(TerrainData data) {
        data.size = new Vector3(width, heightIntensity, height);
        data.heightmapResolution = width + 1;
        var noise = NoiseGenerator.CreateNoiseMap(width, height, noiseScale, octaves, persistence, lacunarity, seed);
        data.SetHeights(0, 0, noise);
        return data;
    }

    private void AddGrassPatches(TerrainData data) {
        var random = new System.Random();
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                var number = random.Next(0, 200);
                if (number == 10) {
                    var height = data.GetHeight(x, y);
                    var position = new Vector3(x, height, y);
                    Instantiate(grassPrefab, position, Quaternion.identity);
                } else if (number == 20) {
                    
                }
            }
        }
    }
}
