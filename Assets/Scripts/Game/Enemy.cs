using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject healthBarObject;

    [SerializeField] private int healthMax;
    private float healthCurrent;

    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject damageBox;

    [SerializeField] private Animator animator;

    [SerializeField] private GameObject shadow;

    [SerializeField] private GameObject enemyGFX;
    private SpriteRenderer[] enemyGFXcollection;

    [Header("Currency")]
    [SerializeField] private int coinQuantity;
    [SerializeField] private float coinDropChanse;
    [SerializeField] private GameObject coinObject;
    [SerializeField] private float crystalDropChanse;
    [SerializeField] private GameObject crystalObject;
    private int luckyPotionMultiplier = 1;

    private bool isAttack = false;
    private bool isOnceAttack = true;
    private bool isDie = false;
    private bool isOnceDie = true;

    private void Start()
    {
        healthCurrent = healthMax;
        healthBar.SetMaxHealth(healthMax);
        healthBarObject.SetActive(false);

        enemyGFXcollection = enemyGFX.GetComponentsInChildren<SpriteRenderer>();

        if (animator != null)
            animator.SetFloat("attackSpeed", 1 / attackSpeed);
        if (animator != null)
            animator.SetFloat("speed", 1);

        rb = GetComponent<Rigidbody2D>();

        FindObjectOfType<AllySpawner>().potionEnemyEvent += PotionEffect;
    }
    private void Update()
    {
        if (healthCurrent <= 0 && isDie == false)
        {
            isDie = true;
        }

        if (!isDie)
        {
            DetectAlly();
            if (!isAttack)
                rb.velocity = new Vector2(-1, rb.velocity.y).normalized * speed;
            else
                rb.velocity = new Vector2(0.0001f, 0);

            rb.velocity = new Vector2(rb.velocity.x + 0.0001f, rb.velocity.y);
        }
        else
        {
            if (isOnceDie)
                Death();
        }
    }
    public void PotionEffect(int value)
    {
        if (value == 2)
        {
            LightningStrike(60);
        }
        if (value == 3)
        {
            StartCoroutine(Buff(45, 2));
        }
        if (value == 9)
        {
            LightningStrike(40);
        }
        if (value == 10)
        {
            StartCoroutine(Buff(20, 1));
        }
    }
    private void LightningStrike(int value)
    {
        Damaged(healthCurrent / 100 * value);
    }
    private IEnumerator Buff(int duration, int value)
    {
        if (value == 1)
        {
            speed /= 2;
            if (animator != null)
                animator.SetFloat("speed", 0.5f);

            attackSpeed *= 2;
            if (animator != null)
                animator.SetFloat("attackSpeed", 1 / attackSpeed);
        }
        if (value == 2)
        {
            luckyPotionMultiplier = 2;
        }

        yield return new WaitForSeconds(duration);

        if (value == 1)
        {
            speed *= 2;
            if (animator != null)
                animator.SetFloat("speed", 1);

            attackSpeed /= 2;
            if (animator != null)
                animator.SetFloat("attackSpeed", 1 / attackSpeed);
        }
        if (value == 2)
        {
            luckyPotionMultiplier = 1;
        }
    }
    public bool IsDead()
    {
        return isDie;
    }

    private int tempDuration;
    private bool isHpBarShowed;
    public void Damaged(float damage)
    {
        healthCurrent -= damage;
        tempDuration = 4;

        if (!isHpBarShowed)
        {
            isHpBarShowed = true;
            StartCoroutine(HpBarDuration());
        }

        healthBar.SetHealth(healthCurrent);
        StartCoroutine(ColorDelay(enemyGFXcollection, new Color(1, 0.5f, 0.5f)));
    }
    private IEnumerator HpBarDuration()
    {
        healthBarObject.SetActive(true);
        while(tempDuration != 0)
        {
            yield return new WaitForSeconds(1);
            tempDuration--;
        }
        healthBarObject.SetActive(false);
        isHpBarShowed = false;
    }
    private IEnumerator ColorDelay(SpriteRenderer[] spriteCollection, Color color)
    {
        foreach (SpriteRenderer e in spriteCollection)
            e.color = color;

        yield return new WaitForSeconds(0.1f);

        foreach (SpriteRenderer e in spriteCollection)
            e.color = Color.white;
    }
    private void Death()
    {
        isOnceDie = false;
        FindObjectOfType<AllySpawner>().potionEnemyEvent -= PotionEffect;
        gameObject.transform.SetParent(null);
        gameObject.GetComponent<Collider2D>().enabled = false;
        healthBarObject.SetActive(false);
        rb.velocity = new Vector2(0, 0);
        StopCoroutine(nameof(Attack));
        shadow.SetActive(false);

        if (animator != null)
            animator.SetBool("isDie", isDie);

        if (PlayerPrefs.GetInt("currencyTimer") == 1)
            CurrensyDrop();

        Destroy(gameObject, 2f);
    }
    private void CurrensyDrop()
    {
        StartCoroutine(CrystalDropDelay(0.1f));
        StartCoroutine(CoinDropDelay(0.15f));
    }
    private IEnumerator CoinDropDelay(float delay)
    {
        for (int i = 0; i < coinQuantity * luckyPotionMultiplier; i++)
        {
            yield return new WaitForSeconds(delay);
            int valueCoin = Random.Range(0, 100);
            if (valueCoin <= coinDropChanse)
            {
                GameObject coin = Instantiate(coinObject, transform);
                Destroy(coin, 2f);
                PlayerPrefs.SetInt("coins", PlayerPrefs.GetInt("coins") + 1);
            }
        }
    }
    private IEnumerator CrystalDropDelay(float delay)
    {
        for (int i = 0; i < luckyPotionMultiplier; i++)
        {
            int valueCrystal = Random.Range(0, 100);
            if (valueCrystal <= crystalDropChanse)
            {
                GameObject crystal = Instantiate(crystalObject, transform);
                Destroy(crystal, 2f);
                PlayerPrefs.SetInt("crystals", PlayerPrefs.GetInt("crystals") + 1);
            }
            yield return new WaitForSeconds(delay);
        }
    }
    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackSpeed - attackSpeed / 6);
            GameObject d = Instantiate(damageBox, transform);
            d.GetComponent<DamageBox>().DamageValue(damage);
            yield return new WaitForSeconds(attackSpeed / 6);
        }
    }
    private void DetectAlly()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.left), attackRange, LayerMask.GetMask("Ally"));
        if (hit && isOnceAttack)
        {
            isOnceAttack = false;
            isAttack = true;

            if (animator != null)
                animator.SetBool("isAttack", isAttack);

            StartCoroutine(nameof(Attack));
        }
        if (!hit)
        {
            isOnceAttack = true;
            isAttack = false;

            if (animator != null)
                animator.SetBool("isAttack", isAttack);

            StopCoroutine(nameof(Attack));
        }
    }
}
