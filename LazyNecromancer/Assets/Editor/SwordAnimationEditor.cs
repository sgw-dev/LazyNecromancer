using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SwordAnimation))]
public class SwordAnimationEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SwordAnimation swordAnimation = (SwordAnimation)target;
        if (GUILayout.Button("Play Animation"))
        {
            swordAnimation.TestAnimation();
        }
    }
}
