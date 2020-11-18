using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidManager : MonoBehaviour
{
    public List<GameObject> boids = new List<GameObject>();
    public float maxSpeed;
    public float maxForce;
    [Range(0, 50)]
    public float alignmentWeight;
    [Range(0, 50)]
    public float separationWeight;
    [Range(0, 50)]
    public float cohesionWeight;
    public float neighborRadius;

    public Vector3 GetAverageLocation(List<GameObject> neighbors)
    {
        var average = new Vector3(0, 0, 0);
        foreach (var boid in neighbors)
        {
            average += boid.transform.position;
        }
        average /= neighbors.Count;
        return average;
    }

    public Vector3 GetAverageVelocity(List<GameObject> neighbors)
    {
        var average = new Vector3(0, 0, 0);
        foreach (var boid in neighbors)
        {
            
        }
        average /= neighbors.Count;
        return average;
    }
    
    public List<GameObject> GetBoids()
    {
        return boids;
    }

    public List<GameObject> GetBoidsInBoxCollider(BoxCollider boxCollider)
    {
        return boids.FindAll(obj => boxCollider.bounds.Contains(obj.transform.position)).ToList();
    }

    public GameObject GetBoidFromPosition(Vector3 position)
    {
        return boids.Find(obj => Vector3.Distance(position, obj.transform.position) <= 0.5);
    }
}
