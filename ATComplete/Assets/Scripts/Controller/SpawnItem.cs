using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour
{

    private Camera playerCamera;
    private PlayerControls controls;

    public GameObject prefab;


    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        controls = new PlayerControls();
    }

    private void Update()
    {
        /*if (controls.Player.Y.triggered)
        {
            Debug.Log("Interact Triggered");
            SpawnPrefab();
        }*/
    }

    private void SpawnPrefab()
    {
        Debug.Log("Spawn Prefab Method");
        RaycastHit hit;
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            Instantiate(prefab, hit.point, Quaternion.identity);
            Debug.Log("Prefab Spawned");
        }
        
    }
}
