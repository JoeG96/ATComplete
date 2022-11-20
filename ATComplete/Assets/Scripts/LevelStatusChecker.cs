using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStatusChecker : MonoBehaviour
{
    // Game idea for mechanic
    // Player has a certain number of boxes to stack and climb to escape level
    // Boxes have set size, if distance away from goal area (add collision box by exit) is greater than boxes * box height then display issue

    // Array of collectable objects
    // Check all collectables
    // Check if collectables left 

    [SerializeField] public GameObject startingPoint;
    [SerializeField] public GameObject endingPoint;
    [SerializeField] public GameObject[] boxes;

    [SerializeField] public int jumpHeight;
    [SerializeField] int boxHeight;
    private int totalBoxHeight;
    private int totalReachHeight;
    private float startToEndDistance;


    private void Start()
    {
        totalBoxHeight = boxHeight * boxes.Length;
        totalReachHeight = totalBoxHeight + jumpHeight;
        Debug.Log("Total Box Height: " + totalBoxHeight);
        Debug.Log("Total Reach Height: " + totalReachHeight);


        //getStartToEnd();
    }

    private void Update()
    {
        CheckPossible();
    }

    private void getStartToEnd()
    {

        startToEndDistance = endingPoint.transform.position.y - startingPoint.transform.position.y;
        //Debug.Log("Start to End Distance: " + startToEndDistance);
    }

    private bool CheckPossible()
    {
        getStartToEnd();
        if (startToEndDistance <= totalReachHeight)
        {
            Debug.Log("Level Possible");
            return true;
        }
        else
        {
            Debug.Log("Level Not Possible");
            Debug.Log("Start to End Distance: " + startToEndDistance + " Distance too great");
            return false;
        }
        
    }


}
