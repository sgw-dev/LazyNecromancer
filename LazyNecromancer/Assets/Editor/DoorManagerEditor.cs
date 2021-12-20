using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DoorManager))]
public class DoorManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DoorManager doorManager = (DoorManager)target;
        if (GUILayout.Button("Unlock Room"))
        {
            doorManager.RoomCleared();
        }
        if (GUILayout.Button("Reset Room"))
        {
            doorManager.ResetRoom();
        }
    }
}
