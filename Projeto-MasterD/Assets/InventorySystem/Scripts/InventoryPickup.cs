using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPickup : MonoBehaviour
{
    //Inventory Reference
    private InventorySystem inventory;

    //Item Reference
    public GameObject itemObject;

    //UI Reference
    public GameObject UIItem;

    //Message Reference
    public Text UIItemMessage;

    //Messages
    private string standardUIMessage;
    private string inventoryFullMessage;

    public bool canPickUp;

    private void Start()
    {
        canPickUp = false;

        //Set Item Reference
        itemObject = transform.GetChild(1).gameObject;

        //Set Message Reference
        UIItemMessage = UIItem.transform.GetChild(0).GetComponent<Text>();

        //Set Messages
        standardUIMessage = UIItemMessage.text;
        inventoryFullMessage = "Inventory Full";

        ActivateUI(false);
    }

    private void Update()
    {
        if (!canPickUp)
        {
            return;
        }
        else if(canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            if(!inventory.PickUpItem(itemObject, this))
            {
                StartCoroutine(InventoryFull());
            }
        }
    }

    public void ActivateItem()
    {
        //TO BE IMPLEMENTED
    }

    /// <summary>
    /// Deactivates GameObject
    /// </summary>
    public void DeactivateItem()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Activates or Deactivates Item UI
    /// </summary>
    /// <param name="isActive">true - Activates UI, false - Deactivates UI</param>
    private void ActivateUI(bool isActive)
    {
        UIItem.SetActive(isActive);
    }

    /// <summary>
    /// Changes Message for UIMessage
    /// </summary>
    /// <returns></returns>
    private IEnumerator InventoryFull()
    {
        UIItemMessage.text = inventoryFullMessage;
        yield return new WaitForSeconds(1f);
        UIItemMessage.text = standardUIMessage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            canPickUp = true;
            inventory = other.GetComponent<InventorySystem>();

            ActivateUI(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canPickUp = false;
            inventory = null;

            ActivateUI(false);
        }
    }
}
