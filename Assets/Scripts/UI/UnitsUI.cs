using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI unitName;
    [SerializeField] private TextMeshProUGUI unitHP;
    [SerializeField] private TextMeshProUGUI unitHP_up;
    [SerializeField] private TextMeshProUGUI unitDMG;
    [SerializeField] private TextMeshProUGUI unitDMG_up;
    [SerializeField] private TextMeshProUGUI unitAS;
    [SerializeField] private TextMeshProUGUI unitAS_up;

    [SerializeField] private GameObject[] unitsCollection;
    [SerializeField] private GameObject[] pointerCollection;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private TextMeshProUGUI upgradeButtonText;
    [SerializeField] private TextMeshProUGUI unitLvlText;
    [SerializeField] private Image coinImageUp;

    [SerializeField] private Button prevButton;
    [SerializeField] private Button nextButton;

    [SerializeField] private GameObject statDamageText;
    [SerializeField] private GameObject statHealText;

    [SerializeField] private GameObject arrows;

    [Header("Units Price")]
    [SerializeField] private int[] priceList1;
    [SerializeField] private int[] priceList2;
    [SerializeField] private int[] priceList3;
    [SerializeField] private int[] priceList4;
    [SerializeField] private int[] priceList5;
    [SerializeField] private int[] priceList6;
    [SerializeField] private int[] priceList7;

    private int currentUnit;

    private void Start()
    {
        currentUnit = 0;
        prevButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        
        UpdateUI();

        statDamageText.SetActive(true);
        statHealText.SetActive(false);
    }
    private void Update()
    {
        
    }
    public void SelectButton(int value)
    {
        unitsCollection[currentUnit].SetActive(false);
        pointerCollection[currentUnit].SetActive(false);

        currentUnit += value;

        if (currentUnit == 0)
            prevButton.gameObject.SetActive(false);
        else
            prevButton.gameObject.SetActive(true);

        if (currentUnit == 6)
            nextButton.gameObject.SetActive(false);
        else
            nextButton.gameObject.SetActive(true);     

        UpdateUI();

        unitsCollection[currentUnit].SetActive(true);
        pointerCollection[currentUnit].SetActive(true); 

        if (currentUnit == 5)
        {
            statDamageText.SetActive(false);
            statHealText.SetActive(true);
        }
        else
        {
            statDamageText.SetActive(true);
            statHealText.SetActive(false);
        }
    }
    public void UpgradeButton()
    {
        PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") - UpgradeButtonFormula());
        PlayerPrefs.SetInt("unit" + currentUnit + "lvl", PlayerPrefs.GetInt("unit" + currentUnit + "lvl") + 1);

        UpgradeUnitStats();
    }
    public void UpdateUI()
    {
        if (PlayerPrefs.GetInt("coins") < UpgradeButtonFormula() || PlayerPrefs.GetInt("unit" + currentUnit + "lvl") == 8)
        {
            upgradeButton.interactable = false;
            coinImageUp.color = new Color(coinImageUp.color.r, coinImageUp.color.g, coinImageUp.color.b, 0.5f);
            upgradeButtonText.color = new Color(upgradeButtonText.color.r, upgradeButtonText.color.g, upgradeButtonText.color.b, 0.5f);
        }
        else
        {
            upgradeButton.interactable = true;
            coinImageUp.color = new Color(coinImageUp.color.r, coinImageUp.color.g, coinImageUp.color.b, 1f);
            upgradeButtonText.color = new Color(upgradeButtonText.color.r, upgradeButtonText.color.g, upgradeButtonText.color.b, 1f);
        }

        coinImageUp.gameObject.SetActive(true);
        upgradeButtonText.text = UpgradeButtonFormula().ToString();

        CoinImageFixer();
        upgradeButtonText.fontSize = 80;

        if (PlayerPrefs.GetInt("unit" + currentUnit + "lvl") == 8)
        {
            coinImageUp.gameObject.SetActive(false);
            upgradeButtonText.text = "max lvl";
            upgradeButtonText.fontSize = 60;
            upgradeButtonText.rectTransform.anchoredPosition = new Vector2(0, 10);
        }
        unitName.text = unitsCollection[currentUnit].name;

        unitHP.text = PlayerPrefs.GetInt("unit" + currentUnit + "hp").ToString();
        unitDMG.text = PlayerPrefs.GetFloat("unit" + currentUnit + "dmg").ToString();
        unitAS.text = PlayerPrefs.GetFloat("unit" + currentUnit + "as").ToString() + " sec.";

        if (currentUnit == 0)
            UpgradeUnitStatsUp_Visual(0, 2, 0.6f, 0.1f);
        if (currentUnit == 1)
            UpgradeUnitStatsUp_Visual(1, 4, 1f, 0.06f);
        if (currentUnit == 2)
            UpgradeUnitStatsUp_Visual(2, 2, 1f, 0.1f);
        if (currentUnit == 3)
            UpgradeUnitStatsUp_Visual(3, 15, 0.6f, 0.06f);
        if (currentUnit == 4)
            UpgradeUnitStatsUp_Visual(4, 5, 1.5f, 0.1f);
        if (currentUnit == 5)
            UpgradeUnitStatsUp_Visual(5, 2, 0.5f, 0.06f);
        if (currentUnit == 6)
            UpgradeUnitStatsUp_Visual(6, 2, 4, 0.1f);

        unitLvlText.text = PlayerPrefs.GetInt("unit" + currentUnit + "lvl") + 1 + " lvl";
    }
    private void CoinImageFixer()
    {
        if (currentUnit == 0)
            Fixer(4);
        if (currentUnit == 1)
            Fixer(3);
        if (currentUnit == 2)
            Fixer(2);
        if (currentUnit == 3)
            Fixer(2);
        if (currentUnit == 4)
            Fixer(1);
        if (currentUnit == 5)
            Fixer(0);
        if (currentUnit == 6)
            Fixer(0);
    }
    private void Fixer(int value)
    {
        upgradeButtonText.rectTransform.anchoredPosition = new Vector2(40, 10);
        coinImageUp.rectTransform.anchoredPosition = new Vector2(-65, 10);

        if (PlayerPrefs.GetInt("unit" + currentUnit + "lvl") > value)
        {
            coinImageUp.rectTransform.anchoredPosition = new Vector2(-79, 10);
            upgradeButtonText.rectTransform.anchoredPosition = new Vector2(54, 10);
        }
    }
    private int UpgradeButtonFormula()
    {
        int resultCost = 0;

        if (currentUnit == 0)
            resultCost = priceList1[PlayerPrefs.GetInt("unit" + currentUnit + "lvl")];
        if (currentUnit == 1)
            resultCost = priceList2[PlayerPrefs.GetInt("unit" + currentUnit + "lvl")];
        if (currentUnit == 2)
            resultCost = priceList3[PlayerPrefs.GetInt("unit" + currentUnit + "lvl")];
        if (currentUnit == 3)
            resultCost = priceList4[PlayerPrefs.GetInt("unit" + currentUnit + "lvl")];
        if (currentUnit == 4)
            resultCost = priceList5[PlayerPrefs.GetInt("unit" + currentUnit + "lvl")];
        if (currentUnit == 5)
            resultCost = priceList6[PlayerPrefs.GetInt("unit" + currentUnit + "lvl")];
        if (currentUnit == 6)
            resultCost = priceList7[PlayerPrefs.GetInt("unit" + currentUnit + "lvl")];

        return resultCost;
    }
    private void UpgradeUnitStats()
    {
        if (currentUnit == 0)
            UpgradeUnitStats_Constructor(0, 2, 0.6f, 0.1f);
        if (currentUnit == 1)
            UpgradeUnitStats_Constructor(1, 4, 1f, 0.06f);
        if (currentUnit == 2)
            UpgradeUnitStats_Constructor(2, 2, 1f, 0.1f);
        if (currentUnit == 3)
            UpgradeUnitStats_Constructor(3, 15, 0.6f, 0.06f);
        if (currentUnit == 4)
            UpgradeUnitStats_Constructor(4, 5, 1.5f, 0.1f);
        if (currentUnit == 5)
            UpgradeUnitStats_Constructor(5, 2, 0.5f, 0.06f);
        if (currentUnit == 6)
            UpgradeUnitStats_Constructor(6, 2, 4, 0.1f);

        UpdateUI();
    }
    private void UpgradeUnitStats_Constructor(int unit, int hp, float dmg, float attackSpeed)
    {
        PlayerPrefs.SetInt("unit" + unit + "hp", PlayerPrefs.GetInt("unit" + unit + "hp") + hp);
        PlayerPrefs.SetFloat("unit" + unit + "dmg", float.Parse((PlayerPrefs.GetFloat("unit" + unit + "dmg") + dmg).ToString("0.00")));
        PlayerPrefs.SetFloat("unit" + unit + "as", float.Parse((PlayerPrefs.GetFloat("unit" + unit + "as") - attackSpeed).ToString("0.00")));
    }
    private void UpgradeUnitStatsUp_Visual(int unit, int hp, float dmg, float attackSpeed)
    {
        if (PlayerPrefs.GetInt("unit" + unit + "lvl") == 8)
        {
            unitHP_up.gameObject.SetActive(false);
            unitDMG_up.gameObject.SetActive(false);
            unitAS_up.gameObject.SetActive(false);

            arrows.SetActive(false);

            unitHP.rectTransform.anchoredPosition = new Vector2(-35, -125);
            unitHP.fontSize = 75;
            unitHP.color = Color.red;

            unitDMG.rectTransform.anchoredPosition = new Vector2(290, -125);
            unitDMG.fontSize = 75;
            unitDMG.color = Color.red;

            unitAS.rectTransform.anchoredPosition = new Vector2(615, -125);
            unitAS.color = Color.red;
        }
        else
        {
            unitHP_up.gameObject.SetActive(true);
            unitDMG_up.gameObject.SetActive(true);
            unitAS_up.gameObject.SetActive(true);

            arrows.SetActive(true);

            unitHP.rectTransform.anchoredPosition = new Vector2(-35, -80);
            unitHP.fontSize = 55;
            unitHP.color = new Color(1, 98/255f, 0);

            unitDMG.rectTransform.anchoredPosition = new Vector2(290, -80);
            unitDMG.fontSize = 55;
            unitDMG.color = new Color(1, 98 / 255f, 0);

            unitAS.rectTransform.anchoredPosition = new Vector2(615, -80);
            unitAS.color = new Color(1, 98 / 255f, 0);

            unitHP_up.text = (PlayerPrefs.GetInt("unit" + unit + "hp") + hp).ToString();
            unitDMG_up.text = (float.Parse((PlayerPrefs.GetFloat("unit" + unit + "dmg") + dmg).ToString("0.00"))).ToString();
            unitAS_up.text = (float.Parse((PlayerPrefs.GetFloat("unit" + unit + "as") - attackSpeed).ToString("0.00"))).ToString() + " sec.";
        }
    }
}
