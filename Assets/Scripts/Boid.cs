using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boid : MonoBehaviour {
    private Settings settings;
    [HideInInspector] 
    public Vector3 velocity;

    private void Awake() {
        settings = FindObjectOfType<Settings>();
    }
    
    void Start() {
        velocity = transform.forward * settings.initialSpeed;
    }

    void Update() {
        var visibleNeighbors = GetNeighboringBoids(settings.searchRadius);
        if (visibleNeighbors.Length > 0) {
            ApplyForces();
        } else {
            SteerTowardsClosestNeighbor();
        }
        var normalized = velocity.normalized;
        velocity = normalized * settings.maxSpeed;
        var ray = new Ray(transform.position, transform.forward);
        if (Physics.SphereCast(ray, 0.5f, out var hit, 5)) {
            var clearPath = GetClearPath() * settings.maxSpeed;
            var t = settings.collisionMultiplier * Time.deltaTime * (1 / hit.distance);
            velocity = Vector3.Lerp(velocity, clearPath, t);
        }
        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity / velocity.magnitude;
    }

    private void ApplyForces() {
        var closeNeighbors = GetNeighboringBoids(settings.separationRadius);
        var averagePosition = GetAveragePosition(settings.searchRadius);
        var averageVelocity = GetAverageVelocity(settings.searchRadius).normalized;
        var averageCloseByPosition = GetAveragePosition(settings.separationRadius);

        velocity = Vector3.Lerp(velocity, SteerTowards(averagePosition), settings.cohesionWeight);
        velocity = Vector3.Lerp(velocity, SteerTowards(averageVelocity), settings.alignmentWeight);
        velocity = Vector3.Lerp(velocity, SteerAway(averageCloseByPosition), settings.separationWeight * closeNeighbors.Length);
    }

    private Vector3 GetAverageVelocity(float searchRadius) {
        var boids = GetNeighboringBoids(searchRadius);
        return boids.Aggregate(Vector3.zero, (current, boid) => current + boid.velocity) / boids.Length;
    }

    private Vector3 GetAveragePosition(float searchRadius) {
        var boids = GetNeighboringBoids(searchRadius);
        return boids.Aggregate(Vector3.zero, (current, boid) => current + boid.transform.position) / boids.Length;
    }

    private Vector3 SteerTowards(Vector3 point) {
        return (point - transform.position).normalized;
    }

    private Vector3 SteerAway(Vector3 point) {
        return (transform.position - point).normalized;
    }

    private void SteerTowardsClosestNeighbor() {
        var closest = GetClosestBoid();
        velocity = Vector3.Lerp(velocity, SteerTowards(closest.transform.position), 0.04f);
    }

    private Vector3 GetClearPath() {
        var pointAmount = 20;
        var phi = Math.PI * (3f - Math.Sqrt(5f));
        for (var i = 0; i < pointAmount; i++) {
            var y = 1 - i / (float) pointAmount * 2;
            var radius = Math.Sqrt(1 - y * y);
            var theta = phi * i;
            var x = Math.Cos(theta) * radius;
            var z = Math.Sin(theta) * radius;
            var direction = new Vector3((float) x, (float) (-1 * z), y);
            var transformedDirection = transform.TransformVector(direction);
            var ray = new Ray(transform.position, transformedDirection);
            if (!Physics.SphereCast(ray, 0.5f, 5)) {
                return transformedDirection;
            }
        }
        return transform.forward;
    }


    private Boid GetClosestBoid() {
        var boids = FindObjectsOfType<Boid>();
        if (boids.Length == 0) return null;
        var closest = boids[0];
        var distance = Vector3.Distance(transform.position, closest.transform.position);
        for (var i = 1; i < boids.Length; i++) {
            var currentBoid = boids[i];
            var dist = Vector3.Distance(transform.position, currentBoid.transform.position);
            if (dist > 0 && dist < distance) {
                closest = currentBoid;
                distance = dist;
            }
        }

        return closest;
    }

    private Boid[] GetNeighboringBoids(float radius) {
        var neighbors = new List<Boid>();
        var boids = FindObjectsOfType<Boid>();
        foreach (var boid in boids) {
            var distance = Vector3.Distance(transform.position, boid.transform.position);
            if (distance > 0 && distance <= radius) {
                neighbors.Add(boid);
            }
        }

        return neighbors.ToArray();
    }
}