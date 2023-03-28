using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelStatusChecker : MonoBehaviour
{  
    
    public int jumpHeight;
    [SerializeField] private int boxHeight;
    [SerializeField] private int totalBoxHeight;
    [SerializeField] private int totalReachHeight;
    [SerializeField] private float startToEndDistance;

    [SerializeField] private int numberOfBoxes;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private int playerBoxes;

    [SerializeField] private bool levelPossible;
    [SerializeField] private Material meshMaterial;
    [SerializeField] private Material meshMaterial2;

    // Helper obstacles are added to reach height
    [SerializeField] private GameObject[] helperObstacles;
    [SerializeField] private int[] obstacleHeights;
    [SerializeField] private int obstacleHeight;
    
    // NoHelper obstacles are not added to reach height
    [SerializeField] private GameObject[] noHelpObstacles;
    [SerializeField] private int[] noHelpObstacleHeights;
    [SerializeField] private int noHelpObstacleHeight;

    [SerializeField] GameObject[] startEndPointsList;
    [SerializeField] private GameObject startPlatform;
    [SerializeField] private GameObject endPlatform;

    private MovePlatform[] movePlatf;

    private void Awake()
    {
        obstacleHeight = 0;
        noHelpObstacleHeight = 0;

        playerBoxes = numberOfBoxes;

        GetObstacleHeights();
        GetReachHeight();
        
        spawnPoint = GameObject.Find("Pickup Point").GetComponent<Transform>();
        
        startPlatform = startEndPointsList[0];
        endPlatform = startEndPointsList[startEndPointsList.Length -1];
        meshMaterial = endPlatform.GetComponentInChildren<MeshRenderer>().sharedMaterial;

    }

    private void Update()
    {
        CheckPossible();
        LevelChecks();
    }

    private void GetObstacleHeights()
    {
        helperObstacles = GameObject.FindGameObjectsWithTag("HelpObstacle");
        noHelpObstacles = GameObject.FindGameObjectsWithTag("NoHelpObstacle");

        for (int i = 0; i < helperObstacles.Length; i++)
        {
            obstacleHeight += Mathf.RoundToInt(helperObstacles[i].GetComponent<BoxCollider>().size.y);
            helperObstacles[i].GetComponent<MeshRenderer>().sharedMaterial.color = Color.blue;
        }

        for (int i = 0; i < noHelpObstacles.Length; i++)
        {
            noHelpObstacleHeight += Mathf.RoundToInt(noHelpObstacles[i].GetComponent<BoxCollider>().size.y);
            noHelpObstacles[i].GetComponent<MeshRenderer>().sharedMaterial.color = Color.red;
        }
    }

    private void GetStartToEnd()
    {
        startToEndDistance = endPlatform.transform.position.y - startPlatform.transform.position.y;
    }

    private void GetReachHeight()
    {
        boxHeight = Mathf.RoundToInt(boxPrefab.GetComponent<BoxCollider>().size.y);
        totalBoxHeight = boxHeight * numberOfBoxes;
        totalReachHeight = totalBoxHeight + jumpHeight + obstacleHeight;
    }

    private bool CheckPossible()
    {
        GetStartToEnd();
        GetReachHeight();

        for (int i = 0; i < startEndPointsList.Length; i++)
        {
            if (i == 0) { i = 1; }

            if (!AtoB2(startEndPointsList[i].GetComponent<MovePlatform>().GetCenterPos(), startEndPointsList[i - 1].GetComponent<MovePlatform>().GetCenterPos(), startEndPointsList[i - 1].GetComponent<MovePlatform>().GetDistanceToNext()))
            {
                Debug.Log("Distance Between: " + startEndPointsList[i] + " and: " + startEndPointsList[i - 1] + " too far");
                levelPossible = false;
                return false;
            }
            
        }
        levelPossible = true;
        return true;

    }

    private void LevelChecks()
    {
        // Sets platform colour if level is possible
        if (!levelPossible)
        {
            meshMaterial.color = Color.red;
        }
        else
        {
            meshMaterial.color = Color.green;
        }
    }

    private bool AtoB2(Vector3 pos1, Vector3 pos2, int dist)
    {
        
        if (Vector3.Distance(pos1, pos2) < dist)
        {
            levelPossible = true;
            return true;
        }
        else
        {
            print("Problem Distance: " + Vector3.Distance(pos1, pos2));
            levelPossible = false;
            return false;
        }
    }

    public void SpawnPrefab()
    {
        while(playerBoxes > 0)
        {
            // Spawns boxes as random colour
            GameObject gObj =  Instantiate(boxPrefab, spawnPoint.position, Quaternion.identity) as GameObject;
            gObj.GetComponent<MeshRenderer>().material.color = new Color(Random.Range (0.0f, 1f), Random.Range(0.0f, 1f), Random.Range(0.0f, 1f));
            playerBoxes -= 1;
            Debug.Log("Number of boxes: " + playerBoxes);
            return;
        }
    }

    public void CollectBoxes()
    {
        var boxes = GameObject.FindGameObjectsWithTag("Boxes");
        for (var i = 0; i < boxes.Length; i++)
        {
            Destroy(boxes[i]);
        }
        playerBoxes = numberOfBoxes;
    }


    // Inspector functions
    public void SetToPossible()
    {
        InvokeRepeating(nameof(SetPlatformHeight), 0, 0.01f);
    }

    public void SetToRandomHeight()
    {
        Vector3 position = startEndPointsList[1].transform.position;
        position.y = Random.Range(10, 40);
        startEndPointsList[1].transform.position = position;
    }

    public void AddBoxesNeeded()
    {
        if (!levelPossible)
        {
            int boxesRequired = Mathf.RoundToInt((int)((startToEndDistance - totalReachHeight) / boxHeight));
            Debug.Log("Boxes Required: " + boxesRequired);
            numberOfBoxes += boxesRequired;
            CheckPossible();
        }
    }

    public void SetToExactBoxes()
    {
        int boxesRequired = Mathf.RoundToInt((startToEndDistance - obstacleHeight - jumpHeight - noHelpObstacleHeight) / boxHeight);
        Debug.Log("Boxes Required to exact: " + boxesRequired + " Start to End: " + startToEndDistance + " Obstacle Height: " + obstacleHeight + " No help obstacle height: " + noHelpObstacleHeight );
        numberOfBoxes = boxesRequired;
        CheckPossible();
    }

    public void SetPlatformHeight()
    {
        if (!levelPossible)
        {
            startEndPointsList[1].transform.position -= new Vector3(0, 1f, 0);
        }
        else
        {
            CancelInvoke("MovePlatform");
        }
    }


}
