using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour {

    private new Collider2D collider;
    public Collider2D Collider{ get => collider; }

    void Start() {
        collider = GetComponent<Collider2D>();
    }

    public void Move(Vector2 velocity) {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

}
