using System;
using UnityEngine;


public class ConfineToBounds : MonoBehaviour {

    [SerializeField] private BoxCollider2D boundary;

    private Bounds bounds;

    void Start() {
        if(boundary==null){
            Debug.Log("Boundary needs set!", this);
            this.gameObject.SetActive(false);
            return;
        }
        bounds = boundary.bounds;
    }

    void Update() {
        if(bounds.Contains(transform.position)){ return; }
        WrapAround();
    }

    private void WrapAround() {
        Vector3 pos = transform.position;
        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        if(pos.x < min.x){ pos.x = max.x; }
        if(pos.y < min.y){ pos.y = max.y; }
        if(pos.x > max.x){ pos.x = min.x; }
        if(pos.y > max.y){ pos.y = min.y; }

        transform.position = pos;
    }
}