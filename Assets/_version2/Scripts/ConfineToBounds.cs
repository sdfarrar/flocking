using System;
using UnityEngine;


public class ConfineToBounds : MonoBehaviour {

    public BoxCollider2D Boundary;

    void Update() {
        if(Boundary==null){ return; }
        if(Boundary.bounds.Contains(transform.position)){ return; }
        WrapAround();
    }

    private void WrapAround() {
        Vector3 pos = transform.position;
        Vector3 min = Boundary.bounds.min;
        Vector3 max = Boundary.bounds.max;

        if(pos.x < min.x){ pos.x = max.x; }
        if(pos.y < min.y){ pos.y = max.y; }
        if(pos.x > max.x){ pos.x = min.x; }
        if(pos.y > max.y){ pos.y = min.y; }

        transform.position = pos;
    }
}