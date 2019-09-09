using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct WeightedBehavior {
    public FlockBehavior Behavior;
    public float Weight;
}

[CreateAssetMenu(menuName = "Flock/Behaviors/Composite")]
public class CompositeBehavior : FlockBehavior {

    [SerializeField] private WeightedBehavior[] behaviors;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) {
        Vector2 move = Vector2.zero;

        for(int i=0; i<behaviors.Length; ++i) {
            float weight = behaviors[i].Weight;
            Vector2 partialMove = behaviors[i].Behavior.CalculateMove(agent, context, flock) * weight;

            if(partialMove != Vector2.zero) {
                if(partialMove.sqrMagnitude > weight * weight){
                    partialMove.Normalize();
                    partialMove *= weight;
                }
                move += partialMove;
            }

        }
        
        return move;
    }

}
