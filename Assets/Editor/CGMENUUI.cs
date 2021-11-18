using System.Collections;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CGMenu))]
public class CGMENUUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CGMenu obj = (CGMenu)target;

        /*
        if (GUILayout.Button("TEst tut card"))
        {
            obj.ActivateCardTutorial();
        }
        */

    }
}
