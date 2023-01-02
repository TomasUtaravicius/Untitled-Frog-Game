using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorHandler : CharacterAnimatorHandler
{
    PlayerManager player;
    int vertical;
    int horizontal;
    
    protected override void Awake()
    {
        base.Awake();
        vertical = Animator.StringToHash("Vertical");
        horizontal = Animator.StringToHash("Horizontal");
        player = GetComponent<PlayerManager>();
    }
    public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
    {
        #region Vertical
        float v = 0;
        
        if(verticalMovement>0 && verticalMovement < 0.55f)
        {
            v = 0.5f;
        }
        else if( verticalMovement > 0.55f)
        {
            v = 1f;
        }
        else if( verticalMovement< 0 && verticalMovement > -0.55f)
        {
            v = -0.5f;
        }
        else if (verticalMovement <-0.55f)
        {
            v = -1f;
        }
        else
        {
            v = 0f;
        }
        #endregion

        #region Horizontal
        float h = 0;

        if (horizontalMovement > 0 && horizontalMovement < 0.55f)
        {
            h = 0.5f;
        }
        else if (horizontalMovement > 0.55f)
        {
            h = 1f;
        }
        else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
        {
            h = -0.5f;
        }
        else if (horizontalMovement < -0.55f)
        {
            h = -1f;
        }
        else
        {
            h = 0f;
        }
        #endregion

        if(isSprinting)
        {
            v = 2;
            h = horizontalMovement;
        }
        player.animator.SetFloat(vertical, v, 0.1f, Time.deltaTime);
        player.animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
    }

    
    private void OnAnimatorMove()
    {
        if(character.isInteracting == false)
        {
            return;
        }

        float delta = Time.deltaTime;
        Vector3 deltaPosition = player.animator.deltaPosition;
        Vector3 velocity = deltaPosition / delta;
        player.playerLocomotionManager.AddVelocity(velocity);
    }
}
