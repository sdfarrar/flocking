using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour {

    public float delay = 2f;
    public Transform target;
    public float rotation = 0f;
    public float debugLength = 1f;
    public Vector3 rotationVector;
    public float degreesDelta = 15f;

    public Vector3[] rayDirections;
    public LayerMask hitMask;

    int rayIndexWithCollision = -1;
    int rayIndexWithNoCollision = -1;

    CircleCollider2D cirCol;

    void Start() {
        cirCol = GetComponent<CircleCollider2D>();
        target.localPosition = new Vector3(0, debugLength, 0);
        ComputeRays();
        //StartCoroutine(Doit());
    }

    void Update() {
        rayIndexWithCollision = CheckRaysForCollision();
        rayIndexWithNoCollision = CheckRaysForNoCollision();
    }

    private int CheckRaysForCollision() {
        RaycastHit2D[] results = new RaycastHit2D[2];
        for(int i=0; i<rayDirections.Length; ++i) {
            Vector3 direction = rayDirections[i];
            int hits = Physics2D.RaycastNonAlloc(transform.position, direction, results, 1f, hitMask);
            if(hits == 2) { return i; }
        }
        return -1;
    }

    private int CheckRaysForNoCollision() {
        //RaycastHit2D[] results = new RaycastHit2D[2];
        Collider2D[] results = new Collider2D[2];
        for(int i=0; i<rayDirections.Length; ++i) {
            Vector3 direction = rayDirections[i];
            //int hits = Physics2D.RaycastNonAlloc(transform.position, direction, results, 1f, hitMask);
            Vector3 origin = transform.position + direction * cirCol.radius*2.1f;
            //int hits = Physics2D.CircleCastNonAlloc(origin, cirCol.radius, direction, results, 1f, hitMask);
            int hits = Physics2D.OverlapCircleNonAlloc(origin, cirCol.radius, results, hitMask);
            int totalhits = 0;
            for(int j=0; j<hits; ++j) {
                if(results[j].transform == this.transform) { continue;}
                ++totalhits;
            }
            Debug.Log($"{direction} : {hits} : {totalhits}");
            if(hits < 1) { return i; }
            //if(totalhits < 2) { return i; }
        }
        return -1;
    }

    /// <summary>
    /// Builds a list of ray directions. Indices are in the form of Left, Right, Left, Right
    /// in respect to transform.up
    /// </summary>
    private void ComputeRays() {
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

    private IEnumerator Doit() {
        while(true) {
            //Vector3 thingy = Quaternion.Euler(0, 0, rotation) * target.localPosition;
            //target.localRotation = Quaternion.Euler(0, 0, rotation);
            //target.localPosition = thingy.normalized;
            //Debug.Log($"{thingy.normalized} | {target.localPosition}");
            rotationVector = Quaternion.Euler(0, 0, rotation) * new Vector3(1, 1);
            rotationVector.Normalize();
            Ray2D ray = new Ray2D(transform.position, rotationVector);
            Debug.Log($"Origin({transform.position}) | Direction:({rotationVector}");
            yield return new WaitForSeconds(delay);
            rotation = (rotation - 15f) % 360f;
        }
    }

    private void OnDrawGizmosSelected() {
        //Gizmos.DrawLine(transform.position, target.position);
        if(rayIndexWithCollision != -1) {
            Gizmos.color = Color.red;
            Vector3 direction = rayDirections[rayIndexWithCollision];
            Gizmos.DrawLine(transform.position, transform.position + direction);
        }
        if(rayIndexWithNoCollision != -1) {
            Gizmos.color = Color.green;
            Vector3 direction = rayDirections[rayIndexWithNoCollision];
            Vector3 origin = transform.position + direction * cirCol.radius*2.1f;
            Gizmos.DrawWireSphere(origin, cirCol.radius);
            Gizmos.DrawLine(origin, transform.position + direction);
        }
    }

}
