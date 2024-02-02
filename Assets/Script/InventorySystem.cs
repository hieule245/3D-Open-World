using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    [SerializeField] public GameObject inventoryScreenUI;
    public List<GameObject> slotList = new List<GameObject>();
    public List<string> ItemList = new List<string>();
    private GameObject ItemtoAdds;
    private GameObject WhichSlotToEquip;

    public bool isFull;
    public bool isOpen;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;
        PopulateSlotList();
        isFull = false;
    }

    private void PopulateSlotList() {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }
 

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {

            Debug.Log("i is pressed");
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            isOpen = false;
        }
    }

    public void AddInventory(string itemName)
    {
        WhichSlotToEquip = FindNextEmptySlot();
        ItemtoAdds = (GameObject)Instantiate(Resources.Load<GameObject>(itemName), WhichSlotToEquip.transform.position, WhichSlotToEquip.transform.rotation);
        ItemtoAdds.transform.SetParent(WhichSlotToEquip.transform);
        ItemList.Add(itemName);
    }

    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool checkFull()
    {
        int count = 0;
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                count += 1;
            }
        }
        if(count == 24)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}