using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameMaster))]
public class UiInterpreter : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GameMaster obj = (GameMaster)target;

        if (GUILayout.Button("Add 2000 money"))
        {
            GameMaster.instance.DEBUGMONEY();
        }

        if (GUILayout.Button("Reset Saves"))
        {
            GameMaster.instance.DEBUGSAVESYSTEM();
        }
    }
}