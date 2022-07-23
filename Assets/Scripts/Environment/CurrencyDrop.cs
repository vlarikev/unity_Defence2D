using TMPro;
using UnityEngine;

public class CurrencyDrop : MonoBehaviour
{
    private GameObject coinText;
    private GameObject crystalText;
    private void Start()
    {
        coinText = GameObject.Find("CoinText");
        crystalText = GameObject.Find("CrystalText");

        UpdateCurrencyBars();
    }
    private void UpdateCurrencyBars()
    {
        coinText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("coins").ToString();
        crystalText.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("crystals").ToString();
    }
}
