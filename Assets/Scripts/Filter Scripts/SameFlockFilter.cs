using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Flock/Filter/Same Flock")]
public class SameFlockFilter : ContextFilter {
    public override List<Transform> Filter(FlockAgent agent, List<Transform> list) {
        return list.FindAll( transform => {
            FlockAgent itemAgent = transform.GetComponent<FlockAgent>();
            return itemAgent!=null && itemAgent.Flock==agent.Flock;
        });
    }
}
