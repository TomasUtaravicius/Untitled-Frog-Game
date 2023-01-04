using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleStateHumanoid : State
{
    public PursueStateHumanoid pursueState;
    public LayerMask detectionLayer;
    public LayerMask layersBlockingLineOfSight;

    private void Awake()
    {
        pursueState = GetComponent<PursueStateHumanoid>();
    }
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

                    Debug.LogWarning("Looking for enemy, my name is: " + aiCharacter.name);
                    Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    //if potential target is found it has to be standing infront of the AI's field of view
                    if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximimumDetectionAngle)
                    {
                        if(Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersBlockingLineOfSight))
                        {
                            return this;
                        }
                        else
                        {
                            aiCharacter.currentTarget = targetCharacter;
                            return pursueState;
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
