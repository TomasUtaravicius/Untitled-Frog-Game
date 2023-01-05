using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToLocationState: AIState
{

    [SerializeField]
    AIState interactState;

    [Header("Walk Location")]
    [SerializeField]
    Transform walkLocation;

    public override AIState Tick(AIManager aiCharacter)
    {
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
}
