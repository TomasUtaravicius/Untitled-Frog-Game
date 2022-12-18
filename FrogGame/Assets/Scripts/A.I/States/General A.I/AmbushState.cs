using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbushState : State
{
    public bool isSleeping;
    public float detectionRadius = 2;
    public string sleepAnimation;
    public string wakeAnimation;
    public LayerMask detectionLayer;

    public PursueState pursueState;

    public override State Tick(EnemyManager enemy)
    {
        if(isSleeping && enemy.isInteracting == false)
        {
            enemy.enemyAnimatorHandler.PlayTargetAnimation(sleepAnimation, true);
            Debug.LogWarning("Sleeping code");
        }
        #region Handle Target Detection
        Collider[] colliders = Physics.OverlapSphere(enemy.transform.position, detectionRadius, detectionLayer);

        
        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager character = colliders[i].transform.GetComponent<CharacterManager>();

            if(character !=null)
            {
                Vector3 targetDirection = character.transform.position - enemy.transform.position;
                float viewableAngle = Vector3.Angle(targetDirection, enemy.transform.forward);

                if(viewableAngle >enemy.minimumDetectionAngle 
                    && viewableAngle <enemy.maximimumDetectionAngle)
                {
                    enemy.currentTarget = character;
                    isSleeping = false;
                    Debug.LogWarning("Waking up code");
                    enemy.enemyAnimatorHandler.PlayTargetAnimation(wakeAnimation, true);
                }
            }
        }
        #endregion

        #region Handle State Change

        if(enemy.currentTarget!=null)
        {
            return pursueState;
        }
        else
        {
            return this;
        }

        #endregion
    }
}
