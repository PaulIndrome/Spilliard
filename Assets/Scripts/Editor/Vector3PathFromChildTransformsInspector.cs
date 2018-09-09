using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Vector3PathFromChildTransforms))]
public class Vector3PathFromChildTransformsInspector : Editor {

    Vector3PathFromChildTransforms v3pfct;
    void OnEnable(){
        v3pfct = (Vector3PathFromChildTransforms) target;
    }

    public override void OnInspectorGUI(){
        serializedObject.Update();

        Vector3PathPosition[] posList = v3pfct.posList = v3pfct.transform.GetComponentsInChildren<Vector3PathPosition>();

        Color standardBGColor = GUI.backgroundColor;
        GUI.backgroundColor = Color.red;
        EditorGUI.BeginDisabledGroup(!(posList.Length > 0));
        if(GUILayout.Button("Create Path")){
            v3pfct.CreatePathFromChildren();
        }
        EditorGUI.EndDisabledGroup();
        GUI.backgroundColor = standardBGColor;
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Add Position")){
            v3pfct.AddPosition();
        }
        EditorGUI.BeginDisabledGroup(!(posList.Length > 0));
            if(GUILayout.Button("Toggle gizmos")){
                v3pfct.ToggleGizmos();
                SceneView.RepaintAll();
            }
            if(GUILayout.Button("Remove last")){
                v3pfct.DeleteLastPosition();
            }
            if(GUILayout.Button("Delete all")){
                v3pfct.DeleteAllPositions();
            }
        EditorGUI.EndDisabledGroup();
        GUILayout.EndHorizontal();
        DrawDefaultInspector();
    }

}