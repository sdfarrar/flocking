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

    private Vector3 velocity;

    void Awake(){ 
        view = GetComponent<FieldOfView>();
    }

    public void Initialize(float speed, float turnSpeed) {
        this.speed = speed;
        this.turnSpeed = turnSpeed;
        velocity = transform.up * speed;
    }

    void Update() {
        Transform closest = view.ClosestTarget;
        Vector3 direction = (closest!=null) ? AvoidClosest(closest.position) : transform.up;
        Move(direction.normalized * speed);
        //Vector3 direction = AvoidVisible();
        //Move(direction);
    }

    private Vector3 AvoidVisible() {
        List<Transform> targets = view.VisibleTargets;
        Vector3 direction = Vector2.zero;
        foreach(Transform item in targets) {
            direction += item.position - transform.position;
        }
        //return (targets.Count>0) ? direction/targets.Count : direction;
        direction = (targets.Count>0) ? direction/targets.Count : transform.up;
        direction = direction.normalized * speed - velocity;
        return direction;
        //return Vector3.ClampMagnitude(direction, turnSpeed);
    }

    private Vector3 AvoidClosest(Vector3 objectPosition) {
        //Vector3 avoidanceDirection = Vector3.right;
        //Vector3 targetDir = objectPosition - transform.position + avoidanceDirection; 
        //return Vector3.RotateTowards(transform.up, targetDir, turnSpeed*Time.deltaTime, 0.0f);
        Vector3 targetDir = objectPosition - transform.position;
        Vector3 heading = transform.up;
        float angle = Vector3.Angle(heading, targetDir);

        RaycastHit2D[] hits = new RaycastHit2D[2];

        for(int deg=0; deg<view.ViewAngle*.5f; deg+=15) {
            float radians = deg*Mathf.Deg2Rad;
            Vector3 check1 = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians));
            Vector3 check2 = new Vector3(Mathf.Cos(-radians), Mathf.Sin(-radians));

            Vector3 target = transform.position + check1;
            int hitCount = Physics2D.RaycastNonAlloc(transform.position, target, hits, view.ViewRadius, view.TargetMask);
            //if(hitCount==1) { return Vector3.RotateTowards(transform.up, target, turnSpeed*Time.deltaTime, 0.0f); }
            if(hitCount==1) { return target; }

            target = transform.position + check2;
            hitCount = Physics2D.RaycastNonAlloc(transform.position, target, hits, view.ViewRadius, view.TargetMask);
            //if(hitCount==1) { return Vector3.RotateTowards(transform.up, target, turnSpeed*Time.deltaTime, 0.0f); }
            if(hitCount==1) { return target; }
        }

        //Physics2D.Raycast(transform.position, )
        return heading;
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

    void Log(string msg) {
        if(UnityEditor.Selection.activeGameObject!=this.gameObject){ return; }
        Debug.Log(msg, this);
    }
#endif
}
