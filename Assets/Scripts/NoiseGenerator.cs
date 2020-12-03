using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NoiseGenerator
{

    public static float[,] CreateNoiseMap(int width, int height, float scale, int octaves, float persistence, float lacunarity, int seed) {
        if (scale <= 0) {
            scale = 0.0001f;
        }
        var maxNoiseHeight = float.MinValue;
        var minNoiseHeight = float.MaxValue;
        var map = new float[width, height];
        var offsets = CreateOctaveOffsets(octaves, seed);
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                var amplitude = 1f;
                var freq = 1f;
                var noiseHeight = 0f;
                for (var i = 0; i < octaves; i++) {
                    var offset = offsets[i];
                    var noise = CreatePerlinNoise(x, y, offset, scale, freq);
                    noiseHeight += noise * amplitude;
                    amplitude *= persistence;
                    freq *= lacunarity;
                }
                if (noiseHeight > maxNoiseHeight) {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight) {
                    minNoiseHeight = noiseHeight;
                }
                map[x, y] = noiseHeight;
            }
        }
        NormalizeNoiseMap(map, width, height, minNoiseHeight, maxNoiseHeight);
        return map;
    }

    private static Vector2[] CreateOctaveOffsets(int octaves, int seed) {
        var offsets = new Vector2[octaves];
        var random = new System.Random(seed);
        for (var i = 0; i < octaves; i++) {
            float x = random.Next(-100000, 100000);
            float y = random.Next(-100000, 100000);
            var offset = new Vector2(x, y);
            offsets[i] = offset;
        }
        return offsets;
    }

    private static float CreatePerlinNoise(int x, int y, Vector2 offset, float scale, float freq) {
        var pointX = x / scale * freq + offset.x;
        var pointY = y / scale * freq + offset.y;
       return Mathf.PerlinNoise(pointX, pointY) * 2 - 1;
    }

    private static void NormalizeNoiseMap(float[,] noise, int width, int height, float min, float max) {
        for (var y = 0; y < height; y++) {
            for (var x = 0; x < width; x++) {
                noise[x, y] = Mathf.InverseLerp(min, max, noise[x, y]);
            }
        }
    }
}
