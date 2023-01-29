using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelStatusChecker : MonoBehaviour
{
    // Game idea for mechanic
    // Player has a certain number of boxes to stack and climb to escape level
    // Boxes have set size, if distance away from goal area (add collision box by exit) is greater than boxes * box height then display issue
    
    // Issues:
    //      Vertical moving - horizontal moving *should be* fine as Y position doesn't change -side-lined 
    //      Horizontal moving - make work
    //          - Put movement in seperate script on platform then fetch starting point that from here?
    //      Issue with stacking - need to pyramid for tall heights but how is that included in reach?
    //      Add Pop-ups when can't do something or win
    //      Add controls on screen - done 
    //      Add button to reset boxes - make that a mechanic - done
    //      Make it more colourful?

    // For Next Week
    //  -   Movement
    //  -   Make it more dynamic with platform spacing
    //  -   Use or number value to set distance between platforms in AtoBCheck
    //  -   Get distance to next platform from platform object

    
    
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
    [SerializeField] private int platformSpaceDistance;

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
        //meshMaterial2 = startPlatform.GetComponentInChildren<MeshRenderer>().sharedMaterial;

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
            //movePlatf = startEndPointsList[i].GetComponents<MovePlatform>();
            if (!AtoBDistanceCheck(startEndPointsList[i], startEndPointsList[i - 1], platformSpaceDistance /*movePlatf[i].distToNextPlatform*/))
            {
                Debug.Log("Check " + startEndPointsList[i] + " ");
                //Debug.Log("distToNextPlatform " + movePlatf[i].distToNextPlatform + " ");
                levelPossible = false;
                return false;
            }
            
        }
        levelPossible = true;
        return true;

        /*if (!AtoBDistanceCheck(startEndPointsList[1], startEndPointsList[0], true))
        {
            //Debug.Log("Can't Reach 1st Platform");
            levelPossible = false;
            return false;
        }
        if (!AtoBDistanceCheck(startEndPointsList[2], startEndPointsList[1], false))
        {
            //Debug.Log("Can't Reach 2nd Platform");
            levelPossible = false;
            return false;
        }
        if (!AtoBDistanceCheck(startEndPointsList[3], startEndPointsList[2], false))
        {
            //Debug.Log("Can't Reach 3rd Platform");
            levelPossible = false;
            return false;
        }
        if (!AtoBDistanceCheck(startEndPointsList[4], startEndPointsList[3], false))
        {
            //Debug.Log("Can't Reach 4th Platform");
            levelPossible = false;
            return false;
        }
        if (!AtoBDistanceCheck(startEndPointsList[5], startEndPointsList[4], false))
        {
            //Debug.Log("Can't Reach 5th Platform");
            levelPossible = false;
            return false;
        }*/


        /*if (startToEndDistance <= totalReachHeight) //|| minPlatformHeight <= totalReachHeight)
        {
            levelPossible = true;
            return true;
        }
        else
        {
            
        }*/
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

    private bool AtoBDistanceCheck(GameObject gObj1, GameObject gObj2, int distToNext)
    {   // Compares distance between platforms, addedheights check is to include boxes and helper obstacle height instead of just jump height
        //float distance;
        
/*        if (addedHeights)
        {
            distance = totalReachHeight;
        }
        else
        {
            distance = jumpHeight * 1.6f; // jump distance/height
        }*/

        

        if (Vector3.Distance(gObj1.transform.position, gObj2.transform.position) < distToNext)
        {
            levelPossible = true;
            return true;
        }
        else
        {
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
