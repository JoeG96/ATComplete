using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelStatusChecker : MonoBehaviour
{
    // Game idea for mechanic
    // Player has a certain number of boxes to stack and climb to escape level
    // Boxes have set size, if distance away from goal area (add collision box by exit) is greater than boxes * box height then display issue

    // Array of collectable objects
    // Check all collectables
    // Check if collectables left 

    [SerializeField] private GameObject startingPoint;
    [SerializeField] private GameObject endingPoint;
    
    public int jumpHeight;
    [SerializeField] private int boxHeight;
    [SerializeField] private int totalBoxHeight;
    [SerializeField] private int totalReachHeight;
    [SerializeField] private float startToEndDistance;

    [SerializeField] private int numberOfBoxes;
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private bool levelPossible;

    [SerializeField] private Material meshMaterial;

    [SerializeField] private GameObject[] helperObstacles;
    [SerializeField] private int[] obstacleHeights;
    [SerializeField] private int obstacleHeight;

    public bool isMoving;
    [SerializeField] private int startPlatformHeight;
    [SerializeField] private int minPlatformHeight;
    [SerializeField] private int maxPlatformHeight;
    [SerializeField] private int platformMovingSpeed;
    private float height = 0.5f;


    private void Awake()
    {
        obstacleHeight = 0;
        
        GetObstacleHeights();
        GetReachHeight();
        
        startingPoint = GameObject.Find("StartPoint");
        endingPoint = GameObject.Find("EndPoint");
        spawnPoint = GameObject.Find("Pickup Point").GetComponent<Transform>();
        meshMaterial = GameObject.Find("EndPlatform").GetComponent<MeshRenderer>().sharedMaterial;

        endingPoint.transform.position = new Vector3(endingPoint.transform.position.x, startPlatformHeight, endingPoint.transform.position.z);

    }

    private void Update()
    {
        CheckPossible();
        LevelChecks();
        MovePlatform();
        if (!isMoving)
        {
            MovePlatform();
        }
    }

    private void MovePlatform()
    {
        if (isMoving)
        {
            var pos = endingPoint.transform.position;
            var newY = startPlatformHeight + height * Mathf.Sin(Time.time * platformMovingSpeed);
            endingPoint.transform.position = new Vector3(pos.x, newY, pos.z);

            //float y = Mathf.PingPong(Time.time * platformMovingSpeed, 1) * maxPlatformHeight - minPlatformHeight;
            //endingPoint.transform.position = new Vector3(endingPoint.transform.position.x, y, endingPoint.transform.position.z);
        }
        else
        {
            return;
        }
    }

    private void GetObstacleHeights()
    {
        for (int i = 0; i < helperObstacles.Length; i++)
        {

            obstacleHeight += Mathf.RoundToInt(helperObstacles[i].GetComponent<BoxCollider>().size.y);
            //obstacleHeight += obstacleHeights[i];
            Debug.Log("Obstacle Heights: " + obstacleHeight);
        }

        //Debug.Log("Obstacle Height: " + obstacleHeight);
    }

    private void GetStartToEnd()
    {
        startToEndDistance = endingPoint.transform.position.y - startingPoint.transform.position.y;
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
        if (startToEndDistance <= totalReachHeight)
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

    private void LevelChecks()
    {
        

        if (!levelPossible)
        {
            Debug.Log("Level Not Possible");
            Debug.Log("Start to End Distance: " + startToEndDistance + " Distance too great");
            meshMaterial.color = Color.red;
        }
        else
        {
            Debug.Log("Level Possible");
            meshMaterial.color = Color.green;

        }
    }

    public void SpawnPrefab()
    {
        while(numberOfBoxes > 0)
        {
            Instantiate(boxPrefab, spawnPoint.position, Quaternion.identity);
            numberOfBoxes -= 1;
            Debug.Log("Number of boxes: " + numberOfBoxes);
            return;
        }
    }

    // Inspector functions
    public void SetToPossible()
    {
        InvokeRepeating(nameof(SetPlatformHeight), 0, 0.01f);

    }

    public void SetToRandomHeight()
    {
        Vector3 position = endingPoint.transform.position;
        position.y = Random.Range(10, 40);
        endingPoint.transform.position = position;
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
        int boxesRequired = Mathf.RoundToInt((startToEndDistance - obstacleHeight - jumpHeight) / boxHeight);
        Debug.Log("Boxes Required to exact: " + boxesRequired + " Start to End: " + startToEndDistance + " Obstacle Height: " + obstacleHeight );
        numberOfBoxes = boxesRequired;
        CheckPossible();
    }

    private void SetPlatformHeight()
    {
        if (!levelPossible)
        {
            endingPoint.transform.position -= new Vector3(0, 1f, 0);
        }
        else
        {
            CancelInvoke("MovePlatform");
        }
    }


}
