using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruler : MonoBehaviour {

    [Range(.8f,.99f)]
    [SerializeField]    
    float ruler;

    void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,transform.parent.position);
        
        Gizmos.color = Color.green;
        
        Gizmos.DrawLine(Vector3.Lerp(transform.position,transform.parent.position,ruler),transform.parent.position);
    
    }
    
}
