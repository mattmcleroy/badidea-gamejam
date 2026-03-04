using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CubeField))]
public class CubeFieldEditor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        CubeField cubeField = (CubeField)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Generate", GUILayout.Height(30))) {
            cubeField.Generate();
        }
    }
}