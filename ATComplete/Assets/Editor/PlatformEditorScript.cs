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

        //EditorGUILayout.PropertyField(levelPossible);
        
        EditorGUILayout.PropertyField(platformStartPos);
        EditorGUILayout.PropertyField(platformPointA);
        EditorGUILayout.PropertyField(platformPointB);
        EditorGUILayout.PropertyField(platformMoveSpeed);
        EditorGUILayout.PropertyField(isMoving);
        

/*        EditorGUILayout.LabelField(" Horizontal ", style, GUILayout.ExpandWidth(true));
        horizontalMoveGroup = EditorGUILayout.BeginFoldoutHeaderGroup(horizontalMoveGroup, "Horizontal Movement");
        if (horizontalMoveGroup)
        {
            EditorGUILayout.PropertyField(isMovingH);
            EditorGUILayout.PropertyField(minPlatformXValue);
            EditorGUILayout.PropertyField(maxPlatformXValue);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.LabelField(" Vertical ", style, GUILayout.ExpandWidth(true));
        verticalMoveGroup = EditorGUILayout.BeginFoldoutHeaderGroup(verticalMoveGroup, "Vertical Movement");
        if (verticalMoveGroup)
        {
            EditorGUILayout.PropertyField(isMovingV);
            EditorGUILayout.PropertyField(minPlatformHeight);
            EditorGUILayout.PropertyField(maxPlatformHeight);
        }
        EditorGUILayout.EndFoldoutHeaderGroup();

        EditorGUILayout.LabelField(" Extra ", style, GUILayout.ExpandWidth(true));
        EditorGUILayout.PropertyField(startPlatformHeight);*/



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

        /*        platformsGroup = EditorGUILayout.BeginFoldoutHeaderGroup(platformsGroup, "Platforms");
                if (platformsGroup)
                {
                    EditorGUILayout.PropertyField(startPlatform);
                    EditorGUILayout.PropertyField(endPlatform);
                }
                EditorGUILayout.EndFoldoutHeaderGroup();

                EditorGUILayout.PropertyField(startEndPointsList);
                EditorGUILayout.PropertyField(platformSpaceDistance);

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


                 */



        serializedObject.ApplyModifiedProperties();
        //levelStatusChecker.jumpHeight = (int)EditorGUILayout.Slider("Jump Height", levelStatusChecker.jumpHeight, 1f, 5f);
    }
}
