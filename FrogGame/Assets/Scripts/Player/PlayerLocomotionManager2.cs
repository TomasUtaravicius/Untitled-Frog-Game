using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager2 : MonoBehaviour
{
    PlayerManager player;
    Transform cameraObject;
    public Vector3 moveDirection;

    [Header("Ground & Air Detection Stats")]
    [SerializeField]
    float groundDetectionRayStartPoint = 0.5f;
    [SerializeField]
    float minimumDistanceNeededToBeginFall = 1f;
    [SerializeField]
    float groundDirectionRayDistance = 0.2f;
    LayerMask ignoreForGroundCheck;
    public float inAirTimer;

    public Rigidbody rigidbody;
    public GameObject normalCamera;

    [Header("Movement Stats")]
    [SerializeField]
    float movementSpeed = 5f;
    [SerializeField]
    float walkingSpeed = 3f;
    [SerializeField]
    float rotationSpeed = 10f;
    [SerializeField]
    float sprintSpeed = 8f;
    [SerializeField]
    float fallingSpeed = 45f;

    [Header("Stamina Costs")]
    [SerializeField]
    int rollStaminaCost = 15;
    [SerializeField]
    int backStepStaminaCost = 12;
    [SerializeField]
    int sprintStaminaCost = 1;


    public Collider characterCollider;
    public Collider characterCollisionBlocker;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        player = GetComponent<PlayerManager>();
        cameraObject = Camera.main.transform;
        player.isGrounded = true;

        ignoreForGroundCheck = ~(1 << 9 | 1 << 11);

        Physics.IgnoreCollision(characterCollider, characterCollisionBlocker, true);
    }


    Vector3 normalVector;
    Vector3 targetPosition;

    public void HandleMovement()
    {
        if(player.inputHandler.rollFlag)
        {
            return;
        }
        if(player.isInteracting)
        {
            return;
        }
        moveDirection = cameraObject.forward * player.inputHandler.vertical;
        moveDirection += cameraObject.right * player.inputHandler.horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0f;

        float speed = movementSpeed;

        if(player.inputHandler.sprintFlag && player.inputHandler.moveAmount>0.5f)
        {
            speed = sprintSpeed;
            player.isSprinting = true;
            moveDirection *= speed;
            player.playerStatsManager.DrainStamina(sprintStaminaCost);
        }
        else
        {
            if(player.inputHandler.moveAmount<=0.5f)
            {
                moveDirection *= walkingSpeed;
                player.isSprinting = false;
            }
            else
            {
                moveDirection *= speed;
                player.isSprinting = false;
            }
            

        }
       

        Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
        rigidbody.velocity = projectedVelocity;

        if(player.inputHandler.lockOnFlag && player.inputHandler.sprintFlag == false)
        {
            player.playerAnimatorHandler.UpdateAnimatorValues(player.inputHandler.vertical, player.inputHandler.horizontal, player.isSprinting);
        }
        else
        {
            player.playerAnimatorHandler.UpdateAnimatorValues(player.inputHandler.moveAmount, 0, player.isSprinting);
        }
      
    }
    public void HandleRotation()
    {

        if (player.canRotate)
        {
            if(player.isAiming)
            {
                Quaternion targetRotation = Quaternion.Euler(0, player.cameraHandler.cameraTransform.eulerAngles.y, 0);
                Quaternion playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                transform.rotation = playerRotation;
            }
            else
            {
                if (player.inputHandler.lockOnFlag)
                {
                    if (player.inputHandler.sprintFlag || player.inputHandler.rollFlag)
                    {
                        Vector3 targetDirection = Vector3.zero;
                        targetDirection = player.cameraHandler.cameraTransform.forward * player.inputHandler.vertical;
                        targetDirection += player.cameraHandler.cameraTransform.right * player.inputHandler.horizontal;
                        targetDirection.Normalize();
                        targetDirection.y = 0;
                        if (targetDirection == Vector3.zero)
                        {
                            targetDirection = transform.forward;
                        }

                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        Vector3 rotationDirection = moveDirection;
                        rotationDirection = player.cameraHandler.currentLockOnTarget.transform.position - transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();

                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                        transform.rotation = targetRotation;
                    }

                }
                else
                {
                    Vector3 targetDirection = Vector3.zero;
                    float moveOverride = player.inputHandler.moveAmount;

                    targetDirection = cameraObject.forward * player.inputHandler.vertical;
                    targetDirection += cameraObject.right * player.inputHandler.horizontal;

                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = player.transform.forward;
                    }

                    float rs = rotationSpeed;

                    Quaternion tr = Quaternion.LookRotation(targetDirection);
                    Quaternion targetRotation = Quaternion.Slerp(player.transform.rotation, tr, rs * Time.deltaTime);

                    player.transform.rotation = targetRotation;
                }
            }
           

        }

    }

    public void HandleRollingAndSprinting()
    {
        if(player.animator.GetBool("IsInteracting"))
        {
            return;
        }
        if(player.playerStatsManager.currentStamina <= 0)
        {
            return;
        }
        if(player.inputHandler.rollFlag)
        {
            player.inputHandler.rollFlag = false;
            moveDirection = cameraObject.forward * player.inputHandler.vertical;
            moveDirection += cameraObject.right * player.inputHandler.horizontal;

            if(player.inputHandler.moveAmount>0)
            {
                player.playerAnimatorHandler.PlayTargetAnimation("Rolling", true);
                player.playerAnimatorHandler.EraseHandIKForWeapon();
                moveDirection.y = 0;

                Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                player.transform.rotation = rollRotation;
                player.playerStatsManager.DrainStamina(rollStaminaCost);
            }
            else
            {
                player.playerAnimatorHandler.PlayTargetAnimation("Backstep", true);
                player.playerAnimatorHandler.EraseHandIKForWeapon();
                player.playerStatsManager.DrainStamina(backStepStaminaCost);
            }
        }
    }
    public void HandleFalling(Vector3 moveDirection)
    {
        player.isGrounded = false;
        RaycastHit hit;
        Vector3 origin = player.transform.position;
        origin.y += groundDetectionRayStartPoint;

        if (Physics.Raycast(origin, player.transform.forward, out hit, 0.4f))
        {
            moveDirection = Vector3.zero;
        }

        if (player.isInAir)
        {
            rigidbody.AddForce(-Vector3.up * fallingSpeed);
            rigidbody.AddForce(moveDirection * fallingSpeed / 10f);
        }

        Vector3 dir = moveDirection;
        dir.Normalize();
        origin = origin + dir * groundDirectionRayDistance;

        targetPosition = player.transform.position;

        Debug.DrawRay(origin, -Vector3.up * minimumDistanceNeededToBeginFall, Color.red, 0.1f, false);
        if (Physics.Raycast(origin, -Vector3.up, out hit, minimumDistanceNeededToBeginFall, ignoreForGroundCheck))
        {
            normalVector = hit.normal;
            Vector3 tp = hit.point;
            player.isGrounded = true;
            targetPosition.y = tp.y;

            if (player.isInAir)
            {
                if (inAirTimer > 0.5f)
                {
                    Debug.Log("You were in the air for " + inAirTimer);
                    player.playerAnimatorHandler.PlayTargetAnimation("Land", true);
                    inAirTimer = 0;
                }
                else
                {
                    player.playerAnimatorHandler.PlayTargetAnimation("Empty", false);
                    inAirTimer = 0;
                }

                player.isInAir = false;
            }
        }
        else
        {
            if (player.isGrounded)
            {
                player.isGrounded = false;
            }

            if (player.isInAir == false)
            {
                if (player.isInteracting == false)
                {
                    player.playerAnimatorHandler.PlayTargetAnimation("Falling", true);
                }

                Vector3 vel = rigidbody.velocity;
                vel.Normalize();
                rigidbody.velocity = vel * (movementSpeed / 2);
                player.isInAir = true;
            }
        }

        if (player.isInteracting || player.inputHandler.moveAmount > 0)
        {
            player.transform.position = Vector3.Lerp(player.transform.position, targetPosition, Time.deltaTime / 0.1f);
        }
        else
        {
            player.transform.position = targetPosition;
        }

    }
    public void HandleJumping()
    {
        if (player.isInteracting)
            return;

        if (player.playerStatsManager.currentStamina <= 0)
        {
            return;
        }
        if (player.inputHandler.jump_Input)
        {
            player.inputHandler.jump_Input = false;
            if (player.inputHandler.moveAmount>0)
            {
                moveDirection = cameraObject.forward * player.inputHandler.vertical;
                moveDirection += cameraObject.right * player.inputHandler.horizontal;
                player.playerAnimatorHandler.PlayTargetAnimation("Running Jump", true);
                player.playerAnimatorHandler.EraseHandIKForWeapon();
                moveDirection.y = 0;
                Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                player.transform.rotation = jumpRotation;
            }
            else
            {
                player.playerAnimatorHandler.PlayTargetAnimation("Jump", true);
                player.playerAnimatorHandler.EraseHandIKForWeapon();
                moveDirection.y = 0;  
            }
        }
    }
}
