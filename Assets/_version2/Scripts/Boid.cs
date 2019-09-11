using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour {

    [SerializeField] private new SpriteRenderer renderer;


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
