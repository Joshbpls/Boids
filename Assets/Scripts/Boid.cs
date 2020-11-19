using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boid : MonoBehaviour {
    private Vector3 sceneOrigin = new Vector3(0, 8, 0);
    private Settings settings;
    [HideInInspector] public Vector3 velocity;

    private void Awake() {
        settings = FindObjectOfType<Settings>();
    }

    void Start() {
        velocity = transform.forward * settings.initialSpeed;
    }

    void Update() {
        var neighbors = GetNeighboringBoids(settings.searchRadius);
        var closeNeighbors = GetNeighboringBoids(settings.separationRadius);
        if (neighbors.Length > 0) {
            var averagePosition = GetAveragePosition(settings.searchRadius).normalized;
            var averageVelocity = GetAverageVelocity(settings.searchRadius).normalized;
            var averageCloseByPosition = GetAveragePosition(settings.separationRadius).normalized;
            var separation = (transform.position - averageCloseByPosition).normalized;

            velocity = Vector3.Lerp(velocity, SteerTowards(averagePosition), settings.cohesionWeight);
            velocity = Vector3.Lerp(velocity, SteerTowards(averageVelocity), settings.alignmentWeight);
            velocity = Vector3.Lerp(velocity, separation, settings.separationWeight * closeNeighbors.Length);
        }

        var normalized = velocity.normalized;
        velocity = normalized * settings.maxSpeed;
        var ray = new Ray(transform.position, transform.forward);
        if (Physics.SphereCast(ray, 0.75f, out var hit, 2)) {
            var t = (settings.collisionMultiplier * Time.deltaTime) / hit.distance;
            velocity = Vector3.Lerp(velocity, (sceneOrigin - transform.position), t);
        }

        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity / velocity.magnitude;
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