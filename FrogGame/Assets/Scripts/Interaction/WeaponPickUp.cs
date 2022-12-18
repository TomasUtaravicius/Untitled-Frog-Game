using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickUp : Interactable
{
    public WeaponItem weapon;

    public override void Interact(PlayerManager playerManager)
    {
        Debug.Log("You interacted with an object");

        PickUpItem(playerManager);
    }
    private void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventoryManager playerInventory;
        PlayerLocomotionManager playerLocomotion;
        PlayerAnimatorHandler animatorHandler;

        playerInventory = playerManager.GetComponent<PlayerInventoryManager>();
        playerLocomotion = playerManager.GetComponent<PlayerLocomotionManager>();
        animatorHandler = playerManager.GetComponentInChildren<PlayerAnimatorHandler>();

        playerLocomotion.rigidbody.velocity = Vector3.zero;
        animatorHandler.PlayTargetAnimation("Pick Up Item", true);
        playerInventory.weaponsInventory.Add(weapon);
        playerManager.itemInteractableGameObject.GetComponentInChildren<TextMeshProUGUI>().text = weapon.itemName;
        playerManager.itemInteractableGameObject.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
        playerManager.itemInteractableGameObject.SetActive(true);
        Destroy(gameObject);
    }
}
