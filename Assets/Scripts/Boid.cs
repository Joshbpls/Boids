using System;
using System.Collections;
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
            SteerTowardsClosetNeighbor();
        }
        AvoidCollisions();
        transform.position += velocity * Time.deltaTime;
        transform.forward = velocity / velocity.magnitude;
    }

    private void AvoidCollisions() {
        var normalized = velocity.normalized;
        velocity = normalized * settings.maxSpeed;
        var ray = new Ray(transform.position, transform.forward);
        if (Physics.SphereCast(ray, 0.75f, out var hit, 2)) {
            var t = settings.collisionMultiplier * Time.deltaTime / hit.distance;
            var tangent = GetTangent(hit.normal);
            velocity = Vector3.Lerp(velocity, tangent + hit.normal, t);
        }
    }

    private void ApplyForces() {
        var closeNeighbors = GetNeighboringBoids(settings.separationRadius);
        var averagePosition = GetAveragePosition(settings.searchRadius);
        var averageVelocity = GetAverageVelocity(settings.searchRadius).normalized;
        var averageCloseByPosition = GetAveragePosition(settings.separationRadius);
        var separation = (transform.position - averageCloseByPosition).normalized;
            
        velocity = Vector3.Lerp(velocity, SteerTowards(averagePosition), settings.cohesionWeight);
        velocity = Vector3.Lerp(velocity, SteerTowards(averageVelocity), settings.alignmentWeight);
        velocity = Vector3.Lerp(velocity, separation, settings.separationWeight * closeNeighbors.Length);
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

    private Vector3 GetTangent(Vector3 normal) {
        var a = Vector3.Cross(normal, Vector3.forward);
        var b = Vector3.Cross(normal, Vector3.up);
        return a.magnitude > b.magnitude ? a : b;
    }

    private void SteerTowardsClosetNeighbor() {
        var closest = GetClosestBoid();
        velocity = Vector3.Lerp(velocity, SteerTowards(closest.transform.position), 0.04f);
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