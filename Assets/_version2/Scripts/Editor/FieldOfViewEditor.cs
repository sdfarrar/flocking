using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfViewEditor : Editor {

	void OnSceneGUI(){
		FieldOfView fow = (FieldOfView)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fow.transform.position, Vector3.forward, Vector3.up, 360, fow.ViewRadius);

		Vector3 viewAngleA = fow.DirFromAngle(-fow.ViewAngle/2, false);
		Vector3 viewAngleB = fow.DirFromAngle(fow.ViewAngle/2, false);

		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.ViewRadius);
		Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.ViewRadius);

		Handles.color = Color.red;
		foreach(Transform visibleTarget in fow.VisibleTargets){
			Handles.DrawLine(fow.transform.position, visibleTarget.position);
		}

        //Handles.color = Color.green;
        //for(int i=0; i<360; i+=15) {
        //    Vector3 direction = new Vector3(Mathf.Cos(i*Mathf.Deg2Rad), Mathf.Sin(i*Mathf.Deg2Rad));
		//    Handles.DrawLine(fow.transform.position, fow.transform.position + direction * fow.ViewRadius);
        //}
	}

}
