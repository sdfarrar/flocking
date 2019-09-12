﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsGenerator : MonoBehaviour {

    [SerializeField] private Boid prefab;
    private List<Boid> boids = new List<Boid>();

    [Range(0, 255)]
    [SerializeField] private int total = 50;
    [SerializeField] private float density = 0.08f;
    [SerializeField] private float radius = 5f;

    [Header("Boid Settings")]
    [SerializeField] private float boidSpeed = 2f;
    [SerializeField] private float boidTurnSpeed = 1.25f;
    [SerializeField] private BoxCollider2D boundary;


    void Start() {
        for (int i = 0; i < total; ++i) {
            Vector3 position = Random.insideUnitCircle * total * density * radius;
            Quaternion rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
            Boid boid = Instantiate(prefab, position, rotation, transform);
            boid.name = "Boid " + i;
            boid.Initialize(boidSpeed, boidTurnSpeed);
            boid.GetComponent<ConfineToBounds>().Boundary = boundary;
            boids.Add(boid);
        }
    }
}
