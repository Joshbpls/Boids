using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public List<GameObject> boids = new List<GameObject>();
    
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public List<GameObject> GetNearByBoids(Vector3 location, float searchDistance)
    {
        return boids
            .Where(boid => Vector3.Distance(location, boid.transform.position) <= searchDistance)
            .ToList();
    }

    public Vector3 GetDirectionTowards(GameObject subject, Vector3 target)
    {
        return (subject.transform.forward - target).normalized;
    }

    public Vector3 GetAveragePosition(List<GameObject> boids)
    {
        var x = 0f;
        var y = 0f;
        var z = 0f;
        foreach (var position in boids.Select(boid => boid.transform.position))
        {
            x += position.x;
            y += position.y;
            z += position.z;
        }

        return new Vector3(x / boids.Count, y / boids.Count, z / boids.Count);
    }
}
