using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleTextPanel;
    [SerializeField] private TextMeshProUGUI descTextPanel;
    [SerializeField] private TextMeshProUGUI statTextPanel;
    [SerializeField] private TextMeshProUGUI statUpTextPanel;
    [SerializeField] private GameObject statArrow;
    [SerializeField] private Button buyButton;
    [SerializeField] private TextMeshProUGUI buyButtonTextPanel;
    [SerializeField] private Image buyButtonCoinImage;
    [SerializeField] private Image buyButtonCrystalImage;
    [SerializeField] private Image[] imageItemsList;
    private int openedItem;

    [Header("Items Price")]
    [SerializeField] private int[] priceList;
    [SerializeField] private TextMeshProUGUI castlePriceText;
    [SerializeField] private Image castleCoinIcon;
    [SerializeField] private TextMeshProUGUI influencePriceText;
    [SerializeField] private Image influenceCoinIcon;
    [SerializeField] private TextMeshProUGUI potionSlotPriceText;
    [SerializeField] private Image potionSlotCoinIcon;

    private void Start()
    {
        ControlItemLevelUI();
        ControlItemMaxLevelUI();
    }

    private void Update()
    {

    }
    
    public void BuyButton()
    {
        if (openedItem != 2 && openedItem != 4 && openedItem != 5 && openedItem != 6)
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - priceList[openedItem]);
        else
            PlayerPrefs.SetInt("crystals", PlayerPrefs.GetInt("crystals") - priceList[openedItem]);

        if (openedItem != 4 && openedItem != 5 && openedItem != 6)
            PlayerPrefs.SetInt("item" + openedItem + "value", PlayerPrefs.GetInt("item" + openedItem + "value") + 1);
        else
            PlayerPrefs.SetInt("item" + openedItem + "value", PlayerPrefs.GetInt("item" + openedItem + "value") + 12);

        if (openedItem == 1)
        {
            PlayerPrefs.SetInt("item1stat", PlayerPrefs.GetInt("item1stat") * 2);
            priceList[openedItem] += 250;
        }
        if (openedItem == 2)
        {
            PlayerPrefs.SetInt("item2stat", PlayerPrefs.GetInt("item2stat") - 1);
            priceList[openedItem] += 25;
        }
        if (openedItem == 3)
        {
            PlayerPrefs.SetInt("item3stat", PlayerPrefs.GetInt("item3stat") + 1);
            priceList[openedItem] += 750;
        }

        FixBuyButtonImage(openedItem);
        UpdateItemPanelUI();
        ControlItemLevelUI();
        ControlItemMaxLevelUI();
    }
    public void ItemButton(int value)
    {
        openedItem = value;
        buyButton.gameObject.SetActive(true);

        for (int i = 0; i < imageItemsList.Length; i++)
        {
            if (i == value)
                imageItemsList[i].gameObject.SetActive(true);
            else
                imageItemsList[i].gameObject.SetActive(false);
        }

        FixBuyButtonImage(value);
        UpdateItemPanelUI();
        statTextPanel.rectTransform.anchoredPosition = new Vector2(40, -35);
        ControlItemMaxLevelUI();
    }
    private void FixBuyButtonImage(int value)
    {
        if (value != 2 && value != 4 && value != 5 && value != 6)
        {
            buyButtonCoinImage.gameObject.SetActive(true);
            buyButtonCrystalImage.gameObject.SetActive(false);

            buyButtonCoinImage.rectTransform.anchoredPosition = new Vector2(-80, 10);
            buyButtonTextPanel.rectTransform.anchoredPosition = new Vector2(100, 10);

            if (value == 1)
            {
                if (PlayerPrefs.GetInt("item1value") == 3)
                {
                    buyButtonCoinImage.rectTransform.anchoredPosition = new Vector2(-100, 10);
                    buyButtonTextPanel.rectTransform.anchoredPosition = new Vector2(80, 10);
                }
                else
                {
                    buyButtonCoinImage.rectTransform.anchoredPosition = new Vector2(-80, 10);
                    buyButtonTextPanel.rectTransform.anchoredPosition = new Vector2(100, 10);
                }
            }
            if (value == 3)
            {
                if (PlayerPrefs.GetInt("item3value") == 1)
                {
                    buyButtonCoinImage.rectTransform.anchoredPosition = new Vector2(-100, 10);
                    buyButtonTextPanel.rectTransform.anchoredPosition = new Vector2(80, 10);
                }
                else
                {
                    buyButtonCoinImage.rectTransform.anchoredPosition = new Vector2(-80, 10);
                    buyButtonTextPanel.rectTransform.anchoredPosition = new Vector2(100, 10);
                }
            }
        }
        else
        {
            buyButtonCoinImage.gameObject.SetActive(false);
            buyButtonCrystalImage.gameObject.SetActive(true);

            buyButtonCrystalImage.rectTransform.anchoredPosition = new Vector2(-50, 10);
            buyButtonTextPanel.rectTransform.anchoredPosition = new Vector2(130, 10);
        }
    }
    private void UpdateItemPanelUI()
    {
        if (openedItem != 2 && openedItem != 4 && openedItem != 5 && openedItem != 6)
        {
            if (PlayerPrefs.GetInt("coins") < priceList[openedItem])
                buyButton.interactable = false;
            else
                buyButton.interactable = true;
        }
        else
        {
            if (PlayerPrefs.GetInt("crystals") < priceList[openedItem])
                buyButton.interactable = false;
            else
                buyButton.interactable = true;
        }

        if (openedItem == 0)
            ItemPanelUI("TREASURE BOX", "treasure box description", priceList[0].ToString(), "", "", true);

        if (openedItem == 1)
            ItemPanelUI("UPGRADE CASTLE", "\nIncreases your Castle health.",
                priceList[1].ToString(), PlayerPrefs.GetInt("item1stat").ToString(), (PlayerPrefs.GetInt("item1stat") * 2).ToString(), false);

        if (openedItem == 2)
            ItemPanelUI("UPGRADE INFLUENCE", "\nMakes your influence regeneration faster.", priceList[2].ToString(), "", "", true);

        if (openedItem == 3)
            ItemPanelUI("UPGRADE POTION SLOT", "Allow you to take extra potion of the same type in battle.",
                priceList[3].ToString(), PlayerPrefs.GetInt("item3stat").ToString(), (PlayerPrefs.GetInt("item3stat") + 1).ToString(), false);

        if (openedItem == 4)
            ItemPanelUI("DIVINE POWER POTION x12",
                "Ally units on field gain x2 <color=#FF6200>Health</color>, <color=#FF6200>Damage</color> and <color=#FF6200>Attack speed</color> for 20 seconds.\n\n<color=#FFB400>YOU HAVE: </color>"
                + PlayerPrefs.GetInt("item4value"), priceList[4].ToString(), "", "", true);

        if (openedItem == 5)
            ItemPanelUI("GREAT SUMMONING POTION x12", "\nSummon 4 random allies. \n\n\n<color=#FFB400>YOU HAVE: </color>" + PlayerPrefs.GetInt("item5value"), priceList[5].ToString(), "", "", true);

        if (openedItem == 6)
            ItemPanelUI("GREAT LIGHTNING POTION x12", "Lightning strikes all enemies on field for 60% of their <color=#FF6200>Health</color>. \n\n<color=#FFB400>YOU HAVE: </color>" + PlayerPrefs.GetInt("item6value"), priceList[6].ToString(), "", "", true);

        if (openedItem == 7)
            ItemPanelUI("LUCKY POTION", "Increases x2 <color=#FF6200>currency drop quantity</color> for 45 seconds. \n\n\n<color=#FFB400>YOU HAVE: </color>" + PlayerPrefs.GetInt("item7value"), priceList[7].ToString(), "", "", true);

        if (openedItem == 8)
            ItemPanelUI("INFLUENCE POTION", "Makes your <color=#FF6200>influence regeneration</color> x2 faster for 20 seconds. \n\n<color=#FFB400>YOU HAVE: </color>" + PlayerPrefs.GetInt("item8value"), priceList[8].ToString(), "", "", true);

        if (openedItem == 9)
            ItemPanelUI("POTION OF STRENGTH", "Ally units on field gain x2 <color=#FF6200>Damage</color> for 30 seconds.\n\n\n<color=#FFB400>YOU HAVE: </color>"
                + PlayerPrefs.GetInt("item9value"), priceList[9].ToString(), "", "", true);

        if (openedItem == 10)
            ItemPanelUI("POTION OF SWIFTNESS", "Ally units on field gain x2 <color=#FF6200>Attack speed</color> for 30 seconds.\n\n<color=#FFB400>YOU HAVE: </color>"
                + PlayerPrefs.GetInt("item10value"), priceList[10].ToString(), "", "", true);

        if (openedItem == 11)
            ItemPanelUI("POTION OF DURABILITY", "Ally units on field gain x2 <color=#FF6200>Health</color> for 30 seconds.\n\n\n<color=#FFB400>YOU HAVE: </color>"
                + PlayerPrefs.GetInt("item11value"), priceList[11].ToString(), "", "", true);

        if (openedItem == 12)
            ItemPanelUI("SUMMONING POTION", "\nSummon 2-3 random allies. \n\n\n<color=#FFB400>YOU HAVE: </color>" + PlayerPrefs.GetInt("item12value"), priceList[12].ToString(), "", "", true);

        if (openedItem == 13)
            ItemPanelUI("LIGHTNING POTION", "Lightning strikes all enemies on field for 40% of their <color=#FF6200>Health</color>. \n\n<color=#FFB400>YOU HAVE: </color>" + PlayerPrefs.GetInt("item13value"), priceList[13].ToString(), "", "", true);

        if (openedItem == 14)
            ItemPanelUI("FREEZING POTION", "Slows x2 <color=#FF6200>Attack speed</color> and <color=#FF6200>Speed</color> of all enemies on field for 20 seconds. \n\n<color=#FFB400>YOU HAVE: </color>"
                + PlayerPrefs.GetInt("item14value"), priceList[14].ToString(), "", "", true);
    }
    private void ItemPanelUI(string title, string desc, string price, string stat, string statUp, bool isSimple)
    {
        titleTextPanel.text = title;
        descTextPanel.text = desc;
        buyButtonTextPanel.text = price;

        if (isSimple)
        {
            statTextPanel.gameObject.SetActive(false);
            statUpTextPanel.gameObject.SetActive(false);
            statArrow.gameObject.SetActive(false);
        }
        else
        {
            statTextPanel.gameObject.SetActive(true);
            statUpTextPanel.gameObject.SetActive(true);
            statArrow.gameObject.SetActive(true);

            statTextPanel.text = stat;
            statUpTextPanel.text = statUp;
        }
    }
    private void ControlItemMaxLevelUI()
    {
        if (PlayerPrefs.GetInt("item1value") == 4 && openedItem == 1)
        {
            statTextPanel.rectTransform.anchoredPosition = new Vector2(200, -35);
            buyButton.gameObject.SetActive(false);

            statUpTextPanel.gameObject.SetActive(false);
            statArrow.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("item2value") == 2 && openedItem == 2)
        {
            statTextPanel.rectTransform.anchoredPosition = new Vector2(200, -35);
            buyButton.gameObject.SetActive(false);

            statUpTextPanel.gameObject.SetActive(false);
            statArrow.gameObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("item3value") == 2 && openedItem == 3)
        {
            statTextPanel.rectTransform.anchoredPosition = new Vector2(200, -35);
            buyButton.gameObject.SetActive(false);

            statUpTextPanel.gameObject.SetActive(false);
            statArrow.gameObject.SetActive(false);
        }
    }
    private void ControlItemLevelUI()
    {
        priceList[1] = (1 + PlayerPrefs.GetInt("item1value")) * 250;
        priceList[2] = (1 + PlayerPrefs.GetInt("item2value")) * 25;
        priceList[3] = (1 + PlayerPrefs.GetInt("item3value")) * 750;

        if (PlayerPrefs.GetInt("item1value") == 3)
        {
            castleCoinIcon.rectTransform.anchoredPosition = new Vector2(-70, -134);
            castlePriceText.rectTransform.anchoredPosition = new Vector2(65, -135);
        }
        if (PlayerPrefs.GetInt("item3value") == 1)
        {
            potionSlotCoinIcon.rectTransform.anchoredPosition = new Vector2(-70, -134);
            potionSlotPriceText.rectTransform.anchoredPosition = new Vector2(65, -135);
        }

        if (PlayerPrefs.GetInt("item1value") == 4)
        {
            castlePriceText.text = "max lvl";
            castlePriceText.fontSize = 45;
            castlePriceText.color = Color.red;
            castlePriceText.rectTransform.anchoredPosition = new Vector2(0, -135);
            castleCoinIcon.gameObject.SetActive(false);
        }
        else
            castlePriceText.text = priceList[1].ToString();

        if (PlayerPrefs.GetInt("item2value") == 2)
        {
            influencePriceText.text = "max lvl";
            influencePriceText.fontSize = 45;
            influencePriceText.color = Color.red;
            influencePriceText.rectTransform.anchoredPosition = new Vector2(0, -135);
            influenceCoinIcon.gameObject.SetActive(false);
        }
        else
            influencePriceText.text = priceList[2].ToString();

        if (PlayerPrefs.GetInt("item3value") == 2)
        {
            potionSlotPriceText.text = "max lvl";
            potionSlotPriceText.fontSize = 45;
            potionSlotPriceText.color = Color.red;
            potionSlotPriceText.rectTransform.anchoredPosition = new Vector2(0, -135);
            potionSlotCoinIcon.gameObject.SetActive(false);
        }
        else
            potionSlotPriceText.text = priceList[3].ToString();
    }
}
