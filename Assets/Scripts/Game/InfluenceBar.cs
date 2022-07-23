using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfluenceBar : MonoBehaviour
{
    [SerializeField]
    private Image bar;
    [SerializeField]
    private TextMeshProUGUI text;

    private float fill;
    private int influenceMax;
    private void Start()
    {
        fill = 1f;
    }
    public void SetMaxInfluence(int value)
    {
        influenceMax = value;
    }
    public void SetInfluence(int value)
    {
        fill = (float)value / influenceMax;
        bar.fillAmount = fill;
        text.text = value + " / " + influenceMax;
    }
}
