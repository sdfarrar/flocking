using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Filter/Layer")]
public class LayerFilter : ContextFilter {

    [SerializeField] private LayerMask mask;

    public override List<Transform> Filter(FlockAgent agent, List<Transform> list) {
        return list.FindAll( transform => {
            bool isLayerInMask = (mask == (mask | (1 << transform.gameObject.layer)));
            return isLayerInMask;
        });
    }

}
