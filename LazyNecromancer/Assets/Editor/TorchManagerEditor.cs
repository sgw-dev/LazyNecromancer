using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TorchManager))]
public class TorchManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        TorchManager torchManager = (TorchManager)target;
        if (GUILayout.Button("Reset Torches"))
        {
            torchManager.ResetTorches();
        }
    }
}
