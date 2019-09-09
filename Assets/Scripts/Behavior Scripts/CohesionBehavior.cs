using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The CohesionBehavior will attempt to make agents converge with one another
[CreateAssetMenu(menuName = "Flock/Behaviors/Cohesion")]
public class CohesionBehavior : FlockBehavior {

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
        return move;
    }

}
