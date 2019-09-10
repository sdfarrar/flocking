using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviors/Steered Cohesion")]
public class SteeredCohesionBehavior : FlockBehavior {

    [SerializeField] private float agentSmoothTime = 0.5f;

    Vector2 currentVelocity;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) {
        // If no neighbors, then no adjustment
        if (context.Count == 0) { return Vector2.zero; }

        // Otherwise average out position of neighbors
        Vector2 move = Vector2.zero;
        foreach (Transform item in context) {
            move += (Vector2)item.position;
        }
        move /= context.Count;

        // Create offset from agent position
        move -= (Vector2)agent.transform.position;
        move = Vector2.SmoothDamp(agent.transform.up, move, ref currentVelocity, agentSmoothTime);
        return move;
    }

}
