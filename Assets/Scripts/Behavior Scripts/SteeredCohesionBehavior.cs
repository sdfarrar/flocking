using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviors/Steered Cohesion")]
public class SteeredCohesionBehavior : FilteredFlockBehavior {

    [SerializeField] private float agentSmoothTime = 0.5f;

    Vector2 currentVelocity;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) {
        // If no neighbors, then no adjustment
        if (context.Count == 0) { return Vector2.zero; }

        // Otherwise average out position of neighbors
        Vector2 move = Vector2.zero;
        List<Transform> filteredContext = (ContextFilter!=null) ? ContextFilter.Filter(agent, context) : context;
        foreach (Transform item in filteredContext) {
            move += (Vector2)item.position;
        }

        if(filteredContext.Count!=0) {
            move /= filteredContext.Count;
        }

        // Create offset from agent position
        move -= (Vector2)agent.transform.position;
        move = Vector2.SmoothDamp(agent.transform.up, move, ref currentVelocity, agentSmoothTime);
        return move;
    }

}
