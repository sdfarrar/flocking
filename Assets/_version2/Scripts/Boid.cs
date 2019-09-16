using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    [Header("Components")]
    [SerializeField] private new SpriteRenderer renderer;
    private FieldOfView view;
    private new CircleCollider2D collider;

    [Header("Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float maxTurnDegrees = 90f;

    [SerializeField] private static float wAvoid = 0.6f;
    [SerializeField] private static float wConverge = 0.6f;
    [SerializeField] private static float wAlign = 0.4f;

    private Vector3 velocity;

    void Awake(){ 
        view = GetComponent<FieldOfView>();
        collider = GetComponent<CircleCollider2D>();
        velocity = transform.up * speed;
        ComputeRays();
    }

    public void Initialize(float speed, float turnSpeed) {
        this.speed = speed;
        this.maxTurnDegrees = turnSpeed;
        velocity = transform.up * speed;
    }

    void Update() {
        //Vector3 direction = AvoidVisible();
        Vector3 direction = Avoid()*wAvoid + Converge()*wConverge + Align()*wAlign;
        Move(direction.normalized);
    }

    private Vector3 AvoidVisible() {
        Vector3 direction = FindOpenDirection();
        return Vector3.Lerp(velocity.normalized, direction.normalized, maxTurnDegrees*Mathf.Deg2Rad*Time.deltaTime);
        //return Vector3.RotateTowards(velocity, direction, maxTurnDegrees*Mathf.Deg2Rad*Time.deltaTime, 1.0f);
    }

    private Vector3 Avoid() {
        if (view.VisibleTargets.Count == 0) { return Vector2.zero; }

        // Otherwise average out position of neighbors
        Vector2 move = Vector2.zero;
        int nAvoid = 0;
        float radiusSq = view.ViewRadius * view.ViewRadius;
        foreach (Transform item in view.VisibleTargets) {
            bool inAvoidanceRadius = Vector2.SqrMagnitude(item.position - transform.position) < radiusSq;
            if(!inAvoidanceRadius) { continue; }

            ++nAvoid;
            move += (Vector2)(transform.position - item.position);
        }

        return (nAvoid > 0) ? move / nAvoid : move;
    }

    private Vector3 Converge() {
        // If no neighbors, then no adjustment
        if (view.VisibleTargets.Count == 0) { return Vector2.zero; }

        // Otherwise average out position of neighbors
        Vector2 move = Vector2.zero;
        foreach (Transform item in view.VisibleTargets) {
            move += (Vector2)item.position;
        }
        move /= view.VisibleTargets.Count;

        // Create offset from agent position
        move -= (Vector2)transform.position;
        return move;
    }

    private Vector3 Align() {
        // If no neighbors, maintain current alignment
        if (view.VisibleTargets.Count == 0) { return transform.up; }

        // Otherwise average out alignment of neighbors
        Vector2 move = Vector2.zero;
        foreach (Transform item in view.VisibleTargets) {
            move += (Vector2)item.up;
        }
        if(view.VisibleTargets.Count!=0) {
            move /= view.VisibleTargets.Count;
        }

        return move;
    }

    private static Vector3[] rayDirections;
    private void ComputeRays() {
        if(rayDirections!=null){ return; }
        int degreesDelta = 15;
        int count = (int)(360 / degreesDelta);
        int halfCount = count / 2;
        rayDirections = new Vector3[count];
        for(int i=0; i<halfCount; i+=2) {
            float rotation = i * degreesDelta;
            // Left Direction
            Vector3 direction = Quaternion.Euler(0, 0, rotation) * transform.up;
            direction.Normalize();
            rayDirections[i] = direction;
            // Right Direction
            direction = Quaternion.Euler(0, 0, -rotation) * transform.up;
            direction.Normalize();
            rayDirections[i+1] = direction;
        }

    }

    private Vector3 FindOpenDirection() {
        Collider2D[] results = new Collider2D[2];
        for(int i=0; i<rayDirections.Length; ++i) {
            Vector3 direction = (transform.up + rayDirections[i]).normalized;
            Vector3 origin = transform.position + direction * collider.radius*2.1f;
            int hits = Physics2D.OverlapCircleNonAlloc(origin, collider.radius, results, view.TargetMask);
            int totalhits = 0;
            for(int j=0; j<hits; ++j) {
                if(results[j].transform == this.transform) { continue;}
                ++totalhits;
            }
            if(hits < 1) { return direction; }
        }
        return transform.up;
    }


    public void Move(Vector2 direction) {
        transform.up = direction;
        transform.position += (Vector3)direction * speed * Time.deltaTime;
        Log("up: " + transform.up);
    }

#if UNITY_EDITOR
    void OnDrawGizmos() {
        if(renderer==null){ return; }
        GameObject[] selected = UnityEditor.Selection.gameObjects;
        if(UnityEditor.Selection.activeGameObject != this.gameObject){
            renderer.color = Color.white;
            return;
        }
        renderer.color = Color.red;
    }

    void Log(string msg) {
        if(UnityEditor.Selection.activeGameObject!=this.gameObject){ return; }
        Debug.Log(msg, this);
    }
#endif
}
