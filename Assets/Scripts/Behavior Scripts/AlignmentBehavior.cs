using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The AlignmentBehavior will attempt to align all agents in the same direction
[CreateAssetMenu(menuName = "Flock/Behaviors/Alignment")]
public class AlignmentBehavior : FilteredFlockBehavior {

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) {
        // If no neighbors, maintain current alignment
        if (context.Count == 0) { return agent.transform.up; }

        // Otherwise average out alignment of neighbors
        Vector2 move = Vector2.zero;
        List<Transform> filteredContext = (ContextFilter!=null) ? ContextFilter.Filter(agent, context) : context;
        foreach (Transform item in filteredContext) {
            move += (Vector2)item.up;
        }
        if(filteredContext.Count!=0) {
            move /= filteredContext.Count;
        }

        return move;
    }

}
