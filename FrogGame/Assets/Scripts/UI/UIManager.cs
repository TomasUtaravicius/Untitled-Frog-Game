using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI interactionPrompt;
    public PlayerManager player;
    public PlayerInventoryManager playerInventoryManager;
    public EquipmentWindowUI equipmentWindowUI;
    private QuickSlotsUI quickSlotsUI;
    [Header("UI Windows")]
    public GameObject hudWindow;
    public GameObject selectWindow;
    public GameObject equipmentScreenWindow;
    public GameObject weaponInventoryWindow;

    [Header("Equipment Window Slot Selected")]
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;

    [Header("Weapon Inventory")]
    public GameObject weaponInventorySlotPrefab;
    public Transform weaponInventorySlotsParent;
    WeaponInventorySlot[] weaponInventorySlots;

    [Header("HUD")]
    public GameObject crosshair;
    private void Awake()
    {
        quickSlotsUI = GetComponentInChildren<QuickSlotsUI>();
    }
    private void Start()
    {
        weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
        //equipmentWindowUI.LoadWeaponsOnEquipmentScreen(playerInventoryManager);
        //quickSlotsUI.UpdateCurrentConsumableIcon(playerInventoryManager.currentConsumable);
       //quickSlotsUI.UpdateCurrentSpellIcon(playerInventoryManager.currentSpell);
    }
    public void FixedUpdate()
    {
        if (player.eligibleForInteraction)
        {
            EnableInteractionPrompt();
        }
        else
        {
            DisableInteractionPrompt();
        }
    }
    public void UpdateUI()
    {
        #region Weapon Inventory Slots

        for (int i = 0; i < weaponInventorySlots.Length; i++)
        {
            /*if(i<playerInventoryManager.weaponsInventory.Count)
            {
                if(weaponInventorySlots.Length <playerInventoryManager.weaponsInventory.Count)
                {
                    Instantiate(weaponInventorySlotPrefab, weaponInventorySlotsParent);
                    weaponInventorySlots = weaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>();
                }

                weaponInventorySlots[i].AddItem(playerInventoryManager.weaponsInventory[i]);
            }
            else
            {
                weaponInventorySlots[i].ClearInventorySlot();
            }*/
        }
       

        #endregion
    }
    public void EnableInteractionPrompt()
    {
        interactionPrompt.gameObject.SetActive(true);
    }
    public void DisableInteractionPrompt()
    {
        interactionPrompt.gameObject.SetActive(false);
    }
    public void OpenSelectWindow()
    {
        selectWindow.SetActive(true);
    }
    public void ClosSelectWindow()
    {
        selectWindow.SetActive(false);
    }
    public void CloseAllInventoryWindows()
    {
        ResetAllSelectedSlots();
        weaponInventoryWindow.SetActive(false);
        equipmentScreenWindow.SetActive(false);
    }
    public void ResetAllSelectedSlots()
    {
        rightHandSlot01Selected = false;
        rightHandSlot02Selected = false;
        leftHandSlot01Selected = false;
        leftHandSlot02Selected = false;
    }
    public void UpdateSubtitleText(string subtitleText)
    {
        dialogueText.text = subtitleText;
    }
    

}
