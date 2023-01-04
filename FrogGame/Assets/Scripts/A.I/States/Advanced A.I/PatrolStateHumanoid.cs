using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolStateHumanoid : State
{
    public PursueStateHumanoid pursueState;
    public LayerMask detectionLayer;
    public LayerMask layersBlockingLineOfSight;
    public bool patrolComplete;
    public bool repeatPatrol;

    //Time until next patrol
    [Header("Patrol Rest Time")]
    public float endOfPatrolRestTime;
    public float endOfPatrolTimer;

    [Header("Patrol Position")]
    public bool hasPatrolDestination;
    public int patrolDestinationIndex;
    public Transform currentPatrolDestination;
    public float distanceFromCurrentPatrolPoint;
    public List<Transform> listOfPatrolDestinations = new List<Transform>();



    private void Awake()
    {
        pursueState = GetComponent<PursueStateHumanoid>();
    }
    public override State Tick(EnemyManager aiCharacter)
    {
        SearchForTarget(aiCharacter);
        if(aiCharacter.isInteracting)
        {
            aiCharacter.animator.SetFloat("Vertical", 0);
            aiCharacter.animator.SetFloat("Horizontal", 0);
            return this;
        }

        if(aiCharacter.currentTarget!=null)
        {
            return pursueState;
        }

        if(patrolComplete && repeatPatrol)
        {
            if(endOfPatrolRestTime> endOfPatrolTimer)
            {
                aiCharacter.animator.SetFloat("Vertical", 0, 0.2f,Time.deltaTime);

                endOfPatrolTimer += Time.deltaTime;
                return this;
            }
            else if(endOfPatrolTimer >= endOfPatrolRestTime)
            {
                patrolDestinationIndex = -1;
                hasPatrolDestination = false;
                currentPatrolDestination = null;
                patrolComplete = false;
                endOfPatrolTimer = 0f;
            }

        }

        else if(patrolComplete && !repeatPatrol)
        {
            aiCharacter.navMeshAgent.enabled = false;
            aiCharacter.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }

        if(hasPatrolDestination)
        {
            if(currentPatrolDestination!= null)
            {
                distanceFromCurrentPatrolPoint = Vector3.Distance(aiCharacter.transform.position, currentPatrolDestination.transform.position);

                if(distanceFromCurrentPatrolPoint > 1)
                {
                    aiCharacter.navMeshAgent.enabled = true;
                    aiCharacter.navMeshAgent.destination = currentPatrolDestination.transform.position;
                    Quaternion targetRotation = Quaternion.Lerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, 0.5f);
                    aiCharacter.transform.rotation = targetRotation;
                    aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime);
                }
                else
                {
                    currentPatrolDestination = null;
                    hasPatrolDestination = false;
                  
                }
            }
   
        }

        if(!hasPatrolDestination)
        {
            patrolDestinationIndex += 1;

            if(patrolDestinationIndex>listOfPatrolDestinations.Count-1)
            {
                patrolComplete = true;
                return this;
            }

            currentPatrolDestination = listOfPatrolDestinations[patrolDestinationIndex];
            hasPatrolDestination = true;
            
        }
        return this;
    }

    private void SearchForTarget(EnemyManager aiCharacter)
    {
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
                        return;
                    }
                    if (targetCharacter.isDead)
                        return;

                    Debug.LogWarning("Looking for enemy, my name is: " + aiCharacter.name);
                    Vector3 targetDirection = targetCharacter.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    //if potential target is found it has to be standing infront of the AI's field of view
                    if (viewableAngle > aiCharacter.minimumDetectionAngle && viewableAngle < aiCharacter.maximimumDetectionAngle)
                    {
                        if (Physics.Linecast(aiCharacter.lockOnTransform.position, targetCharacter.lockOnTransform.position, layersBlockingLineOfSight))
                        {
                            return;
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
            return;
        }
        else
        {
            return;
        }
    }
}
