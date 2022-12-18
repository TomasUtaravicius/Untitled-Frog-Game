using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventColliderBossFight : MonoBehaviour
{
    WorldEventManager worldEventManager;


    private void Awake()
    {
        worldEventManager = FindObjectOfType<WorldEventManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.LogWarning("PLAYER WALKED THROUGH");
            worldEventManager.ActivateBossFight();
        }
    }
}
