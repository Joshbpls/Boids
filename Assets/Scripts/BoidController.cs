using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class BoidController : MonoBehaviour
{
    private const float SEARCH_RADIUS = 25;
    public float speed;
    public float separation;
    private BoidManager manager;
    private Vector3 velocity = new Vector3(0, 0, 0);
    private Vector3 acceleration = new Vector3(0, 0, 0);

    void Start()
    {
        transform.rotation = Random.rotation;
        manager = FindObjectOfType<BoidManager>();
    }
    
    void Update()
    {
        var nearby = manager.GetNearByBoids(transform.position, SEARCH_RADIUS);
        acceleration += PointTowardsFlock(nearby);
        acceleration += GetSeparationVector(nearby);
        ApplyCalculatedVelocity();
    }

    private void ApplyCalculatedVelocity()
    {
        velocity += acceleration;
        AddToPosition(velocity);
        acceleration *= 0;
    }

    private void AddToPosition(Vector3 vel)
    {
        var position = transform.position;
        var x = position.x + vel.x;
        var y = position.y + vel.y;
        var z = position.z + vel.z;
        transform.position = new Vector3(x, y, z);

    }

    private Vector3 PointTowards(Vector3 destination)
    {
        var direction = destination - transform.position;
        direction.Normalize();
        direction *= speed;
        var point = direction - velocity;
        return Vector3.ClampMagnitude(point, speed);
    }

    private Vector3 GetSeparationVector(List<GameObject> nearbyBoids)
    {
        var vector = new Vector3(0, 0, 0);
        foreach (var boid in nearbyBoids)
        {
            var distance = Vector3.Distance(transform.position, boid.transform.position);
            if (distance > 0 && distance < separation)
            {
                var difference = transform.position - boid.transform.position;
                difference.Normalize();
                difference /= distance;
                vector += difference;
            }
        }

        if (nearbyBoids.Count > 0)
        {
            vector /= nearbyBoids.Count;
        }

        if (vector.magnitude > 0)
        {
            vector.Normalize();
            vector *= speed;
            vector -= velocity;
            vector = Vector3.ClampMagnitude(vector, speed);

        }

        return vector;
    }

    /**
     * Creates a vector that points towards the center of a flock of boids.
     */
    private Vector3 PointTowardsFlock(List<GameObject> nearbyBoids)
    {
        var sum = new Vector3(0, 0, 0);
        if (nearbyBoids.Count <= 0)
        {
            return sum;
        }
        foreach (var position in nearbyBoids.Select(boid => boid.transform.position))
        {
            sum.x += position.x;
            sum.y += position.y;
            sum.z += position.z;
        }
        sum /= nearbyBoids.Count;
        return PointTowards(sum);
    }
}
