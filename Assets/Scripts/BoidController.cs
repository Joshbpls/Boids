using System;
using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class BoidController : MonoBehaviour
{
    private BoidManager manager;
    private Vector3 velocity;
    private Transform cachedTransform;

    private void Awake()
    {
        manager = FindObjectOfType<BoidManager>();
        cachedTransform = transform;
    }

    void Start()
    {
        
    }

    private void OnDrawGizmos()
    {
        var nearby = GetBoidsInRadius();
        var averagePosition = manager.GetAverageLocation(nearby);
        foreach (var boid in nearby)
        {
            var separate = CreateSeparateVector(boid.transform.position);
            var align = TurnTowards(averagePosition);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, separate * 10);
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, align * 10);
        }
    }

    void Update()
    {
        var acceleration = Vector3.zero;
        var neighbors = GetBoidsInRadius();
        if (neighbors.Count > 0)
        {
            var averagePosition = manager.GetAverageLocation(neighbors);
            var averageVelocity = manager.GetAverageVelocity(neighbors);
            
            var separate = TurnTowards(transform.position - averagePosition) * manager.separationWeight;
            var align = TurnTowards(averageVelocity) * manager.alignmentWeight;
            var cohesion = TurnTowards(averagePosition) * manager.cohesionWeight;
            
            acceleration += separate;
            acceleration += align;
            acceleration += cohesion;

            var avoidance = GetObjectAvoidanceVector() * 20;
            if (avoidance != null)
            {
                acceleration += Vector3.ClampMagnitude((Vector3) avoidance, manager.maxForce);
            }
        }
        velocity += acceleration * Time.deltaTime;
    }

    Vector3 TurnTowards(Vector3 vector)
    {
        var turn = vector.normalized * manager.maxSpeed - velocity;
        return Vector3.ClampMagnitude(turn, manager.maxForce);
    }

    Vector3 CreateSeparateVector(Vector3 position)
    {
        return transform.position - position;
    }

    List<GameObject> GetBoidsInRadius()
    {
        return manager.boids.FindAll(IsNeighbor);
    }

    private Vector3? GetObjectAvoidanceVector()
    {
        if(Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out var hit, 10))
        {
            return (transform.position - hit.point) / hit.distance;
        }

        return null;
    }

    private Boolean IsNeighbor(GameObject otherBoid)
    {
        var distance = Vector3.Distance(transform.position, otherBoid.transform.position);
        return distance <= manager.neighborRadius;
    }
}
