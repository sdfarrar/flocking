using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviors/Stay in Radius")]
public class StayInRadiusBehavior : FlockBehavior {

    [SerializeField] private Vector2 center;
    public float radius = 15f;

    public override Vector2 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock) {
        Vector2 centerOffset = center - (Vector2)agent.transform.position;
        float t = centerOffset.magnitude / radius;
        if(1 < 0.9f) {
            return Vector2.zero;
        }
        return centerOffset * t * t;
    }

}
