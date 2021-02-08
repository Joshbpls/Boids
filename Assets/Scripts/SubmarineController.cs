using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubmarineController : MonoBehaviour {

    private const int LEFT_MOUSE = 0;
    private const int RIGHT_MOUSE = 1;
    
    public float maxSpeed;
    public float thrust;
    private Rigidbody body;

    void Start() {
        body = GetComponent<Rigidbody>();
    }

    private void OnDrawGizmos() {
        var ray = new Ray(transform.position, transform.forward);
        Gizmos.DrawRay(ray);
    }

    void Update() {
        HandleVerticalMovement();
        HandleRotation();
        HandleMouseClick();
        var clamped = Vector3.ClampMagnitude(body.velocity, maxSpeed);
        body.velocity = clamped;
    }

    private void HandleVerticalMovement() {
        if (Input.GetKey(KeyCode.W)) {
            transform.Rotate(Vector3.right, -0.5f);
        } 
        if (Input.GetKey(KeyCode.S)) {
            transform.Rotate(Vector3.right, 0.5f);
        }
    }

    private void HandleRotation() {
        if (Input.GetKey(KeyCode.A)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                transform.Rotate(Vector3.forward, 0.5f);
            }            
            transform.Rotate(Vector3.up, 0.5f);
        }
        if (Input.GetKey(KeyCode.D)) {
            if (Input.GetKey(KeyCode.LeftShift)) {
                transform.Rotate(Vector3.forward, -0.5f);
            }                 
            transform.Rotate(Vector3.up, -0.5f);
        }
    }
    
    private void HandleMouseClick() {
        if (Input.GetMouseButton(LEFT_MOUSE)) {
            body.velocity += transform.forward * thrust;
        }
        if (Input.GetMouseButton(RIGHT_MOUSE)) {
            body.velocity += -transform.forward * thrust;
        }
    }
}
