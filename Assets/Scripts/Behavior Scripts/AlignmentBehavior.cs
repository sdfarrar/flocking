using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The AlignmentBehavior will attempt to align all agents in the same direction
[CreateAssetMenu(menuName = "Flock/Behaviors/Alignment")]
public class AlignmentBehavior : FlockBehavior {

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) {
        // If no neighbors, maintain current alignment
        if (context.Count == 0) { return agent.transform.up; }

        // Otherwise average out alignment of neighbors
        Vector2 move = Vector2.zero;
        foreach (Transform item in context) {
            move += (Vector2)item.up;
        }
        move /= context.Count;

        return move;
    }

}
