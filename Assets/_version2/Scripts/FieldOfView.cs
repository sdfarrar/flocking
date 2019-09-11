using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    public float viewRadius;
	[Range(0,360)]
	public float viewAngle;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	public List<Transform> visibileTargets = new List<Transform>();

	private Collider2D[] targetsBuffer = new Collider2D[256]; // probably only player

	private void Start(){
		StartCoroutine(FindTargetsWithDelay(0.2f));
	}

	IEnumerator FindTargetsWithDelay(float delay){
		while(true){
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}

	void FindVisibleTargets(){
		visibileTargets.Clear();
		int hits = Physics2D.OverlapCircleNonAlloc(transform.position, viewRadius, targetsBuffer, targetMask);
		for(int i=0; i<hits; ++i){
			Transform target = targetsBuffer[i].transform;

            if(target==this.transform) { continue; }

			// Changes transform.forward to transform.up when calculating angle
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if(Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2){
				float dstToTarget = Vector3.Distance(transform.position, target.position);
				if(!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)){
					visibileTargets.Add(target);
				}
			}
		}
		
	}

	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal){
		// Differs from video some. We're using the z axis if the angle is not global.
		// Alsow we're setting the y instead of z in our return vector
		if(!angleIsGlobal){
			angleInDegrees += -transform.eulerAngles.z; // rotate with player rotation
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
	}

}
