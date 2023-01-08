using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToLocationState: AIState
{
    public SaySomethingAIAction saySomethingAction;
    [SerializeField]
    AIState interactState;

    [Header("Walk Location")]
    [SerializeField]
    Transform walkLocation;

    public HeadLookIKTarget lookTarget;
    public LayerMask detectionLayer;

    public bool interactWithPlayer;
    float interactWithPlayerTimer;

    public override AIState Tick(AIManager aiCharacter)
    {
        LookForPlayer(aiCharacter);
        if (lookTarget!=null)
        {
            
            float distanceWeight = 1-Vector3.Distance(lookTarget.transform.position, aiCharacter.transform.position)/7;
            aiCharacter.characterAnimatorHandler.SetHeadIKToTarget(lookTarget,distanceWeight);
        }
        else
        {
            aiCharacter.characterAnimatorHandler.EraseHeadIK();
        }
       
        if(aiCharacter.interruptedByPlayer)
        {
            InteractWithPlayer(aiCharacter);
        }
        else
        {
            if (interactWithPlayer)
            {
                if(lookTarget!=null && Vector3.Distance(lookTarget.transform.position, aiCharacter.transform.position)<3f)
                InteractWithPlayer(aiCharacter);
            }
        }
        if (walkLocation != null)
        {
            float distance = Vector3.Distance(aiCharacter.transform.position, walkLocation.position);
            

            if (distance > 0.5f)
            {
                aiCharacter.navMeshAgent.enabled = true;
                aiCharacter.navMeshAgent.destination = walkLocation.position;
                Quaternion targetRotation = Quaternion.Lerp(aiCharacter.transform.rotation, aiCharacter.navMeshAgent.transform.rotation, 0.5f);
                aiCharacter.transform.rotation = targetRotation;
                aiCharacter.animator.SetFloat("Vertical", 0.5f, 0.2f, Time.deltaTime);
            }
            else
            {
                aiCharacter.navMeshAgent.enabled = false;
                aiCharacter.animator.SetFloat("Vertical", 0f, 0.2f, Time.deltaTime);
                return interactState;
            }
        }
       
        return this;
    }
    public void LookForPlayer(AIManager aiCharacter)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 7, detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager targetCharacter = colliders[i].transform.GetComponent<CharacterManager>();


            if (targetCharacter.gameObject.name != gameObject.name)
            {
                if (targetCharacter != null)
                {

                    if (targetCharacter.characterStatsManager.type == NPCType.Player)
                    {
                        lookTarget = targetCharacter.eyeLevel;
                        return;
                    }
                }
            }

        }
        lookTarget = null;
    }
    public void InteractWithPlayer(AIManager aiCharacter)
    {
        if(Time.time>interactWithPlayerTimer)
        {
            aiCharacter.interruptedByPlayer = true;
            saySomethingAction.PerformAction(aiCharacter);
            interactWithPlayerTimer = Time.time + 5f;
            StartCoroutine(FinishInteraction(aiCharacter, 4.5f));
        }
    }
    IEnumerator FinishInteraction(AIManager aiCharacter, float timer)
    {
        
        yield return new WaitForSeconds(timer);
        aiCharacter.interruptedByPlayer = false;

    }
}
