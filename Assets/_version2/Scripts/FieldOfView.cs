using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    [Header("Settings")]
    [SerializeField] private float viewRadius;
	[Range(0,360)]
	[SerializeField] private float viewAngle;
    [Range(0.2f, 1f)]
    [Tooltip("How long to wait before checking the field of view.")]
    [SerializeField] private float delay = 0.2f;

    [Header("Masks")]
	[SerializeField] private LayerMask targetMask;
	[SerializeField] private LayerMask obstacleMask;

    [Space()]
	[SerializeField] private List<Transform> visibleTargets = new List<Transform>();
	[SerializeField] private Transform closestTarget;
    
    #region Properties
    public float ViewRadius { get => viewRadius; }
    public float ViewAngle { get => viewAngle; }
    public List<Transform> VisibleTargets { get => visibleTargets; }
    public Transform ClosestTarget { get => closestTarget; }
    #endregion

	private Collider2D[] targetsBuffer = new Collider2D[256];

	private void Start(){
		StartCoroutine(FindTargetsWithDelay());
	}

	IEnumerator FindTargetsWithDelay(){
		while(true){
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}

	void FindVisibleTargets(){
		visibleTargets.Clear();
        closestTarget = null;
		int hits = Physics2D.OverlapCircleNonAlloc(transform.position, viewRadius, targetsBuffer, targetMask);
        float closestDistance = float.MaxValue;
		for(int i=0; i<hits; ++i){
			Transform target = targetsBuffer[i].transform;

            if(target==this.transform) { continue; }

			// Changes transform.forward to transform.up when calculating angle
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if(Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2){
				float dstToTarget = Vector3.Distance(transform.position, target.position);
				if(!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask)){
					visibleTargets.Add(target);
                    if(dstToTarget<closestDistance) {
                        closestDistance = dstToTarget;
                        this.closestTarget = target;
                    }
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
