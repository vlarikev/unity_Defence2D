using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveIdText;
    private int currentWave;

    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    [Header("Potions")]
    [SerializeField] private TextMeshProUGUI mainPotionText;
    [SerializeField] private TextMeshProUGUI mainPotionQuantity;

    [SerializeField] private Button[] potionButtonList;
    [SerializeField] private TextMeshProUGUI[] potionQuantityTextList;
    [SerializeField] private Image[] potionTopImageList;
    [SerializeField] private Image[] potionActiveImageList;

    private int pickedPotion = -1;
    private int pickedPotionQ = 0;
    private string[] potionNameList = new string[11] { "Divine power potion", "Great summoning potion", "Great lightning potion", "Lucky potion ",
                                                        "Influence potion", "Potion of strength", "Potion of swiftness", "Potion of durability",
                                                        "Summoning potion", "Lightning potion", "Freezing potion" };
    private int[] potionQuantityList;


    private void Start()
    {
        potionQuantityList = new int[potionButtonList.Length];
        currentWave = PlayerPrefs.GetInt("waveUnlocked");
        UpdateUI();
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt("wave", currentWave);
        SavePotions();

        SceneManager.LoadScene(1);
    }
    public void SelectButton(int value)
    {
        currentWave += value;
        UpdateUI();
    }
    public void UpdateUI()
    {
        waveIdText.text = "wave " + currentWave;

        if (currentWave == 1)
            prevButton.gameObject.SetActive(false);
        else
            prevButton.gameObject.SetActive(true);

        if (currentWave == PlayerPrefs.GetInt("waveUnlocked"))
            nextButton.gameObject.SetActive(false);
        else
            nextButton.gameObject.SetActive(true);
    }

    public void UpdatePotionsUI()
    {
        mainPotionQuantity.text = "";
        mainPotionText.text = "";
        
        pickedPotion = -1;
        pickedPotionQ = 0;
        PlayerPrefs.SetInt("potionActive", pickedPotion);
        PlayerPrefs.SetInt("potionQuantity", pickedPotionQ);
        foreach (Image e in potionActiveImageList)
        {
            e.gameObject.SetActive(false);
        }

        for (int i = 0; i < potionButtonList.Length; i++)
        {
            potionQuantityList[i] = PlayerPrefs.GetInt("item" + (i + 4) + "value");
        }

        for (int i = 0; i < potionButtonList.Length; i++)
        {
            if (potionQuantityList[i] != 0)
            {
                potionButtonList[i].interactable = true;
                potionTopImageList[i].color = new Color(potionTopImageList[i].color.r, potionTopImageList[i].color.g, potionTopImageList[i].color.b, 1f);
            }
            else
            {
                potionButtonList[i].interactable = false;
                potionTopImageList[i].color = new Color(potionTopImageList[i].color.r, potionTopImageList[i].color.g, potionTopImageList[i].color.b, 0.5f);
            }

            potionQuantityTextList[i].text = potionQuantityList[i].ToString();
        }
    }
    public void BackPotion()
    {
        UpdatePotionsUI();
    }
    public void PickPotion(int value)
    {
        pickedPotion = value;
        pickedPotionQ++;
        mainPotionQuantity.text = pickedPotionQ.ToString();

        for (int i = 0; i < potionActiveImageList.Length; i++)
        {
            if (pickedPotion == i)
                potionActiveImageList[i].gameObject.SetActive(true);
        }

        for (int i = 0; i < potionButtonList.Length; i++)
        {
            potionButtonList[i].interactable = false;
            potionTopImageList[i].color = new Color(potionTopImageList[i].color.r, potionTopImageList[i].color.g, potionTopImageList[i].color.b, 0.5f);

            if (pickedPotion == i)
            {
                potionQuantityList[i]--;
                mainPotionText.text = potionNameList[i];
                potionButtonList[i].interactable = true;
                potionQuantityTextList[i].text = potionQuantityList[i].ToString();

                potionTopImageList[i].color = new Color(potionTopImageList[i].color.r, potionTopImageList[i].color.g, potionTopImageList[i].color.b, 1f);

                if (potionQuantityList[i] == 0 || pickedPotionQ == PlayerPrefs.GetInt("item3stat"))
                {
                    potionButtonList[i].interactable = false;
                    potionTopImageList[i].color = new Color(potionTopImageList[i].color.r, potionTopImageList[i].color.g, potionTopImageList[i].color.b, 0.5f);
                }
            }
        }
    }
    private void SavePotions()
    {
        PlayerPrefs.SetInt("potionActive", pickedPotion);
        PlayerPrefs.SetInt("potionQuantity", pickedPotionQ);
    }
}
