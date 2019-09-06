using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    [SerializeField] private FlockAgent agentPrefab;
    private List<FlockAgent> agents = new List<FlockAgent>();

    [SerializeField] private FlockBehavior behavior;

    [Range(10, 500)]
    [SerializeField] private int startingCount = 250;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    [SerializeField] private float driveFactor = 10f;
    [Range(1f, 100f)]
    [SerializeField] private float maxSpeed = 5f;
    [Range(1f, 10f)]
    [SerializeField] private float neighborRadius = 5f;
    [Range(0f, 1f)]
    [SerializeField] private float avoidanceRadiusMultiplier = 0.5f;


    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius{ get => squareAvoidanceRadius; }


    void Start() {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = squareNeighborRadius * squareNeighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for(int i=0; i<startingCount; ++i) {
            Vector3 position = Random.insideUnitCircle * startingCount * AgentDensity;
            Quaternion rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
            FlockAgent agent = Instantiate(agentPrefab, position, rotation, transform);
            agent.name = "Agent " + i;
            agents.Add(agent);
        }

    }

    void Update() {

    }
}
