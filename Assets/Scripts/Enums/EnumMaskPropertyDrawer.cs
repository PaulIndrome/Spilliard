/*
Written by: Lucas Antunes (aka ItsaMeTuni), lucasba8@gmail.com
In: 2/15/2018
The only thing that you cannot do with this script is sell it by itself without substantially modifying it.
*/
 
using System;
using UnityEngine;
 
#if UNITY_EDITOR
using UnityEditor;
[CustomPropertyDrawer(typeof(EnumMaskAttribute))]
public class EnumMaskPropertyDrawer : PropertyDrawer
{
    bool foldoutOpen = false;
 
    object theEnum;
 
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (foldoutOpen)
            return EditorGUIUtility.singleLineHeight * (Enum.GetValues(fieldInfo.FieldType).Length + 2);
        else
            return EditorGUIUtility.singleLineHeight;
    }
 
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        theEnum = fieldInfo.GetValue(property.serializedObject.targetObject);
 
        EditorGUI.BeginProperty(position, label, property);
 
        foldoutOpen = EditorGUI.Foldout(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight), foldoutOpen, label);
 
        if (foldoutOpen)
        {
            if (GUI.Button(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * 1, 30, 15), "All"))
            {
                for (int i = 0; i < Enum.GetNames(fieldInfo.FieldType).Length; i++)
                {
                    theEnum = (int)theEnum | (1 << i);
                }
            }
            if (GUI.Button(new Rect(position.x + 32, position.y + EditorGUIUtility.singleLineHeight * 1, 40, 15), "None"))
            {
                for (int i = 0; i < Enum.GetNames(fieldInfo.FieldType).Length; i++)
                {
                    theEnum = (int)theEnum & ~(1 << i);
                }
            }
 
            for (int i = 0; i < Enum.GetNames(fieldInfo.FieldType).Length; i++)
            {
                if (EditorGUI.Toggle(new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight * (2 + i), position.width, EditorGUIUtility.singleLineHeight), Enum.GetNames(fieldInfo.FieldType)[i], ((int)theEnum & (1 << i)) != 0))
                {
                    theEnum = (int)theEnum | (1 << i);
                }
                else
                {
                    theEnum = (int)theEnum & ~(1 << i);
                }
            }
        }
 
        fieldInfo.SetValue(property.serializedObject.targetObject, theEnum);
        property.serializedObject.ApplyModifiedProperties();
 
    }
}
#endif
 
[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
public class EnumMaskAttribute : PropertyAttribute
{
 
}