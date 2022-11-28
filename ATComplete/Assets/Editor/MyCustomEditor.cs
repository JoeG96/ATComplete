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
    SerializedProperty startingPoint;
    SerializedProperty endingPoint;
    SerializedProperty jumpHeight;
    SerializedProperty boxHeight;
    SerializedProperty numberOfBoxes;
    SerializedProperty boxPrefab;
    SerializedProperty helperObstacles;
    SerializedProperty obstacleHeight;
    SerializedProperty totalBoxHeight;
    SerializedProperty totalReachHeight;
    SerializedProperty startToEndDistance;
    SerializedProperty spawnPoint;
    SerializedProperty isMoving;
    SerializedProperty startPlatformHeight;
    SerializedProperty minPlatformHeight;
    SerializedProperty maxPlatformHeight;
    SerializedProperty platformMovingSpeed;

    bool isMovingGroup, functionsGroup = false;
    #endregion

    private void OnEnable()
    {
        startingPoint = serializedObject.FindProperty("startingPoint");
        endingPoint = serializedObject.FindProperty("endingPoint");
        jumpHeight = serializedObject.FindProperty("jumpHeight");
        boxHeight = serializedObject.FindProperty("boxHeight");
        numberOfBoxes = serializedObject.FindProperty("numberOfBoxes");
        boxPrefab = serializedObject.FindProperty("boxPrefab");
        helperObstacles = serializedObject.FindProperty("helperObstacles");
        obstacleHeight = serializedObject.FindProperty("obstacleHeight");
        totalBoxHeight = serializedObject.FindProperty("totalBoxHeight");
        totalReachHeight = serializedObject.FindProperty("totalReachHeight");
        startToEndDistance = serializedObject.FindProperty("startToEndDistance");
        spawnPoint = serializedObject.FindProperty("spawnPoint");
        isMoving = serializedObject.FindProperty("isMoving");
        startPlatformHeight = serializedObject.FindProperty("startPlatformHeight");
        minPlatformHeight = serializedObject.FindProperty("minPlatformHeight");
        maxPlatformHeight = serializedObject.FindProperty("maxPlatformHeight");
        platformMovingSpeed = serializedObject.FindProperty("platformMovingSpeed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        LevelStatusChecker levelStatusChecker = (LevelStatusChecker)target;
        
        EditorGUILayout.PropertyField(startingPoint);
        EditorGUILayout.PropertyField(endingPoint);
        EditorGUILayout.LabelField(" Heights ", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(jumpHeight);
        
        EditorGUILayout.PropertyField(boxHeight);
        EditorGUILayout.PropertyField(numberOfBoxes);
        EditorGUILayout.PropertyField(boxPrefab);
         
        EditorGUILayout.LabelField("  Obstacles ", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(helperObstacles);
        EditorGUILayout.PropertyField(obstacleHeight);
        EditorGUILayout.PropertyField(totalReachHeight);

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

        isMovingGroup = EditorGUILayout.BeginFoldoutHeaderGroup(isMovingGroup, "Movement");
            if (isMovingGroup)
            {
                EditorGUILayout.PropertyField(isMoving);
                if(levelStatusChecker.isMoving)
                {
                    EditorGUILayout.PropertyField(startPlatformHeight);
                    EditorGUILayout.PropertyField(maxPlatformHeight);
                    EditorGUILayout.PropertyField(minPlatformHeight);
                    EditorGUILayout.PropertyField(platformMovingSpeed);
                }
            }
        EditorGUILayout.EndFoldoutHeaderGroup();

        


        serializedObject.ApplyModifiedProperties();
        //levelStatusChecker.jumpHeight = (int)EditorGUILayout.Slider("Jump Height", levelStatusChecker.jumpHeight, 1f, 5f);
    }

    /*    public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = new VisualElement();
            LevelStatusChecker levelStatusChecker = (LevelStatusChecker)target;
            inspector.Add(base.CreateInspectorGUI());
            //inspector.Add(new Label("Bingus"));

            // Visible in UI
            SerializedProperty startingPoint = serializedObject.FindProperty("startingPoint");
            SerializedProperty endingPoint = serializedObject.FindProperty("endingPoint");
            SerializedProperty jumpHeight = serializedObject.FindProperty("jumpHeight");
            SerializedProperty boxHeight = serializedObject.FindProperty("boxHeight");
            SerializedProperty numberOfBoxes = serializedObject.FindProperty("numberOfBoxes");
            SerializedProperty boxPrefab = serializedObject.FindProperty("boxPrefab");
            SerializedProperty helperObstacles = serializedObject.FindProperty("helperObstacles");
            SerializedProperty obstacleHeight = serializedObject.FindProperty("obstacleHeight");

            // Not Visible in UI
            SerializedProperty totalBoxHeight = serializedObject.FindProperty("totalBoxHeight");
            SerializedProperty totalReachHeight = serializedObject.FindProperty("totalReachHeight");
            SerializedProperty startToEndDistance = serializedObject.FindProperty("startToEndDistance");
            SerializedProperty spawnPoint = serializedObject.FindProperty("spawnPoint");


            inspector.Add(new PropertyField(startingPoint));
            inspector.Add(new PropertyField(endingPoint));
            inspector.Add(new PropertyField(jumpHeight));
            inspector.Add(new Label("Boxes"));
            inspector.Add(new PropertyField(boxHeight));
            inspector.Add(new PropertyField(numberOfBoxes));
            inspector.Add(new PropertyField(boxPrefab));
            inspector.Add(new Label("Level Features"));
            inspector.Add(new PropertyField(helperObstacles));
            inspector.Add(new PropertyField(obstacleHeight));

            return inspector;

        }*/




}
