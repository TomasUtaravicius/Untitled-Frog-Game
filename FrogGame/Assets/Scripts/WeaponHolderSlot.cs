using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolderSlot : MonoBehaviour
{
    public Transform parentOverride;
    public WeaponItem currentWeapon;
    public ToolItem currentTool;
    public bool isLeftHandSlot;
    public bool isRightHandSlot;
    public bool isBackSlot;
    public bool isSideSlot;
    public GameObject currentItemModel;

    public void UnloadItem()
    {
        if(currentItemModel != null)
        {
            currentItemModel.SetActive(false);
        }
    }
    public void UnloadItemAndDestroy()
    {
        if(currentItemModel!=null)
        {
            Destroy(currentItemModel);
        }
    }
    public void LoadWeaponModel(WeaponItem weaponItem)
    {
        UnloadItemAndDestroy();
        if(weaponItem==null)
        {
            UnloadItem();
            return;
        }

        GameObject model = Instantiate(weaponItem.modelPrefab) as GameObject;
        if(model!=null)
        {
            if(parentOverride !=null)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }
        currentItemModel = model;
    }
    public void LoadToolModel(ToolItem tool)
    {
        UnloadItemAndDestroy();
        if (tool == null)
        {
            UnloadItem();
            return;
        }

        GameObject model = Instantiate(tool.modelPrefab) as GameObject;
        if (model != null)
        {
            if (parentOverride != null)
            {
                model.transform.parent = parentOverride;
            }
            else
            {
                model.transform.parent = transform;
            }
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            model.transform.localScale = Vector3.one;
        }
        currentItemModel = model;
    }
}
