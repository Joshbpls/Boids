using UnityEngine;
using Random = UnityEngine.Random;

public class BoidSpawner : MonoBehaviour
{
    private BoidManager manager;
    public GameObject boidPrefab;
    public int count;

    private void Awake()
    {
        manager = FindObjectOfType<BoidManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            var boid = Instantiate(boidPrefab, CreateRandomPosition(), Quaternion.identity);
            manager.boids.Add(boid);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Vector3 CreateRandomPosition()
    {
        var position = transform.position;
        var x = position.x + Random.value * 10;
        var y = position.y + Random.value * 2;
        var z = position.z + Random.value * 10;
        return new Vector3(x, y, z);
    }
}
