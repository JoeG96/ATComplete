using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController: MonoBehaviour
{

    private bool isSprinting => canSprint && controls.Player.Sprint.IsPressed() && characterController.isGrounded;
    private bool shouldJump => controls.Player.Jump.triggered && characterController.isGrounded;
    private bool shouldCrouch => controls.Player.Crouch.triggered && !duringCrouchAnimation && characterController.isGrounded;

    private PlayerControls controls;
    private Vector2 move;
    private CharacterController characterController;
    private Camera playerCamera;

    private bool canSprint = true;

    [Header("Crouch Parameters")]
    [SerializeField] private float crouchHeight = 0.5f;
    [SerializeField] private float standingHeight = 2.0f;
    [SerializeField] private float timeToCrouch = 0.25f;
    private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    private Vector3 standingCenter = new Vector3(0, 0, 0);
    private bool isCrouching;
    private bool duringCrouchAnimation;

    [Header("Headbob Parameters")]
    [SerializeField] private float walkBobSpeed = 14f;
    [SerializeField] private float walkBobAmount = 0.05f;
    [SerializeField] private float sprintBobSpeed = 18f;
    [SerializeField] private float sprintBobAmount = 0.1f;
    [SerializeField] private float crouchBobSpeed = 8f;
    [SerializeField] private float crouchBobAmount = 0.025f;
    private float defaultYPos = 0;
    private float timer;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 4.0f;
    [SerializeField] private float sprintSpeed = 8.0f;
    [SerializeField] private float crouchSpeed = 2.0f;
    private float slopeSpeed = 8.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float gravity = 30.0f;
    [SerializeField] private float jumpForce = 8.0f;

    [Header("Footstep Parameters")]
    [SerializeField] private float baseStepSpeed = 0.6f;
    [SerializeField] private float crouchStepMultiplier = 1.5f;
    [SerializeField] private float sprintStepMultiplier = 0.6f;
    [SerializeField] private AudioSource footstepAudioSource = default;
    [SerializeField] private AudioClip[] woodClips = default;
    [SerializeField] private AudioClip[] stoneClips = default;
    [SerializeField] private AudioClip[] grassClips = default;
    private float footstepTimer = 0;
    private float GetCurrentOffset => isCrouching ? baseStepSpeed * crouchStepMultiplier : isSprinting ? baseStepSpeed * sprintStepMultiplier : baseStepSpeed;

    private Vector3 hitPointNormal;

    private bool isSliding
    {
        get
        {
            if (characterController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2f))
            {
                hitPointNormal = slopeHit.normal;
                return Vector3.Angle(hitPointNormal, Vector3.up) > characterController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }

    [Header("Interaction")]
    [SerializeField] private Vector3 interactionRayPoint = default;
    [SerializeField] private float interactionDistance = default;
    [SerializeField] private LayerMask interactionLayer = default;
    private Interactable currentInteractable;

    private Vector3 moveDirection;
    private Vector2 currentInput;

    public static PlayerController instance;

    [Header("PhysicsPickup")]
    [SerializeField] private LayerMask pickupMask;
    private Transform pickupTarget;
    [SerializeField] private float pickupRange;
    private Rigidbody currentObject;

    private LevelStatusChecker levelChecker;

    private void Awake()
    {
        instance = this;
        controls = new PlayerControls();
        characterController = GetComponent<CharacterController>();
        playerCamera = GetComponentInChildren<Camera>();
        defaultYPos = playerCamera.transform.localPosition.y;
        pickupTarget = GameObject.Find("Pickup Point").GetComponent<Transform>();
        levelChecker = GameObject.Find("GameManager").GetComponent<LevelStatusChecker>();
    }

    void Update()
    {
        Movement();
        Jump();
        Crouch();
        HeadBob();
        Footsteps();
        InteractionCheck();
        InteractionInput();
        ApplyFinalMovements();
        PhysicsPickup();
        SpawnPrefab();

        //DebugStuff();

    }

    private void Movement()
    {
        move = controls.Player.Move.ReadValue<Vector2>();       
        currentInput = new Vector2((isCrouching ? crouchSpeed : isSprinting ? sprintSpeed : walkSpeed) * move.y,
              (isCrouching ? crouchSpeed :  walkSpeed) * move.x); // isSprinting? sprintSpeed : / Doesn't allow sprinting on side movement

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x)
            + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;

        if (currentInput.x > 1 && currentInput.y >= 0) // Limits sprinting to forward movement
        {
            canSprint = true;
        }
        else
        {
            canSprint = false;
        }
        
    }

    private void Jump()
    {
        if(shouldJump)
        {
            moveDirection.y = jumpForce;
        }
    }

    private void Crouch()
    {
        if (shouldCrouch)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private void HeadBob()
    {
        if (!characterController.isGrounded)
        {
            return;
        }

        if (Mathf.Abs(moveDirection.x) > 0.1f || Mathf.Abs(moveDirection.z) > 0.1f)
        {
            timer += Time.deltaTime * (isCrouching ? crouchBobSpeed : isSprinting ? sprintBobSpeed : walkBobSpeed);
            playerCamera.transform.localPosition = new Vector3
                (playerCamera.transform.localPosition.x,
                defaultYPos + Mathf.Sin(timer) * (isCrouching ? crouchBobAmount : isSprinting ? sprintBobAmount : walkBobAmount),
                playerCamera.transform.localPosition.z);
        }
    }

    private void Footsteps()
    {
        if (!characterController.isGrounded)
        {
            return;
        }
        if (move == Vector2.zero)
        {
            return;
        }

        footstepTimer -= Time.deltaTime;
        if (footstepTimer <= 0)
        {
            int layerMask = (-1) - (1 << LayerMask.NameToLayer("Player")); // Every Layer except "Player" layer
            if (Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 2, layerMask))
            {
                switch (hit.collider.tag)
                {
                    case "Footsteps/Grass":
                        footstepAudioSource.PlayOneShot(grassClips[Random.Range(0, grassClips.Length - 1)]);
                        break;
                    case "Footsteps/Wood":
                        footstepAudioSource.PlayOneShot(woodClips[Random.Range(0, woodClips.Length - 1)]);
                        break;
                    case "Footsteps/Stone":
                        footstepAudioSource.PlayOneShot(stoneClips[Random.Range(0, stoneClips.Length - 1)]);
                        break;
                    default:
                        //footstepAudioSource.PlayOneShot(stoneClips[Random.Range(0, stoneClips.Length - 1)]);
                        break;
                }
            }

            footstepTimer = GetCurrentOffset;
        }
    }

    private void InteractionCheck()
    {
        if (Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance))
        {
            if (hit.collider.gameObject.layer == 10 && (currentInteractable == null || 
                hit.collider.gameObject.GetInstanceID() != currentInteractable.GetInstanceID()))
            {
                hit.collider.TryGetComponent(out currentInteractable);
                if (currentInteractable)
                {
                    currentInteractable.OnFocus();
                }
            }
        }
        else if (currentInteractable)
        {
            currentInteractable.OnLoseFocus();
            currentInteractable = null;
        }
    }    
    private void InteractionInput()
    {
        if (controls.Player.Interact.triggered && currentInteractable != null  && Physics.Raycast(playerCamera.ViewportPointToRay(interactionRayPoint), out RaycastHit hit, interactionDistance, interactionLayer))
        {
            currentInteractable.OnInteract();
        }
    }

    private void SpawnPrefab()
    {
        //Debug.Log("Spawn Prefab Method");
        //RaycastHit hit;
        //Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));

        if (controls.Player.Y.triggered)
        {
            levelChecker.SpawnPrefab();
            //if (Physics.Raycast(ray, out hit, 100.0f)){}
        }
    }

    private void PhysicsPickup()
    {
        if (controls.Player.RightShoulder.triggered)
        {
            if (currentObject)
            {
                currentObject.useGravity = true;
                currentObject = null;
                return;
            }

            Ray cameraRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));
            if (Physics.Raycast(cameraRay, out RaycastHit hit, pickupRange, pickupMask))
            {
                currentObject = hit.rigidbody;
                currentObject.useGravity = false;
            }

        }
    }

    private void ApplyFinalMovements()
    {
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (characterController.velocity.y < -1 && characterController.isGrounded)
        {
            moveDirection.y = 0;
        }
        
        if (isSliding)
        {
            moveDirection += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z) * slopeSpeed;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (currentObject)
        {
            Vector3 directionToPoint = pickupTarget.position - currentObject.position;
            float distanceToPoint = directionToPoint.magnitude;

            currentObject.velocity = directionToPoint * 12f * distanceToPoint;
        }
    }

    private IEnumerator CrouchStand()
    {
        if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
        {
            yield break;
        }
        
        
        duringCrouchAnimation = true;

        float timeElapsed = 0;
        float targetHeight = isCrouching ? standingHeight : crouchHeight;
        float currentHeight = characterController.height;
        Vector3 targetCenter = isCrouching ? standingCenter : crouchingCenter;
        Vector3 currentCenter = characterController.center;

        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        characterController.height = targetHeight;
        characterController.center = targetCenter;

        isCrouching = !isCrouching;

        duringCrouchAnimation = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }

    private void DebugStuff()
    {
        if (controls.Player.Sprint.triggered)
        {
            Debug.Log("Sprinting");
        }
        Debug.Log("Sprinting? " + isSprinting);
        Debug.Log("Current X Input: " + currentInput.x + " Current Y: Input: " + currentInput.y);
    }

}
