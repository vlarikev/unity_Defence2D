using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private TextMeshProUGUI text;

    private float fill;
    private int healthMax;
    private void Start()
    {
        fill = 1f;
    }
    public void SetMaxHealth(int health)
    {
        healthMax = health;
    }
    public void SetHealth(float health)
    {
        fill = health / healthMax;
        bar.fillAmount = fill;
    }
    public void SetHealthText(float health)
    {
        text.text = health + " / " + healthMax;
    }
}
