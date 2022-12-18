using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    public PursueState pursueState;
    public LayerMask detectionLayer;
    public LayerMask layerBlockingLineOfSight;
    public override State Tick(EnemyManager aiCharacter)
    {
        //Searches for potential target within the detection radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, aiCharacter.detectionRadius, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();


            if (targetCharacter.gameObject.name != aiCharacter.name)
            {
                if (targetCharacter != null)
                {
                    if (targetCharacter.characterStatsManager.teamIDNumber == aiCharacter.enemyStatsManager.teamIDNumber)
                    {
                        return this;
                    }
                    if (targetCharacter.isDead)
                        return this;

                    Vector3 targetDirection = targetCharacter.transform.position - transform.position;
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

                    }
                }
            }

        }

        if (aiCharacter.currentTarget != null)
        {
            return pursueState;
        }
        else
        {
            return this;
        }

    }
}
