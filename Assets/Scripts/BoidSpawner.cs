using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidSpawner : MonoBehaviour {
    
    public GameObject prefab;
    public float maxDeviation;
    public int amount;

    void Start() {
        for (var i = 0; i < amount; i++) {
            var position = CreateRandomPosition();
            SpawnBoid(position);
        }
    }

    private float GetValueInRange(float origin) {
        var min = origin - maxDeviation;
        var max = origin + maxDeviation;
        return Random.Range(min, max);
    }
    
    private Vector3 CreateRandomPosition() {
        var x = GetValueInRange(transform.position.x);
        var y = GetValueInRange(transform.position.y);
        var z = GetValueInRange(transform.position.z);
        return new Vector3(x, y, z);
    }

    private void SpawnBoid(Vector3 position) {
        Instantiate(prefab, position, Quaternion.identity);
    }
}
