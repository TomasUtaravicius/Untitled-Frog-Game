using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    

    [Header("Input")]
    public InputHandler inputHandler;

    [Header("Camera")]
    public CameraHandler cameraHandler;


    [Header("Player")]
    public PlayerLocomotionManager playerLocomotionManager;
    public PlayerAnimatorHandler playerAnimatorHandler;
    public PlayerStatsManager playerStatsManager;
    public PlayerInventoryManager playerInventoryManager;
    public PlayerEffectsManager playerEffectsManager;
    public PlayerWeaponSlotManager playerWeaponSlotManager;
    public PlayerCombatManager playerCombatManager;
    public PlayerEquipmentManager playerEquipmentManager;

    [Header("UI")]
    InteractableUI interactableUI;
    public QuickSlotsUI quickSlotsUI;
    public UIManager uiManager;
    public GameObject interactableUIGameObject;
    public GameObject itemInteractableGameObject;

    public LayerMask detectionLayer;
    public LayerMask layerBlockingLineOfSight;

    public float stayInCombatTimer;
    private float lastCombatTimer;
    public bool isDrawing;

    protected override void Awake()
    {
        base.Awake();
        isInCombat = false;
        cameraHandler = FindObjectOfType<CameraHandler>();
        backstabCollider = GetComponentInChildren<CriticalDamageCollider>();

        interactableUI = FindObjectOfType<InteractableUI>();
        inputHandler = GetComponent<InputHandler>();
        
        animator = GetComponent<Animator>();

        playerStatsManager = GetComponent<PlayerStatsManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorHandler = GetComponent<PlayerAnimatorHandler>();
        playerEffectsManager = GetComponent<PlayerEffectsManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerCombatManager = GetComponent<PlayerCombatManager>();
        playerWeaponSlotManager = GetComponent<PlayerWeaponSlotManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        uiManager = FindObjectOfType<UIManager>();
        quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
    }

    protected override void FixedUpdate()
    {
        //base.FixedUpdate();
        playerAnimatorHandler.CheckHandIKWeight(playerWeaponSlotManager.rightHandIKTarget, playerWeaponSlotManager.leftHandIKTarget, isTwoHanding);
        float delta = Time.fixedDeltaTime;

        
        //playerLocomotionManager.HandleMovement();
       // playerLocomotionManager.HandleRotation();
        if (cameraHandler != null)
        {
            
            cameraHandler.FollowTarget(delta);
            if (isDead)
                return;
            cameraHandler.HandleCameraRotation();
        }
    }
   

    // Update is called once per frame
    void Update()
    {
        isInteracting = animator.GetBool("IsInteracting");
        canDoCombo = animator.GetBool("CanDoCombo");
        canRotate = animator.GetBool("CanRotate");
        isInvulnerable = animator.GetBool("IsInvulnerable");
        isFiringSpell = animator.GetBool("IsFiringSpell");
        isHoldingArrow = animator.GetBool("IsHoldingArrow");
        isPerformingFullyChargedAttack = animator.GetBool("IsPerformingFullyCharged");
        isAiming = animator.GetBool("IsAiming");
        animator.SetBool("IsDead", isDead);
        animator.SetBool("IsInAir", isInAir);
        animator.SetBool("IsBlocking", isBlocking);

        animator.SetBool("IsTwoHanding", isArmed);
     

        inputHandler.TickInput();
        //playerLocomotionManager.HandleRollingAndSprinting();
        //playerLocomotionManager.HandleFalling(playerLocomotionManager.moveDirection);
        //playerLocomotionManager.HandleJumping();
        playerStatsManager.RegenerateStamina();

        HandleInCombatState();
       

        //CheckForInteractable();
    }
    private void LateUpdate()
    {
        ResetFlags();
        ResetInputs();
  
        /*if(isInAir)
        {
            playerLocomotionManager.inAirTimer += Time.deltaTime;
        }*/
        
    }
    private void ResetFlags()
    {
        inputHandler.rollFlag = false;
    }
    private void ResetInputs()
    {
        
        inputHandler.d_Pad_Up = false;
        inputHandler.d_Pad_Right = false;
        inputHandler.d_Pad_Left = false;
        inputHandler.d_Pad_Down = false;
        inputHandler.a_Input = false;
        inputHandler.jump_Input = false;
        inputHandler.inventory_Input = false;
    }
    public void HandleInCombatState()
    {
        if (isInCombat)
        {
            if(!isArmed)
            {
                if (isDrawing)
                    return;
                DrawSword();
            }
            lastCombatTimer = Time.time;
        }
        isInCombat = CheckForEnemies();

        if(isArmed && !isInCombat)
        {
            if(Time.time>=lastCombatTimer+stayInCombatTimer)
            {
                SheathSword();
            }
        }
    }
    public void DrawSword()
    {
        playerAnimatorHandler.PlayTargetAnimation("Draw Sword 1", false);
        isDrawing = true;
        isArmed = true;
    }
    public void SheathSword()
    {
        Debug.LogWarning("Sheating Sword");
        playerAnimatorHandler.PlayTargetAnimation("Sheath Sword 1", false);
        isArmed = false;
    }
    public void TakeSword()
    {
        playerWeaponSlotManager.LoadBothWeaponsOnSlots();
        isDrawing = false;
    }
    public void PutBackSword()
    {
        playerWeaponSlotManager.LoadBothWeaponsOnSlots();
    }
    public bool CheckForEnemies()
    {
        //Player ID is 1
        //Enemy ID is 0

        Collider[] colliders = Physics.OverlapSphere(transform.position, 15f, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();


            if (targetCharacter.gameObject.name != gameObject.name)
            {
                if (targetCharacter != null)
                {

                    if (targetCharacter.characterStatsManager.teamIDNumber == 1)
                    {
                        return true;
                    }
                    if (targetCharacter.characterStatsManager.teamIDNumber == characterStatsManager.teamIDNumber)
                    {
                        return false;
                    }
                    if (targetCharacter.isDead)
                        return false;

                    return true;
                    /*Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    //if potential target is found it has to be standing infront of the AI's field of view
                    if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximimumDetectionAngle)
                    {
                        //If our potential target has an obstacle in between, don't do anything
                        if (Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layerBlockingLineOfSight))
                        {
                            return this;
                        }
                        else
                        {
                            aiCharacter.currentTarget = targetCharacter;
                        }

                    }*/
                }
            }

        }
        return false;
    }
    public void CheckForInteractable()
    {
        RaycastHit hit;

        if(Physics.SphereCast(transform.position,1f,transform.forward,out hit, 1f, cameraHandler.ignoreLayers))
        {

            if (hit.collider.tag == "Interactable")
            {
                Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                if(interactableObject!=null)
                {
                    string interactableText = interactableObject.interactableText;

                    interactableUI.interactableText.text = interactableText;
                    if(interactableUIGameObject)
                    interactableUIGameObject.SetActive(true);

                    if (inputHandler.a_Input)
                    {
                        hit.collider.GetComponent<Interactable>().Interact(this);
                    }

                }
            }
        }
        else
        {
            if(interactableUIGameObject!=null)
            {
                interactableUIGameObject.SetActive(false);
            }
            if(itemInteractableGameObject!=null && inputHandler.a_Input)
            {
                itemInteractableGameObject.SetActive(false);
            }
        }
    }
    public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
    {
        transform.position = playerStandsHereWhenOpeningChest.transform.position;
        playerAnimatorHandler.PlayTargetAnimation("Open Chest", true);
        //playerLocomotionManager.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }
}
