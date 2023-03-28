using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

[CustomEditor(typeof (LevelStatusChecker))]
public class MyCustomEditor : Editor
{
    #region Serialized Properties

    SerializedProperty levelPossible;

    SerializedProperty jumpHeight;
    
    SerializedProperty boxHeight;
    SerializedProperty numberOfBoxes;
    SerializedProperty boxPrefab;
    
    SerializedProperty helperObstacles;
    SerializedProperty obstacleHeight;
    SerializedProperty noHelpObstacles;
    SerializedProperty noHelpObstacleHeight;
    
    SerializedProperty totalBoxHeight;
    SerializedProperty totalReachHeight;
    SerializedProperty startToEndDistance;
    SerializedProperty spawnPoint;

    SerializedProperty startEndPointsList;
    SerializedProperty startPlatform;
    SerializedProperty endPlatform;

    SerializedProperty platformSpaceDistance;


    bool functionsGroup, platformsGroup = false;
    #endregion

    private void OnEnable()
    {
        levelPossible = serializedObject.FindProperty("levelPossible");

        jumpHeight = serializedObject.FindProperty("jumpHeight");
        
        boxHeight = serializedObject.FindProperty("boxHeight");
        numberOfBoxes = serializedObject.FindProperty("numberOfBoxes");
        boxPrefab = serializedObject.FindProperty("boxPrefab");
        
        helperObstacles = serializedObject.FindProperty("helperObstacles");
        obstacleHeight = serializedObject.FindProperty("obstacleHeight");
        noHelpObstacles = serializedObject.FindProperty("noHelpObstacles");
        noHelpObstacleHeight = serializedObject.FindProperty("noHelpObstacleHeight");
        
        totalBoxHeight = serializedObject.FindProperty("totalBoxHeight");
        totalReachHeight = serializedObject.FindProperty("totalReachHeight");
        startToEndDistance = serializedObject.FindProperty("startToEndDistance");
        spawnPoint = serializedObject.FindProperty("spawnPoint");
        
        startEndPointsList = serializedObject.FindProperty("startEndPointsList");
        startPlatform = serializedObject.FindProperty("startPlatform");
        endPlatform = serializedObject.FindProperty("endPlatform");

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;
        style.fontStyle = FontStyle.Bold;

        var levelStyle = new GUIStyle();
        levelStyle.normal.textColor = Color.red;

        LevelStatusChecker levelStatusChecker = (LevelStatusChecker)target;

        EditorGUILayout.PropertyField(levelPossible);

        platformsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(platformsGroup, "Platforms");
        if (platformsGroup)
        {
            EditorGUILayout.PropertyField(startPlatform);
            EditorGUILayout.PropertyField(endPlatform);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.PropertyField(startEndPointsList);

        EditorGUILayout.LabelField(" Heights ", style,GUILayout.ExpandWidth(true));
        EditorGUILayout.PropertyField(jumpHeight);
        EditorGUILayout.PropertyField(boxHeight);
        EditorGUILayout.PropertyField(totalReachHeight);

        EditorGUILayout.PropertyField(numberOfBoxes);
        EditorGUILayout.PropertyField(boxPrefab);
        
        EditorGUILayout.LabelField(" Obstacles ", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.PropertyField(helperObstacles);
        EditorGUILayout.PropertyField(noHelpObstacles);
        EditorGUILayout.PropertyField(obstacleHeight);
        EditorGUILayout.PropertyField(noHelpObstacleHeight);

        
        functionsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(functionsGroup, "Functions");
        if (functionsGroup)
        {
            if (GUILayout.Button("Set Platform to Possible"))
            { levelStatusChecker.SetToPossible(); }
            if (GUILayout.Button("Add Boxes Required To Complete"))
            { levelStatusChecker.AddBoxesNeeded(); }
            if (GUILayout.Button("Set Platform To Random Height"))
            { levelStatusChecker.SetToRandomHeight(); }
            if (GUILayout.Button("Set To Exact Height Required"))
            { levelStatusChecker.SetToExactBoxes(); }
        }
        EditorGUILayout.EndFoldoutHeaderGroup(); 
        serializedObject.ApplyModifiedProperties();
    }
}
