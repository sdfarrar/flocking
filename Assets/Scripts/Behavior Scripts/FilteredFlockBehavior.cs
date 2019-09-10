using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FilteredFlockBehavior : FlockBehavior {
    [SerializeField] private ContextFilter filter;
    public ContextFilter ContextFilter { get => filter; }
}
