using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    //Inventory Slots
    public GameObject[] inventory;

    private void Start()
    {
        inventory = new GameObject[3];
    }

    public bool PickUpItem(GameObject item, InventoryPickup pickup)
    {
        for(int slot = 0; slot < inventory.Length; slot++)
        {
            if(inventory[slot] == null)
            {
                inventory[slot] = item;
                pickup.DeactivateItem();
                return true;
            }
        }

        return false;
    }
}
