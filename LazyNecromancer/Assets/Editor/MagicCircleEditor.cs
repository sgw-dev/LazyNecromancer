using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MagicCircleRotator))]
public class MagicCircleEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MagicCircleRotator magicCircleRotator = (MagicCircleRotator)target;
        if (GUILayout.Button("Add Components"))
        {
            magicCircleRotator.CreateComponentList();
        }
    }
}
