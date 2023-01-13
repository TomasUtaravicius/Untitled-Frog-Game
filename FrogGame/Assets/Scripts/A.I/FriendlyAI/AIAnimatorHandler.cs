using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimatorHandler : CharacterAnimatorHandler
{
    AIManager aiCharacter;
    protected override void Awake()
    {
        base.Awake();
        aiCharacter = GetComponent<AIManager>();
    }
    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        aiCharacter.rigidbody.drag = 0;
        Vector3 deltaPosition = character.animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        aiCharacter.rigidbody.velocity = velocity;

        if (character.isRotatingWithRootMotion)
        {
            aiCharacter.transform.rotation *= aiCharacter.animator.deltaRotation;
        }
    }
}
