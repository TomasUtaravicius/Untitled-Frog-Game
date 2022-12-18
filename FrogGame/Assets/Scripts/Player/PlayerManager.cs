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

    protected override void Awake()
    {
        base.Awake();
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
        base.FixedUpdate();
        float delta = Time.fixedDeltaTime;

        
        playerLocomotionManager.HandleMovement();
        playerLocomotionManager.HandleRotation();
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

        animator.SetBool("IsDead", isDead);
        animator.SetBool("IsInAir", isInAir);
        animator.SetBool("IsBlocking", isBlocking);
        animator.SetBool("IsTwoHanding", isTwoHanding);
       
     

        inputHandler.TickInput();
        playerLocomotionManager.HandleRollingAndSprinting();
        playerLocomotionManager.HandleFalling(playerLocomotionManager.moveDirection);
        playerLocomotionManager.HandleJumping();
        playerStatsManager.RegenerateStamina();

       

        CheckForInteractable();
    }
    private void LateUpdate()
    {
        ResetFlags();
        ResetInputs();
  
        if(isInAir)
        {
            playerLocomotionManager.inAirTimer += Time.deltaTime;
        }
        
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
        playerLocomotionManager.rigidbody.velocity = Vector3.zero;
    }
}
