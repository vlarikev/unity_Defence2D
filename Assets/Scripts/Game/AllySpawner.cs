using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AllySpawner : MonoBehaviour
{
    [SerializeField] private InfluenceBar influenceBar;
    private int influenceMax = 30;
    private int influenceCurrent = 0;
    private float influenceTick = 0.9f;
    private int influenceUpCost = 30;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private GameObject collection;

    [Header("Currency")]
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI crystalsText;

    [Header("Potion")]
    [SerializeField] private GameObject potionButton;
    [SerializeField] private TextMeshProUGUI potionTimerText;
    [SerializeField] private TextMeshProUGUI potionQuantityText;
    [SerializeField] private GameObject[] potionImageList;
    [SerializeField] private Image[] potionEffectImageList;

    [Header("Units")]
    [SerializeField] private GameObject farmer;
    [SerializeField] private Button farmerButton;
    [SerializeField] private Image farmerImage;

    [SerializeField] private GameObject guard;
    [SerializeField] private Button guardButton;
    [SerializeField] private Image guardrImage;

    [SerializeField] private GameObject archer;
    [SerializeField] private Button archerButton;
    [SerializeField] private Image archerImage;

    [SerializeField] private GameObject tank;
    [SerializeField] private Button tankButton;
    [SerializeField] private Image tankImage;

    [SerializeField] private GameObject knight;
    [SerializeField] private Button knightButton;
    [SerializeField] private Image knightImage;

    [SerializeField] private GameObject healer;
    [SerializeField] private Button healerButton;
    [SerializeField] private Image healerImage;

    [SerializeField] private GameObject wizrd;
    [SerializeField] private Button wizardButton;
    [SerializeField] private Image wizardImage;

    private GameObject[] alliesList;

    private bool isOnceUpButton = true;
    private int upgradeButtonCount = 2;
    private bool isPotioned;

    private float[] spawnPosArray = new float[5] { -3.4f, -3.55f, -3.7f, -3.85f, -4f };

    private void Start()
    {
        alliesList = new GameObject[7] { farmer, guard, archer, tank, knight, healer, wizrd };

        influenceTick = PlayerPrefs.GetInt("item2stat");
        influenceTick = influenceTick / 10;

        SetPotions();
        UpdateCurrencyBars();

        influenceBar.SetMaxInfluence(influenceMax);
        influenceBar.SetInfluence(0);

        upgradeButton.interactable = false;

        StartCoroutine(InfluenceRegen());
    }
    private void Update()
    {
        if (influenceCurrent >= influenceUpCost && isOnceUpButton && upgradeButtonCount != 0)
        {
            isOnceUpButton = false;
            upgradeButton.interactable = true;
        }

        if (upgradeButtonCount == 0)
            upgradeButton.gameObject.SetActive(false);

        ButtonsInteractions();
    }

    public void SpawnAllyButton(int value)
    {
        if (value == 1)
            SpawnAlly(farmer, 5);
        if (value == 2)
            SpawnAlly(guard, 10);
        if (value == 3)
            SpawnAlly(archer, 15);
        if (value == 4)
            SpawnAlly(tank, 30);
        if (value == 5)
            SpawnAlly(knight, 35);
        if (value == 6)
            SpawnAlly(healer, 45);
        if (value == 7)
            SpawnAlly(wizrd, 50);
    }

    public void UpgradeButton()
    {
        upgradeButtonCount--;
        upgradeButton.interactable = false;

        if (influenceCurrent >= influenceUpCost && influenceUpCost < 100)
        {
            influenceCurrent -= influenceUpCost;
            influenceUpCost *= 2;

            influenceMax = influenceUpCost;
            if (isPotioned)
            {
                if (upgradeButtonCount == 1)
                {
                    influenceTick = PlayerPrefs.GetInt("item2stat");
                    influenceTick = influenceTick / 10;
                    influenceTick = (influenceTick - 0.2f) / 2;
                }
                if (upgradeButtonCount == 0)
                {
                    influenceTick = PlayerPrefs.GetInt("item2stat");
                    influenceTick = influenceTick / 10;
                    influenceTick = (influenceTick - 0.4f) / 2;
                }
            }
            else
                influenceTick -= 0.2f;

            influenceBar.SetMaxInfluence(influenceUpCost);
            influenceBar.SetInfluence(influenceCurrent);
        }

        isOnceUpButton = true;
    }

    public void UpdateCurrencyBars()
    {
        coinsText.text = PlayerPrefs.GetInt("coins").ToString();
        crystalsText.text = PlayerPrefs.GetInt("crystals").ToString();
    }
    public void UsePotion()
    {
        PlayerPrefs.SetInt("potionQuantity", PlayerPrefs.GetInt("potionQuantity") - 1);
        potionQuantityText.text = PlayerPrefs.GetInt("potionQuantity").ToString();

        string temp = "item" + (PlayerPrefs.GetInt("potionActive") + 4) + "value";
        PlayerPrefs.SetInt(temp, PlayerPrefs.GetInt(temp) - 1);

        PotionEffect();

        if (PlayerPrefs.GetInt("potionQuantity") == 0)
        {
            potionButton.GetComponent<Button>().interactable = false;
            potionImageList[PlayerPrefs.GetInt("potionActive")].GetComponent<Image>().color = new Color(potionImageList[PlayerPrefs.GetInt("potionActive")].GetComponent<Image>().color.r,
                                                                                                    potionImageList[PlayerPrefs.GetInt("potionActive")].GetComponent<Image>().color.g,
                                                                                                    potionImageList[PlayerPrefs.GetInt("potionActive")].GetComponent<Image>().color.b, 0.5f);
        }
        else
        {
            StartCoroutine(PotionCooldown());
        }
    }

    public delegate void PotionAction (int value);
    public event PotionAction potionAllyEvent;
    public event PotionAction potionEnemyEvent;
    private void PotionEffect()
    {
        if (PlayerPrefs.GetInt("potionActive") == 0)
        {
            potionAllyEvent?.Invoke(0);
            StartCoroutine(EffectImageDuration(0, 20));
        }

        if (PlayerPrefs.GetInt("potionActive") == 1)
        {
            StartCoroutine(SummoningPotion(4));
            StartCoroutine(EffectImageDuration(1, 0));
        }

        if (PlayerPrefs.GetInt("potionActive") == 2)
        {
            potionEnemyEvent?.Invoke(2);
            StartCoroutine(EffectImageDuration(2, 0));
        }

        if (PlayerPrefs.GetInt("potionActive") == 3)
        {
            potionEnemyEvent?.Invoke(3);
            StartCoroutine(EffectImageDuration(3, 45));
        }

        if (PlayerPrefs.GetInt("potionActive") == 4)
        {
            StartCoroutine(InfluencePotion());
            StartCoroutine(EffectImageDuration(4, 20));
        }

        if (PlayerPrefs.GetInt("potionActive") == 5)
        {
            potionAllyEvent?.Invoke(5);
            StartCoroutine(EffectImageDuration(5, 30));
        }

        if (PlayerPrefs.GetInt("potionActive") == 6)
        {
            potionAllyEvent?.Invoke(6);
            StartCoroutine(EffectImageDuration(6, 30));
        }

        if (PlayerPrefs.GetInt("potionActive") == 7)
        {
            potionAllyEvent?.Invoke(7);
            StartCoroutine(EffectImageDuration(7, 30));
        }

        if (PlayerPrefs.GetInt("potionActive") == 8)
        {
            StartCoroutine(SummoningPotion(Random.Range(2, 4)));
            StartCoroutine(EffectImageDuration(8, 0));
        }

        if (PlayerPrefs.GetInt("potionActive") == 9)
        {
            potionEnemyEvent?.Invoke(9);
            StartCoroutine(EffectImageDuration(9, 0));
        }

        if (PlayerPrefs.GetInt("potionActive") == 10)
        {
            potionEnemyEvent?.Invoke(10);
            StartCoroutine(EffectImageDuration(10, 20));
        }
    }
    private IEnumerator EffectImageDuration(int value, int duration)
    {
        potionEffectImageList[value].gameObject.SetActive(true);

        StartCoroutine(EffectImageLoop(value));
        yield return new WaitForSeconds(duration);
        StopCoroutine(EffectImageLoop(value));
        
        for (float i = 1; i > 0; i -= 0.1f)
        {
            potionEffectImageList[value].color = new Color(potionEffectImageList[value].color.r, potionEffectImageList[value].color.g, potionEffectImageList[value].color.b, i);
            yield return new WaitForSeconds(0.04f);
        }
        potionEffectImageList[value].gameObject.SetActive(false);
    }
    private IEnumerator EffectImageLoop(int value)
    {
        float temp = 0.002f;
        while (true)
        {
            for (int i = 0; i < 20; i++)
            {
                potionEffectImageList[value].rectTransform.localScale = new Vector2(potionEffectImageList[value].rectTransform.localScale.x + temp, potionEffectImageList[value].rectTransform.localScale.y + temp);
                yield return new WaitForSeconds(0.05f);
            }
            temp = -temp;
        }
    }
    private IEnumerator PotionCooldown()
    {
        potionButton.GetComponent<Button>().interactable = false;
        potionImageList[PlayerPrefs.GetInt("potionActive")].GetComponent<Image>().color = new Color(potionImageList[PlayerPrefs.GetInt("potionActive")].GetComponent<Image>().color.r,
                                                                                                    potionImageList[PlayerPrefs.GetInt("potionActive")].GetComponent<Image>().color.g,
                                                                                                    potionImageList[PlayerPrefs.GetInt("potionActive")].GetComponent<Image>().color.b, 0.5f);

        for (int i = 60; i != 0; i--)
        {
            potionTimerText.text = i.ToString();
            yield return new WaitForSeconds(1f);
        }

        potionTimerText.text = "";
        potionImageList[PlayerPrefs.GetInt("potionActive")].GetComponent<Image>().color = new Color(potionImageList[PlayerPrefs.GetInt("potionActive")].GetComponent<Image>().color.r,
                                                                                                    potionImageList[PlayerPrefs.GetInt("potionActive")].GetComponent<Image>().color.g,
                                                                                                    potionImageList[PlayerPrefs.GetInt("potionActive")].GetComponent<Image>().color.b, 1f);
        potionButton.GetComponent<Button>().interactable = true;
    }
    private IEnumerator SummoningPotion(int value)
    {
        int tempRandom = 0;
        for (int i = 0; i < value; i++)
        {
            GameObject allyObject = Instantiate(alliesList[Random.Range(0, 7)], new Vector3(transform.position.x, spawnPosArray[tempRandom], transform.position.z), Quaternion.identity, collection.transform);
            allyObject.GetComponentInChildren<SortingGroup>().sortingOrder = 10 + tempRandom;
            tempRandom++;
            yield return new WaitForSeconds(0.3f);
        }
    }
    private IEnumerator InfluencePotion()
    {
        isPotioned = true;
        influenceTick /= 2;

        yield return new WaitForSeconds(20);

        isPotioned = false;
        influenceTick *= 2;
    }

    #region ButtonsInteractions
    private void ButtonsInteractions()
    {
        if (influenceCurrent < 5)
        {
            farmerButton.interactable = false;
            farmerImage.color = new Color(farmerImage.color.r, farmerImage.color.g, farmerImage.color.b, 0.5f);
        }
        else
        {
            farmerButton.interactable = true;
            farmerImage.color = new Color(farmerImage.color.r, farmerImage.color.g, farmerImage.color.b, 1f);
        }
        if (influenceCurrent < 10)
        {
            guardButton.interactable = false;
            guardrImage.color = new Color(guardrImage.color.r, guardrImage.color.g, guardrImage.color.b, 0.5f);
        }
        else
        {
            guardButton.interactable = true;
            guardrImage.color = new Color(guardrImage.color.r, guardrImage.color.g, guardrImage.color.b, 1f);
        }
        if (influenceCurrent < 15)
        {
            archerButton.interactable = false;
            archerImage.color = new Color(archerImage.color.r, archerImage.color.g, archerImage.color.b, 0.5f);
        }
        else
        {
            archerButton.interactable = true;
            archerImage.color = new Color(archerImage.color.r, archerImage.color.g, archerImage.color.b, 1f);
        }
        if (influenceCurrent < 30)
        {
            tankButton.interactable = false;
            tankImage.color = new Color(tankImage.color.r, tankImage.color.g, tankImage.color.b, 0.5f);
        }
        else
        {
            tankButton.interactable = true;
            tankImage.color = new Color(tankImage.color.r, tankImage.color.g, tankImage.color.b, 1f);
        }
        if (influenceCurrent < 35)
        {
            knightButton.interactable = false;
            knightImage.color = new Color(knightImage.color.r, knightImage.color.g, knightImage.color.b, 0.5f);
        }
        else
        {
            knightButton.interactable = true;
            knightImage.color = new Color(knightImage.color.r, knightImage.color.g, knightImage.color.b, 1f);
        }
        if (influenceCurrent < 45)
        {
            healerButton.interactable = false;
            healerImage.color = new Color(healerImage.color.r, healerImage.color.g, healerImage.color.b, 0.5f);
        }
        else
        {
            healerButton.interactable = true;
            healerImage.color = new Color(healerImage.color.r, healerImage.color.g, healerImage.color.b, 1f);
        }
        if (influenceCurrent < 50)
        {
            wizardButton.interactable = false;
            wizardImage.color = new Color(wizardImage.color.r, wizardImage.color.g, wizardImage.color.b, 0.5f);
        }
        else
        {
            wizardButton.interactable = true;
            wizardImage.color = new Color(wizardImage.color.r, wizardImage.color.g, wizardImage.color.b, 1f);
        }
    }
    #endregion

    private void SetPotions()
    {
        if (PlayerPrefs.GetInt("potionActive") == -1)
        {
            potionButton.SetActive(false);
        }
        else
        {
            potionButton.SetActive(true);
            potionTimerText.text = "";
            potionQuantityText.text = PlayerPrefs.GetInt("potionQuantity").ToString();
            potionImageList[PlayerPrefs.GetInt("potionActive")].SetActive(true);
        }
    }
    private void SpawnAlly(GameObject ally, int cost)
    {
        if (influenceCurrent >= cost)
        {
            upgradeButton.interactable = false;
            isOnceUpButton = true;

            influenceCurrent -= cost;
            influenceBar.SetInfluence(influenceCurrent);

            int tempRandom = Random.Range(0, 5);
            GameObject allyObject = Instantiate(ally, new Vector3(transform.position.x, spawnPosArray[tempRandom], transform.position.z), Quaternion.identity, collection.transform);
            allyObject.GetComponentInChildren<SortingGroup>().sortingOrder = 10 + tempRandom;
        }
    }
    private IEnumerator InfluenceRegen()
    {
        while (true)
        {
            yield return new WaitForSeconds(influenceTick);

            if (influenceCurrent < influenceMax)
                influenceCurrent++;
            if (influenceCurrent >= influenceMax)
                influenceCurrent = influenceMax;

            influenceBar.SetInfluence(influenceCurrent);
        }
    }
}
