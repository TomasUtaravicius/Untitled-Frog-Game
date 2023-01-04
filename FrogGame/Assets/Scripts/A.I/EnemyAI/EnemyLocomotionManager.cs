using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyLocomotionManager : MonoBehaviour
{
    EnemyManager enemy;
    public Collider characterCollider;
    public Collider characterCollisionBlocker;
    private void Start()
    {
        Physics.IgnoreCollision(characterCollider, characterCollisionBlocker, true);
    }
    private void Awake()
    {
        enemy = GetComponent<EnemyManager>();
    }
    
}
