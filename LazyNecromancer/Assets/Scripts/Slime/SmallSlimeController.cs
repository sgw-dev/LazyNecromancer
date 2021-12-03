using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSlimeController : MonoBehaviour {
    
    public AnimationCurve travelarc;
    [SerializeField]
    float TimeUntilImpact;
    float timer;

    public float maxHeight; 
    
    Animator anim;

    public delegate void ReturnToPool(SlimeProjectile glob);
    public ReturnToPool putback;
    
    GameObject inRange;
    bool dmgDealt;

    void Awake() {
        anim = GetComponent<Animator>();
    }

}
