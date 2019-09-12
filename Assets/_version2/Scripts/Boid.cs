using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    [Header("Components")]
    [SerializeField] private new SpriteRenderer renderer;
    private FieldOfView view;

    [Header("Settings")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float turnSpeed = 2f;


    void Awake(){ 
        view = GetComponent<FieldOfView>();
    }

    public void Initialize(float speed, float turnSpeed) {
        this.speed = speed;
        this.turnSpeed = turnSpeed;
    }

    void Update() {
        Transform closest = view.ClosestTarget;
        Vector3 direction = (closest!=null) ? Avoid(closest.position) : transform.up;
        //Vector3 direction = AvoidVisible();
        Move(direction.normalized * speed);
    }

    private Vector3 AvoidVisible() {
        List<Transform> targets = view.VisibleTargets;
        Vector3 direction = transform.up;
        foreach(Transform item in targets) {
            direction += item.position - transform.position;
        }
        return (targets.Count>0) ? direction/targets.Count : direction;
    }

    private Vector3 Avoid(Vector3 objectPosition) {
        Vector3 avoidanceDirection = Vector3.right;
        Vector3 targetDir = objectPosition - transform.position + avoidanceDirection; 
        return Vector3.RotateTowards(transform.up, targetDir, turnSpeed*Time.deltaTime, 0.0f);
    }

    public void Move(Vector2 velocity) {
        transform.up = velocity;
        transform.position += (Vector3)velocity * Time.deltaTime;
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
#endif
}
