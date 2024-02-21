using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{

    public GameObject ToolUI;
    public GameObject CraftUI;
    public GameObject craftingScreenUI;
    public GameObject toolScreenUI;
    public List<string> InventoryItemList = new List<string>();

    Button toolsBTN;
    Button craftAxeBTN;
    Text AxeReq1, AxeReq2;
    bool IsOpen;
    public bool isOpen;

    public BluePrint AxeBLP = new BluePrint("Axe", 2, "Stone", 3, "Stick", 3);

    public static CraftingSystem Instance { get; set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        IsOpen = false;
        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate { OpenToolCategory(); });
        AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("req1").GetComponent<Text>(); 
        AxeReq2 = toolScreenUI.transform.Find("Axe").transform.Find("req2").GetComponent<Text>();

        craftAxeBTN = toolScreenUI.transform.Find("Axe").transform.Find("CraftButton").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate { CraftAnyItem(AxeBLP); });
    }

    void CraftAnyItem(BluePrint bluePrintToCraft)
    {
        InventorySystem.Instance.AddInventory(bluePrintToCraft.ItemName);

        if(bluePrintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(bluePrintToCraft.Req1, bluePrintToCraft.Req1Amount);
        } else if(bluePrintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(bluePrintToCraft.Req1, bluePrintToCraft.Req1Amount);
            InventorySystem.Instance.RemoveItem(bluePrintToCraft.Req2, bluePrintToCraft.Req2Amount);
        }

        StartCoroutine(calculate());
        RefreshNeededItems();
    }

    public IEnumerator calculate()
    {
        yield return new WaitForSeconds(1f);
        InventorySystem.Instance.RecalculeList();
    }

    void OpenToolCategory()
    {
        CraftUI.SetActive(false);
        ToolUI.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        RefreshNeededItems();

        if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            CraftUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            ToolUI.SetActive(false);
            CraftUI.SetActive(false);
            if (!InventorySystem.Instance.isOpen)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            isOpen = false;
        }
    }

    private void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;

        InventoryItemList = InventorySystem.Instance.ItemList;

        foreach(string itemName in  InventoryItemList)
        {
            switch (itemName)
            {
                case "Stone":
                    stone_count += 1;
                    break;
                case "Stick":
                    stick_count += 1;
                    break;
            }
        }

        AxeReq1.text = "2 stones [ " + stone_count + " ]";
        AxeReq2.text = "3 sticks [ " + stick_count + " ]";
        if(stone_count >= 2 && stick_count >= 3)
        {
            craftAxeBTN.gameObject.SetActive(true);
        }
        else
        {
            craftAxeBTN.gameObject.SetActive(false);
        }
    }
}
