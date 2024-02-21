using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }

    [SerializeField] public GameObject statusScreenUI;
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

        if (Input.GetKeyDown(KeyCode.E) && !isOpen)
        {
            statusScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.E) && isOpen)
        {
            statusScreenUI.SetActive(false);
            if (!CraftingSystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
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

    public void RemoveItem(string name, int amountItemToRemove)
    {
        int counter = amountItemToRemove;
        for(var i = slotList.Count - 1; i >= 0; i--)
        {
            if (slotList[i].transform.childCount > 0)
            {
                if(slotList[i].transform.GetChild(0).name == name + "Clone" && counter != 0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);
                    counter -= 1;
                }
            }
        }
    }

    public void RecalculeList()
    {
        ItemList.Clear();
        foreach (GameObject slot in slotList) 
        {
            if(slot.transform.childCount == 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");
                ItemList.Add(result);
            }
        }
    }
}