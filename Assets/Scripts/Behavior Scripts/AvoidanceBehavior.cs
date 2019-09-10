using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The AvoidanceBehavior will cause agents to avoid from running into each other
[CreateAssetMenu(menuName = "Flock/Behaviors/Avoidance")]
public class AvoidanceBehavior : FilteredFlockBehavior {

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) {
        // If no neighbors, then no adjustment
        if (context.Count == 0) { return Vector2.zero; }

        // Otherwise average out position of neighbors
        Vector2 move = Vector2.zero;
        int nAvoid = 0;
        List<Transform> filteredContext = (ContextFilter!=null) ? ContextFilter.Filter(agent, context) : context;
        foreach (Transform item in filteredContext) {
            bool inAvoidanceRadius = Vector2.SqrMagnitude(item.position - agent.transform.position) < flock.SquareAvoidanceRadius;
            if(!inAvoidanceRadius) { continue; }

            ++nAvoid;
            move += (Vector2)(agent.transform.position - item.position);
        }

        return (nAvoid > 0) ? move / nAvoid : move;
    }

}
