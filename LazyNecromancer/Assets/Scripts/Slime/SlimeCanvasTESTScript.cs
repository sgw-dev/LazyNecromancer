using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeCanvasTESTScript : MonoBehaviour {
    
    public void StartSequence() {

    }

    public void DropSlime() {
        var tmp = FindObjectOfType<SlimeSpawner>();
        tmp.TriggerBossStart();
    }

    public void _21_damage_to_big_slime() {
        var tmp = FindObjectOfType<SlimeBehaviour>();
        tmp.TakeDamage(21);
    }

    public void _5_damage_to_small_slime() {
        var tmp = FindObjectsOfType<SmallSlimeController>();
        tmp.ToList<SmallSlimeController>().ForEach(slime => slime.TakeDamage(5));
    }

}
