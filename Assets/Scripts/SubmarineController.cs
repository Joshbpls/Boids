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
            body.velocity += new Vector3(0, 0.01f, 0);
        } 
        if (Input.GetKey(KeyCode.S)) {
            body.velocity += new Vector3(0, -0.01f, 0);
        }
    }

    private void HandleRotation() {
        if (Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.up, 0.5f);
        }
        if (Input.GetKey(KeyCode.D)) {
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
