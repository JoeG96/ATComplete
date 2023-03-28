using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MovePlatform : MonoBehaviour
{

    public int distToNextPlatform;

    [SerializeField] public bool isMoving;
    [SerializeField] public bool isMovingH;
    [SerializeField] public bool isMovingV;

    [SerializeField] private int startPlatformHeight;
    [SerializeField] private int minPlatformHeight;
    [SerializeField] private int maxPlatformHeight;

    [SerializeField] private int startPlatformXValue;
    [SerializeField] private int minPlatformXValue;
    [SerializeField] private int maxPlatformXValue;

    [SerializeField] private Vector3 maxPlatformHeightVector;
    [SerializeField] private Vector3 minPlatformHeightVector;

    [SerializeField] private int platformMovingSpeed;
    private float height;

    [SerializeField] Vector3 platformStartPos;
    [SerializeField] Vector3 platformPointA;
    [SerializeField] Vector3 platformPointB;
    [SerializeField] float platformMoveSpeed;
    [SerializeField] int distanceToNextPlatform;
    

    private void Awake()
    {
        height = maxPlatformHeight - minPlatformHeight;
        maxPlatformHeightVector = new Vector3(transform.position.x, maxPlatformHeight, transform.position.z);
        minPlatformHeightVector = new Vector3(transform.position.x, minPlatformHeight, transform.position.z);

    }

    private void Update()
    {
        movePlatform();
        
    }

    private void movePlatform()
    {
        if (isMoving)
        {

            float time = Mathf.PingPong(Time.time * platformMoveSpeed, 1);
            transform.position = Vector3.Lerp(platformPointA, platformPointB, time);

        }
        else
        {
            return;
        }
    }

    public Vector3 GetCenterPos()
    {
        return platformStartPos;
    }

    public int GetDistanceToNext()
    {
        return distanceToNextPlatform;
    }

    public void SetCenterPoint()
    {
        platformStartPos = transform.position;
    }
    
    public void SetPointA()
    {
        platformPointA = transform.position;
    }
    
    public void SetPointB()
    {
        platformPointB = transform.position;
    }

    public void PlaceAtCenter()
    {
        transform.position = platformStartPos;
    }



}
