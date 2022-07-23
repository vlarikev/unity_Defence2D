using TMPro;
using UnityEngine;

public class MainUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI crystalsText;

    private void Start()
    {
        Application.targetFrameRate = 60;
        //PlayerPrefs.DeleteAll();

        #region PlayerPrefs

        // CURRENCY.

        if (PlayerPrefs.HasKey("coins") == false)
            PlayerPrefs.SetInt("coins", 0);
        if (PlayerPrefs.HasKey("crystals") == false)
            PlayerPrefs.SetInt("crystals", 0);

        // WAVES.

        if (PlayerPrefs.HasKey("wave") == false)
            PlayerPrefs.SetInt("wave", 1);
        if (PlayerPrefs.HasKey("waveUnlocked") == false)
            PlayerPrefs.SetInt("waveUnlocked", 1);

        // STORE.
        // Active Potion.
        if (PlayerPrefs.HasKey("potionActive") == false)
            PlayerPrefs.SetInt("potionActive", -1);
        if (PlayerPrefs.HasKey("potionQuantity") == false)
            PlayerPrefs.SetInt("potionQuantity", 0);

        // Treasure box.
        if (PlayerPrefs.HasKey("item0value") == false)
            PlayerPrefs.SetInt("item0value", 0);

        // Castle.
        if (PlayerPrefs.HasKey("item1value") == false)
            PlayerPrefs.SetInt("item1value", 0);
        if (PlayerPrefs.HasKey("item1stat") == false)
            PlayerPrefs.SetInt("item1stat", 200);

        // Influence. (stat/10 f)
        if (PlayerPrefs.HasKey("item2value") == false)
            PlayerPrefs.SetInt("item2value", 0);
        if (PlayerPrefs.HasKey("item2stat") == false)
            PlayerPrefs.SetInt("item2stat", 9);

        // Potion slot.
        if (PlayerPrefs.HasKey("item3value") == false)
            PlayerPrefs.SetInt("item3value", 0);
        if (PlayerPrefs.HasKey("item3stat") == false)
            PlayerPrefs.SetInt("item3stat", 2);

        // Divine power potion.
        if (PlayerPrefs.HasKey("item4value") == false)
            PlayerPrefs.SetInt("item4value", 0);

        // Great summoning potion.
        if (PlayerPrefs.HasKey("item5value") == false)
            PlayerPrefs.SetInt("item5value", 0);

        // Great lightning potion.
        if (PlayerPrefs.HasKey("item6value") == false)
            PlayerPrefs.SetInt("item6value", 0);

        // Lucky potion.
        if (PlayerPrefs.HasKey("item7value") == false)
            PlayerPrefs.SetInt("item7value", 0);

        // Influence potion.
        if (PlayerPrefs.HasKey("item8value") == false)
            PlayerPrefs.SetInt("item8value", 0);

        // Potion of strength.
        if (PlayerPrefs.HasKey("item9value") == false)
            PlayerPrefs.SetInt("item9value", 0);

        // Potion of swiftness.
        if (PlayerPrefs.HasKey("item10value") == false)
            PlayerPrefs.SetInt("item10value", 0);

        // Potion of durability.
        if (PlayerPrefs.HasKey("item11value") == false)
            PlayerPrefs.SetInt("item11value", 0);

        // Summoning potion.
        if (PlayerPrefs.HasKey("item12value") == false)
            PlayerPrefs.SetInt("item12value", 0);

        // Lightning potion.
        if (PlayerPrefs.HasKey("item13value") == false)
            PlayerPrefs.SetInt("item13value", 0);

        // Freezing potion.
        if (PlayerPrefs.HasKey("item14value") == false)
            PlayerPrefs.SetInt("item14value", 0);

        // UNIT STATS.

        // Farmer
        if (PlayerPrefs.HasKey("unit0lvl") == false)
            PlayerPrefs.SetInt("unit0lvl", 0);
        if (PlayerPrefs.HasKey("unit0hp") == false)
            PlayerPrefs.SetInt("unit0hp", 20);
        if (PlayerPrefs.HasKey("unit0dmg") == false)
            PlayerPrefs.SetFloat("unit0dmg", 5);
        if (PlayerPrefs.HasKey("unit0as") == false)
            PlayerPrefs.SetFloat("unit0as", 1.9f);

        // Guard
        if (PlayerPrefs.HasKey("unit1lvl") == false)
            PlayerPrefs.SetInt("unit1lvl", 0);
        if (PlayerPrefs.HasKey("unit1hp") == false)
            PlayerPrefs.SetInt("unit1hp", 40);
        if (PlayerPrefs.HasKey("unit1dmg") == false)
            PlayerPrefs.SetFloat("unit1dmg", 7);
        if (PlayerPrefs.HasKey("unit1as") == false)
            PlayerPrefs.SetFloat("unit1as", 1.8f);

        // Archer
        if (PlayerPrefs.HasKey("unit2lvl") == false)
            PlayerPrefs.SetInt("unit2lvl", 0);
        if (PlayerPrefs.HasKey("unit2hp") == false)
            PlayerPrefs.SetInt("unit2hp", 25);
        if (PlayerPrefs.HasKey("unit2dmg") == false)
            PlayerPrefs.SetFloat("unit2dmg", 6);
        if (PlayerPrefs.HasKey("unit2as") == false)
            PlayerPrefs.SetFloat("unit2as", 1.8f);

        // Tank
        if (PlayerPrefs.HasKey("unit3lvl") == false)
            PlayerPrefs.SetInt("unit3lvl", 0);
        if (PlayerPrefs.HasKey("unit3hp") == false)
            PlayerPrefs.SetInt("unit3hp", 100);
        if (PlayerPrefs.HasKey("unit3dmg") == false)
            PlayerPrefs.SetFloat("unit3dmg", 5);
        if (PlayerPrefs.HasKey("unit3as") == false)
            PlayerPrefs.SetFloat("unit3as", 2f);

        // Knight
        if (PlayerPrefs.HasKey("unit4lvl") == false)
            PlayerPrefs.SetInt("unit4lvl", 0);
        if (PlayerPrefs.HasKey("unit4hp") == false)
            PlayerPrefs.SetInt("unit4hp", 80);
        if (PlayerPrefs.HasKey("unit4dmg") == false)
            PlayerPrefs.SetFloat("unit4dmg", 16);
        if (PlayerPrefs.HasKey("unit4as") == false)
            PlayerPrefs.SetFloat("unit4as", 1.6f);

        // Healer
        if (PlayerPrefs.HasKey("unit5lvl") == false)
            PlayerPrefs.SetInt("unit5lvl", 0);
        if (PlayerPrefs.HasKey("unit5hp") == false)
            PlayerPrefs.SetInt("unit5hp", 30);
        if (PlayerPrefs.HasKey("unit5dmg") == false)
            PlayerPrefs.SetFloat("unit5dmg", 4);
        if (PlayerPrefs.HasKey("unit5as") == false)
            PlayerPrefs.SetFloat("unit5as", 1.5f);

        // Wizard
        if (PlayerPrefs.HasKey("unit6lvl") == false)
            PlayerPrefs.SetInt("unit6lvl", 0);
        if (PlayerPrefs.HasKey("unit6hp") == false)
            PlayerPrefs.SetInt("unit6hp", 35);
        if (PlayerPrefs.HasKey("unit6dmg") == false)
            PlayerPrefs.SetFloat("unit6dmg", 35);
        if (PlayerPrefs.HasKey("unit6as") == false)
            PlayerPrefs.SetFloat("unit6as", 1.7f);
        #endregion  

        UpdateCurrency();
    }

    private void Update()
    {
        UpdateCurrency();

        if (Input.GetKeyDown(KeyCode.M))
            PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 1000);
        if (Input.GetKeyDown(KeyCode.C))
            PlayerPrefs.SetInt("crystals", PlayerPrefs.GetInt("crystals") + 10);


        if (Input.GetKeyDown(KeyCode.W))
            PlayerPrefs.SetInt("waveUnlocked", 10);
    }
    private void UpdateCurrency()
    {
        coinsText.text = PlayerPrefs.GetInt("coins").ToString();
        crystalsText.text = PlayerPrefs.GetInt("crystals").ToString();
    }
}
