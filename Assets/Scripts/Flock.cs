using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour {

    [SerializeField] private FlockAgent agentPrefab;
    private List<FlockAgent> agents = new List<FlockAgent>();

    [SerializeField] private FlockBehavior behavior;
    [SerializeField] private bool debug = false;

    [Header("Configuration")]
    [Range(10, 500)]
    [SerializeField] private int startingCount = 250;
    const float AgentDensity = 0.08f;

    [Range(1f, 100f)]
    [SerializeField] private float driveFactor = 10f;
    [Range(1f, 100f)]
    [SerializeField] private float maxSpeed = 5f;
    [Range(1f, 10f)]
    [SerializeField] private float neighborRadius = 1.5f;
    [Range(0f, 1f)]
    [SerializeField] private float avoidanceRadiusMultiplier = 0.5f;


    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius{ get => squareAvoidanceRadius; }


    void Start() {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;

        for(int i=0; i<startingCount; ++i) {
            Vector3 position = Random.insideUnitCircle * startingCount * AgentDensity;
            Quaternion rotation = Quaternion.Euler(Vector3.forward * Random.Range(0f, 360f));
            FlockAgent agent = Instantiate(agentPrefab, position, rotation, transform);
            agent.name = "Agent " + i;
            agent.Initialize(this);
            agents.Add(agent);
        }

    }

    void Update() {
        foreach(FlockAgent agent in agents) {
            List<Transform> context = GetNearbyObjects(agent);
            if(debug) {
                agent.GetComponentInChildren<SpriteRenderer>().color = Color.Lerp(Color.white, Color.red, context.Count / 6f);
            }
            Vector2 move = behavior.CalculateMove(agent, context, this);
            move *= driveFactor;
            if(move.sqrMagnitude > squareMaxSpeed) {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);
        }
    }

    private List<Transform> GetNearbyObjects(FlockAgent agent) {
        List<Transform> context = new List<Transform>();
        Collider2D[] contextColliders = Physics2D.OverlapCircleAll(agent.transform.position, neighborRadius);
        foreach(Collider2D c in contextColliders) {
            if(c==agent.Collider){ continue; }
            context.Add(c.transform);
        }
        return context;
    }
}
