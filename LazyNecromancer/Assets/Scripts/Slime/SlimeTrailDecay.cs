using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeTrailDecay : MonoBehaviour {

    public delegate void QueueReturn(SlimeTrailDecay s);
    public QueueReturn returntopool;
    Animator animator;

    void Start() {
        animator = GetComponent<Animator>();
    }

    public void Decayed() {
        returntopool(this);
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.CompareTag("Player")) {
            Debug.Log("Player Walked into slime damage!");
        }
    }
}
