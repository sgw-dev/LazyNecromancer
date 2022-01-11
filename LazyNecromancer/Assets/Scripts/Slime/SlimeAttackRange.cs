using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(LineRenderer))]
public class SlimeAttackRange : MonoBehaviour {
    
    SlimeBehaviour sb;

    [Range(0,50)]
    public int segments = 50;
    float radius;
    
    [Range(0,10)]
    public float width;
    LineRenderer line;

    public delegate void SetFlag();
    public SetFlag setflag;
    public delegate void UnsetFlag();
    public UnsetFlag clearflag;
    public delegate void SetPositionForInterest(Vector3 pos);
    public SetPositionForInterest PointOfIntereset;

    void Start() {

        sb = GetComponentInParent<SlimeBehaviour>();
        Collider2D tmp = GetComponent<Collider2D>();

        if(tmp.GetType() == typeof(CircleCollider2D)) {

            radius = ((CircleCollider2D)tmp).radius;
            line = gameObject.GetComponent<LineRenderer>();

            line.sharedMaterial = sb.lit;
            line.endWidth       = width;
            line.startWidth     = width;
            line.positionCount  = segments + 1;
            line.useWorldSpace  = false;

            float x,y,z;
            float angle = 20f;

            for (int i = 0; i < (segments + 1); i++) {

                x = Mathf.Sin (Mathf.Deg2Rad * angle) * radius;
                y = Mathf.Cos (Mathf.Deg2Rad * angle) * radius;
                line.SetPosition (i,new Vector3(x,y,0) );
                angle += (360f / segments);
            
            }
            
        } else if(tmp.GetType() == typeof(BoxCollider2D)) {
            
            BoxCollider2D b = ((BoxCollider2D)tmp);
            Vector2 dim     = b.size;
            Vector3 o       = b.bounds.center;
            Vector2 p1,p2,p3,p4;//starting p1 upperleft going clockwise
            
            float w = dim.x/2f;
            float h = dim.y/2f;
            
            p1 = new Vector2(o.x-w,o.y+h);
            p2 = new Vector2(o.x+w,o.y+h);
            p3 = new Vector2(o.x+w,o.y-h);
            p4 = new Vector2(o.x-w,o.y-h);


            p1 = new Vector2(-w,+h);
            p2 = new Vector2(+w,+h);
            p3 = new Vector2(w,-h);
            p4 = new Vector2(-w,-h);
            
            line = gameObject.GetComponent<LineRenderer>();
            line.sharedMaterial = sb.lit;

            line.endWidth   = width;
            line.startWidth = width;
            
            line.useWorldSpace = false;
            line.positionCount = 6;
            
            
            line.SetPosition (0,p1);
            line.SetPosition (1,p2);
            line.SetPosition (2,p3);
            line.SetPosition (3,p4);
            line.SetPosition (4,p1);
            line.SetPosition (5,p2);//nice rect with no corners missing pieces
            
        } else {
            throw new Exception("No setup for collider");
        }

    }

    void ShowattackRangeGreen() {
        line.endColor   = Color.green;
        line.startColor = Color.green;
    }

    void ShowattackRangeRed() {
        line.endColor   = Color.red;
        line.startColor = Color.red;
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag("Player")) {
            setflag();
            PointOfIntereset(col.transform.position); 
            ShowattackRangeRed();
       }
    }
    
    void OnTriggerStay2D(Collider2D col) {
        if(col.CompareTag("Player")) {
            setflag();
            PointOfIntereset(col.transform.position);
            ShowattackRangeRed();
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if(col.CompareTag("Player")) {
            clearflag();
            PointOfIntereset(transform.parent.position);//set the parent position to itself
            ShowattackRangeGreen();
       }
    }

}
