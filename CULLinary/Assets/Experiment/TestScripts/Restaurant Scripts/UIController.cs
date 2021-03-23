using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// In charge of controlling when to show Menu UI and other notifications
public class UIController : MonoBehaviour
{
    public CookingStation cookingStation;

    private int totalAmt = 0;
    private int currButtonIdx = 0;

    // For selecting menu items using wasd
    [Header("Recipe Menu Items")] // Progress Bar variables
    public GameObject menuFirstButton;
    public GameObject menuSecondButton;
    public GameObject menuThirdButton;
    public GameObject menuFourthButton;
    public GameObject menuCloseButton;

    [Header("UI Elements")]
    public GameObject moneyText;
    public GameObject InventoryPanel;
    public GameObject MenuPanel;
    public GameObject CounterNotifPanel;
    public GameObject closeNotifButton;

    private PlayerManager playerManager;

    public string upKey;
    public string downKey;

    private void Start()
    {
        playerManager = PlayerManager.instance;
        if (playerManager != null)
        {
            totalAmt = playerManager.GetMoney();
            moneyText.GetComponent<Text>().text = "Amount earned: $" + totalAmt.ToString();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(upKey))
        {
            currButtonIdx--;
            if (currButtonIdx == 0)
                currButtonIdx = 5; //loop back to the last option
            FindNextSelectedKey();
        }
        if (Input.GetKeyDown(downKey))
        {
            currButtonIdx++;
            if (currButtonIdx == 6)
                currButtonIdx = 1; //loop back to the first option
            FindNextSelectedKey();          
        }
    }

    void FindNextSelectedKey()
    {     
        GameObject selectedButton = null;

        switch (currButtonIdx)
        {
            case 1:
                selectedButton = menuFirstButton;
                break;
            case 2:
                selectedButton = menuSecondButton;
                break;
            case 3:
                selectedButton = menuThirdButton;
                break;
            case 4:
                selectedButton = menuFourthButton;
                break;
            case 5:
                selectedButton = menuCloseButton;
                break;
        }
        EventSystem.current.SetSelectedGameObject(null); // clear selected object
        EventSystem.current.SetSelectedGameObject(selectedButton); //set a new selected object

        //Debug.Log("updated event system??");
    }

    // For Inventory Panel and Menu Panel (Called by CookingStation)
    public void ShowCookingPanel()
    {
        InventoryPanel.SetActive(true);
        MenuPanel.SetActive(true);

        currButtonIdx = 1;
        FindNextSelectedKey();
    }

    public void CloseCookingPanel()
    {
        InventoryPanel.SetActive(false);
        MenuPanel.SetActive(false);
    }

    // NOTIF: "Not enough counter space"
    public void ShowCounterNotifPanel()
    {
        InventoryPanel.SetActive(false);
        MenuPanel.SetActive(false);
        CounterNotifPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null); // clear selected object
        EventSystem.current.SetSelectedGameObject(closeNotifButton); //set a new selected object
    }

    public void CloseCounterNotifPanel()
    {
        CounterNotifPanel.SetActive(false);
        cookingStation.EnableMovementOfPlayer(); // Enable player movement so they can serve food when receive notif that counter has no space
    }

    // To update the Amount Earned at top left hand corner
    public void AddWrongDishEarnings()
    {
        totalAmt += 50;
        moneyText.GetComponent<Text>().text = "Amount earned: $" + totalAmt.ToString();
        AddToGameData();
    }

    public void AddCorrectDishEarnings()
    {
        totalAmt += 100;
        moneyText.GetComponent<Text>().text = "Amount earned: $" + totalAmt.ToString();
        AddToGameData();
    }

    private void AddToGameData()
    {
        
        if (playerManager != null)
        {
            playerManager.SetMoney(totalAmt);
        }
    }
}
