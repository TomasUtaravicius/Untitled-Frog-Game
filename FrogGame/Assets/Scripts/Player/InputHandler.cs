using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    public float horizontal;
    public float vertical;
    public float moveAmount;
    public float mouseX;
    public float mouseY;

    PlayerControls inputActions;
    PlayerManager player;

    public Vector2 movementInput;
    public Vector2 cameraInput;

    public bool b_Input;
    public bool a_Input;
    public bool y_Input;
    public bool x_Input;

    public bool tap_rb_Input;
    public bool tap_rt_Input;
    public bool tap_lb_Input;
    public bool tap_lt_Input;

    public bool hold_rb_Input;
    public bool hold_lb_Input;
    public bool hold_rt_Input;
    public bool hold_lt_Input;

    public bool d_Pad_Up;
    public bool d_Pad_Down;
    public bool d_Pad_Left;
    public bool d_Pad_Right;
    
    public bool jump_Input;
    public bool inventory_Input;
    public bool lockOn_Input;
    public bool right_Stick_Right_Input;
    public bool right_Stick_Left_Input;

    public bool rollFlag;
    public bool twoHandFlag;
    public bool sprintFlag;
    public bool comboFlag;
    public bool lockOnFlag;
    public bool inventoryFlag;
    public bool fireFlag;

    public Player playerController;
    public float rollInputTimer;
    
    
    public void OnEnable()
    {
        if(inputActions == null)
        {
            inputActions = new PlayerControls();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();

            inputActions.PlayerActions.HoldRB.performed += i => hold_rb_Input = true;
            inputActions.PlayerActions.HoldRB.canceled += i => hold_rb_Input = false;

            inputActions.PlayerActions.HoldRT.performed += i => hold_rt_Input = true;
            inputActions.PlayerActions.HoldRT.canceled += i => hold_rt_Input = false;

            inputActions.PlayerActions.HoldLB.performed += i => hold_lb_Input = true;
            inputActions.PlayerActions.HoldLB.canceled += i => hold_lb_Input = false;

            inputActions.PlayerActions.HoldLT.performed += i => hold_lt_Input = true;
            inputActions.PlayerActions.HoldLT.canceled += i => hold_lt_Input = false;

            inputActions.PlayerActions.TapRB.performed += i => tap_rb_Input = true;
            inputActions.PlayerActions.TapRT.performed += i => tap_rt_Input = true;
            inputActions.PlayerActions.TapLT.performed += i => tap_lt_Input = true;
            inputActions.PlayerActions.TapLB.performed += i => tap_lb_Input = true;
            //inputActions.PlayerActions.LB.canceled += i => tap_lb_Input = false;

            inputActions.PlayerQuickSlots.DPadRight.performed += i => d_Pad_Right = true;
            inputActions.PlayerQuickSlots.DPadLeft.performed += i => d_Pad_Left = true;
            inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
            inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;
            inputActions.PlayerActions.A.performed += i => a_Input = true;
            inputActions.PlayerActions.X.performed += i => x_Input = true;
            inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
            inputActions.PlayerActions.Roll.performed += i => b_Input = true;

            inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
            inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
            inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
            inputActions.PlayerActions.Y.performed += i => y_Input = true;
            
        }
        inputActions.Enable();
    }
    private void Awake()
    {
        player = GetComponent<PlayerManager>();
        twoHandFlag = true;
    }
    

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void TickInput()
    {
        if (player.isDead)
            return;
        HandleMoveInput();
        HandleRollInput();

        HandleTapRBInput();
        HandleTapLBInput();
        HandleTapRTInput();
        HandleTapLTInput();

        HandleHoldRBInput();
        HandleHoldLBInput();
        HandleHoldRTInput();

        HandleQuickSlotInput();
        HandleInteractableInput();
        HandleJumpInput();
        HandleInventoryInput();
        HandleLockOnInput();
        HandleTwoHandedInput();
        HandleUseConsumableInput();
    }
    private void HandleMoveInput()
    {
        if(player.isHoldingArrow)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp(Mathf.Abs(horizontal) + Mathf.Abs(vertical), 0f, 0.5f);
            //moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal)/2 + Mathf.Abs(vertical)/2);
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
        else
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
        
    }
    private void HandleRollInput()
    {
        if(b_Input)
        {
            rollInputTimer += Time.deltaTime;
           if(player.playerStatsManager.currentStamina <= 0)
            {
                b_Input = false;
                sprintFlag = false;
            }
           if(moveAmount > 0.5f && player.playerStatsManager.currentStamina > 0)
            {
                sprintFlag = true;
            }
        }
        else
        {
            sprintFlag = false;
            if (rollInputTimer>0 && rollInputTimer <0.5f)
            {
                
                rollFlag = true;
            }
            rollInputTimer = 0;
        }
    }
    private void HandleTapRBInput()
    {
        if(tap_rb_Input)
        {

            tap_rb_Input = false;
            if(player.playerInventoryManager.rightWeapon.tap_RB_Action!=null)
            {
                player.UpdateUsingHand(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                if(player.isArmed)
                {
                    player.playerInventoryManager.rightWeapon.tap_RB_Action.PerformAction(player);
                    player.isInCombat = true;
                }
                else
                {
                    player.isInCombat = true;
                }
            }
            else
            {
                Debug.LogWarning("TapRB Action doesn't exist");
            }
            
        } 
    }
    private void HandleTapLBInput()
    {
        if (tap_lb_Input)
        {
            tap_lb_Input = false;
            
            if (player.isTwoHanding)
            {
                if(player.playerInventoryManager.rightWeapon.tap_LB_Action!=null)
                {
                    player.UpdateUsingHand(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    if (player.isArmed)
                    {
                        player.playerInventoryManager.rightWeapon.tap_LB_Action.PerformAction(player);
                        player.isInCombat = true;
                    }
                    else
                    {
                        player.isInCombat = true;
                    }
                    
                }
                else
                {
                    Debug.LogWarning("TapLB Action doesn't exist");
                }
            }
            /*else
            {
                if (player.playerInventoryManager.leftWeapon.tap_LB_Action!=null)
                {
                    player.UpdateUsingHand(false);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                    player.playerInventoryManager.leftWeapon.tap_LB_Action.PerformAction(player);
                }
                else
                {
                    Debug.LogWarning("TapLB Action doesn't exist");
                }
            }*/
        }
       
    }
    private void HandleTapRTInput()
    {
        if (tap_rt_Input)
        {
            tap_rt_Input = false;
            if (player.playerInventoryManager.rightWeapon.tap_RT_Action!=null)
            {
                player.UpdateUsingHand(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                player.playerInventoryManager.rightWeapon.tap_RT_Action.PerformAction(player);
            }
            else
            {
                Debug.LogWarning("TapRT Action doesn't exist");
            }
            
        }

    }
    private void HandleTapLTInput()
    {
        if (tap_lt_Input)
        {
            tap_lt_Input = false;
            if (player.isTwoHanding)
            {
                if (player.playerInventoryManager.rightWeapon.tap_LT_Action!=null)
                {
                    player.UpdateUsingHand(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.tap_LT_Action.PerformAction(player);
                }
                else
                {
                    Debug.LogWarning("TapLT Action doesn't exist");
                }
                
            }
            /*else
            {
                if (player.playerInventoryManager.leftWeapon.tap_LT_Action!=null)
                {
                    player.UpdateUsingHand(false);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                    player.playerInventoryManager.leftWeapon.tap_LT_Action.PerformAction(player);
                }
                else
                {
                    Debug.LogWarning("TapLT Action doesn't exist");
                }

            }*/
        }
    }

    private void HandleHoldRBInput()
    {
        if (hold_rb_Input)
        {
            if (player.playerInventoryManager.rightWeapon.hold_RB_Action)
            {
                player.UpdateUsingHand(true);
                player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                player.playerInventoryManager.rightWeapon.hold_RB_Action.PerformAction(player);
            }
            else
            {
                Debug.LogWarning("HoldRB Action doesn't exist");
            }
            
        }
    }
    private void HandleHoldRTInput()
    {
        player.animator.SetBool("IsChargingAttack", hold_rt_Input);
        if (hold_rt_Input)
        {
            player.UpdateUsingHand(true);
            player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
            if (player.isTwoHanding)
            {
                if (player.playerInventoryManager.rightWeapon.th_hold_RT_Action != null)
                {
                    player.playerInventoryManager.rightWeapon.th_hold_RT_Action.PerformAction(player);
                }
            }
            else if(player.playerInventoryManager.rightWeapon.hold_RT_Action!=null)
            {
                player.playerInventoryManager.rightWeapon.hold_RT_Action.PerformAction(player);
            }
        }


        
    }
    private void HandleHoldLBInput()
    {
        if (player.isInAir || player.isSprinting || player.isFiringSpell)
        {
            hold_lb_Input = false;
            return;
        }
        if (hold_lb_Input)
        {
            if(player.isTwoHanding)
            {
                if (player.playerInventoryManager.rightWeapon.hold_LB_Action!=null)
                {
                    player.UpdateUsingHand(true);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.rightWeapon;
                    player.playerInventoryManager.rightWeapon.hold_LB_Action.PerformAction(player);
                }
                else
                {
                    Debug.LogWarning("HoldLB Action doesn't exist");
                }
            }
            /*else
            {
                if (player.playerInventoryManager.leftWeapon.hold_LB_Action!=null)
                {
                    player.UpdateUsingHand(false);
                    player.playerInventoryManager.currentItemBeingUsed = player.playerInventoryManager.leftWeapon;
                    player.playerInventoryManager.leftWeapon.hold_LB_Action.PerformAction(player);
                }
                else
                {
                    Debug.LogWarning("HoldLB Action doesn't exist");
                }
                
            }*/
        }
        else if (hold_lb_Input == false)
        {
            if (player.isAiming)
            {
                player.isAiming = false;
                player.uiManager.crosshair.SetActive(false);
                player.cameraHandler.ResetAimCameraRotations();
            }
            if (player.blockingCollider.blockingCollider.enabled)
            {
                player.isBlocking = false;
                player.blockingCollider.DisableBlockingCollider();
            }

        }
    }
    private void HandleQuickSlotInput()
    {
        
        if (d_Pad_Right)
        {
            player.playerInventoryManager.ChangeRightWeapon();
        }
        else if(d_Pad_Left)
        {
            player.playerInventoryManager.ChangeLeftWeapon();
        }
    }
    
    private void HandleInteractableInput()
    {
 
    }
    private void HandleJumpInput()
    {
       
    }
    private void HandleInventoryInput()
    {
       
       /* if(inventory_Input)
        {
            inventoryFlag = !inventoryFlag;
            if(inventoryFlag)
            {
                player.uiManager.OpenSelectWindow();
                player.uiManager.UpdateUI();
                player.uiManager.hudWindow.SetActive(false);
            }
            else
            {
                player.uiManager.ClosSelectWindow();
                player.uiManager.CloseAllInventoryWindows();
                player.uiManager.hudWindow.SetActive(true);
            }
        }
        */
    }
    private void HandleLockOnInput()
    {
        if(lockOn_Input && lockOnFlag== false)
        {
            
            lockOn_Input = false;

            player.cameraHandler.HandleLockOn();
            if (player.cameraHandler.nearestLockOnTarget!=null)
            {
                player.cameraHandler.currentLockOnTarget = player.cameraHandler.nearestLockOnTarget;
                Debug.Log("Nearest lock on target exists");
                lockOnFlag = true;
            }

           
        }
        else if(lockOn_Input && lockOnFlag)
        {
            lockOn_Input = false;
            lockOnFlag = false;
            player.cameraHandler.ClearLockOnTargets();
        }

        if(lockOnFlag && right_Stick_Left_Input)
        {

            right_Stick_Left_Input = false;
            player.cameraHandler.HandleLockOn();
            if(player.cameraHandler.leftLockTarget!=null)
            {
                player.cameraHandler.currentLockOnTarget = player.cameraHandler.leftLockTarget;
            }

        }
        else if (lockOnFlag && right_Stick_Right_Input)
        {
            right_Stick_Right_Input = false;
            player.cameraHandler.HandleLockOn();
            if (player.cameraHandler.rightLockTarget != null)
            {
                player.cameraHandler.currentLockOnTarget = player.cameraHandler.rightLockTarget;
            }

        }

        //player.cameraHandler.SetCameraHeight();
    }

    private void HandleTwoHandedInput()
    {
        twoHandFlag = true;
        player.isTwoHanding = true;
        /*if(y_Input)
        {
            y_Input = false;
            twoHandFlag = !twoHandFlag;

            if(twoHandFlag)
            {
                player.isTwoHanding = true;
                player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                
            }
            else
            {
                player.isTwoHanding = false;
                player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.rightWeapon, false);
                player.playerWeaponSlotManager.LoadWeaponOnSlot(player.playerInventoryManager.leftWeapon, true);
                
            }
        }*/
    }
 
    private void HandleUseConsumableInput()
    {
        if(x_Input)
        {
            x_Input = false;
            player.playerInventoryManager.currentConsumable.AttemptToConsumeItem(player.playerAnimatorHandler, player.playerWeaponSlotManager, player.playerEffectsManager);
        }
    }

}
