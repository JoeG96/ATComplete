using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof (MovePlatform))]
public class PlatformEditorScript : Editor
{
    #region Serialized Properties

    SerializedProperty distToNextPlatform;
    SerializedProperty isMoving;
    SerializedProperty isMovingH;
    SerializedProperty isMovingV;

    SerializedProperty startPlatformHeight;
    SerializedProperty minPlatformHeight;
    SerializedProperty maxPlatformHeight;

    SerializedProperty startPlatformXValue;
    SerializedProperty minPlatformXValue;
    SerializedProperty maxPlatformXValue;

    SerializedProperty maxPlatformHeightVector;
    SerializedProperty minPlatformHeightVector;

    SerializedProperty platformMovingSpeed;
    SerializedProperty height;

    SerializedProperty platformStartPos;
    SerializedProperty platformPointA;
    SerializedProperty platformPointB;
    SerializedProperty platformMoveSpeed;
    SerializedProperty distanceToNextPlatform;

    bool verticalMoveGroup, horizontalMoveGroup, functionsGroup = false;
    #endregion

    private void OnEnable()
    {

        distToNextPlatform = serializedObject.FindProperty("distToNextPlatform");

        isMoving = serializedObject.FindProperty("isMoving");
        isMovingH = serializedObject.FindProperty("isMovingH");
        isMovingV = serializedObject.FindProperty("isMovingV");

        startPlatformHeight = serializedObject.FindProperty("startPlatformHeight");
        minPlatformHeight = serializedObject.FindProperty("minPlatformHeight");
        maxPlatformHeight = serializedObject.FindProperty("maxPlatformHeight");

        startPlatformXValue = serializedObject.FindProperty("startPlatformXValue");
        minPlatformXValue = serializedObject.FindProperty("minPlatformXValue");
        maxPlatformXValue = serializedObject.FindProperty("maxPlatformXValue");

        maxPlatformHeightVector = serializedObject.FindProperty("maxPlatformHeightVector");
        minPlatformHeightVector = serializedObject.FindProperty("minPlatformHeightVector");

        platformMovingSpeed = serializedObject.FindProperty("platformMovingSpeed");
        height = serializedObject.FindProperty("height");

        platformStartPos = serializedObject.FindProperty("platformStartPos");
        platformPointA = serializedObject.FindProperty("platformPointA");
        platformPointB = serializedObject.FindProperty("platformPointB");
        platformMoveSpeed = serializedObject.FindProperty("platformMoveSpeed");
        distanceToNextPlatform = serializedObject.FindProperty("distanceToNextPlatform");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;

        var levelStyle = new GUIStyle();
        levelStyle.normal.textColor = Color.red;

        MovePlatform movePlatform = (MovePlatform)target;
        
        EditorGUILayout.PropertyField(platformStartPos);
        EditorGUILayout.PropertyField(platformPointA);
        EditorGUILayout.PropertyField(platformPointB);
        EditorGUILayout.PropertyField(platformMoveSpeed);
        EditorGUILayout.PropertyField(isMoving);
        EditorGUILayout.PropertyField(distanceToNextPlatform);
        
        functionsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(functionsGroup, "Functions");
        if (functionsGroup)
        {
            if (GUILayout.Button("Set Platform Center Point"))
            { movePlatform.SetCenterPoint(); } 
            if (GUILayout.Button("Set Platform Point A"))
            { movePlatform.SetPointA(); }
            if (GUILayout.Button("Set Platform Point B"))
            { movePlatform.SetPointB(); }
            if (GUILayout.Button("Place Platform at Center"))
            { movePlatform.PlaceAtCenter(); }  
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        serializedObject.ApplyModifiedProperties();
    }
}
