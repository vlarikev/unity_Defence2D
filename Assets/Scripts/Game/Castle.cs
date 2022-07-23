using UnityEngine;
using UnityEngine.SceneManagement;

public class Castle : MonoBehaviour
{
    [SerializeField] private GameObject units;

    [SerializeField] private bool isEnemy;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private int healthMax;
    private float healthCurrent;
    private int startCoins;
    private int startCrystals;

    private bool isDetect = false;

    void Start()
    {
        if (!isEnemy)
            healthMax = PlayerPrefs.GetInt("item1stat");

        healthCurrent = healthMax;
        healthBar.SetMaxHealth(healthMax);

        startCoins = PlayerPrefs.GetInt("coins");
        startCrystals = PlayerPrefs.GetInt("crystals");
    }

    void Update()
    {
        if (healthCurrent <= 0)
            Reward();

        Detector();
        ColliderController();
    }

    public void Damaged(float damage)
    {
        healthCurrent -= damage;
        healthBar.SetHealth(healthCurrent);
    }
    private void Reward()
    {
        if (isEnemy)
        {
            PlayerPrefs.SetInt("waveUnlocked", PlayerPrefs.GetInt("waveUnlocked") + 1);

            PlayerPrefs.SetInt("coins", startCoins + (PlayerPrefs.GetInt("coins") - startCoins) * 2);
            PlayerPrefs.SetInt("crystals", startCrystals + (PlayerPrefs.GetInt("crystals") - startCrystals) * 2);

            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
    private void Detector()
    {
        if (isDetect)
        {
            isDetect = false;
            transform.position = new Vector3(transform.position.x - 0.0001f, transform.position.y, transform.position.z);
        }
        else
        {
            isDetect = true;
            transform.position = new Vector3(transform.position.x + 0.0001f, transform.position.y, transform.position.z);
        }
    }
    private void ColliderController()
    {
        if (units.transform.childCount != 0)
            gameObject.GetComponent<Collider2D>().enabled = false;
        else
            gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
