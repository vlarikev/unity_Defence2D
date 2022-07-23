using System.Collections;
using UnityEngine;

public class Ally : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private GameObject healthBarObject;

    [SerializeField] private int unitId;
    [SerializeField] private int healthMax;
    private float healthCurrent;

    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float speed;
    [SerializeField] private float attackRange;
    [SerializeField] private GameObject damageBox;

    [Header("Specific units")]
    [SerializeField] private bool isHealer;
    [SerializeField] private bool isTank;

    [SerializeField] private Animator animator;

    [SerializeField] private GameObject shadow;
    [SerializeField] private GameObject allyGFX;
    private SpriteRenderer[] allyGFXcollection;

    [SerializeField] private GameObject healEffect;

    private bool isAttack = false;
    private bool isHeal = false;

    private bool isOnceAttack = true;
    private bool isOnceHeal = true;
    private bool isDie = false;
    private bool isOnceDie = true;

    private void Start()
    {
        healthMax = PlayerPrefs.GetInt("unit" + unitId + "hp");
        damage = PlayerPrefs.GetFloat("unit" + unitId + "dmg");
        attackSpeed = PlayerPrefs.GetFloat("unit" + unitId + "as");

        allyGFXcollection = allyGFX.GetComponentsInChildren<SpriteRenderer>();

        healthCurrent = healthMax;
        healthBar.SetMaxHealth(healthMax);
        healthBarObject.SetActive(false);

        if (animator != null)
            animator.SetFloat("attackSpeed", 1 / attackSpeed);
        rb = GetComponent<Rigidbody2D>();

        FindObjectOfType<AllySpawner>().potionAllyEvent += PotionEffect;
    }
    private void Update()
    {
        if (healthCurrent <= 0 && isDie == false)
        {
            isDie = true;
        }

        if (!isDie)
        {
            if (!isHealer)
                DetectEnemy();
            if (isHealer)
                DetectAlly();

            TankNear();

            if (!isAttack)
                rb.velocity = new Vector2(1, rb.velocity.y).normalized * speed;
            else
                rb.velocity = new Vector2(-0.0001f, 0);

            if (isHeal)
                rb.velocity = new Vector2(-0.0001f, 0);

            rb.velocity = new Vector2(rb.velocity.x -0.0001f, rb.velocity.y);
        }
        else
        {
            if (isOnceDie)
                Death();
        }
    }
    public void PotionEffect(int value)
    {
        if (value == 0)
        {
            StartCoroutine(Buff(20, true, true, true));
        }
        if (value == 5)
        {
            StartCoroutine(Buff(30, false, true, false));
        }
        if (value == 6)
        {
            StartCoroutine(Buff(30, false, false, true));
        }
        if (value == 7)
        {
            StartCoroutine(Buff(30, true, false, false));
        }
    }
    private IEnumerator Buff(int duration, bool isHP, bool isDMG, bool isAS)
    {
        if (isHP)
        {
            healthMax *= 2;
            healthCurrent *= 2;

            healthBar.SetMaxHealth(healthMax);
            healthBar.SetHealth(healthCurrent);
        }
        if (isDMG)
        {
            damage *= 2;
        }
        if (isAS)
        {
            attackSpeed /= 2;
            if (animator != null)
                animator.SetFloat("attackSpeed", 1 / attackSpeed);
        }

        yield return new WaitForSeconds(duration);

        if (isHP)
        {
            healthMax /= 2;
            healthCurrent /= 2;

            healthBar.SetMaxHealth(healthMax);
            healthBar.SetHealth(healthCurrent);
        }
        if (isDMG)
        {
            damage /= 2;
        }
        if (isAS)
        {
            attackSpeed *= 2;
            if (animator != null)
                animator.SetFloat("attackSpeed", 1 / attackSpeed);
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
        StartCoroutine(ColorDelay(allyGFXcollection, new Color(1, 0.5f, 0.5f)));
    }
    public void Heal(float heal)
    {
        if (healthCurrent + heal > healthMax)
        {
            healthCurrent = healthMax;
            healthBar.SetHealth(healthCurrent);
        }
        else
        {
            healthCurrent += heal;
            healthBar.SetHealth(healthCurrent);
        }

        tempDuration = 4;

        if (!isHpBarShowed)
        {
            isHpBarShowed = true;
            StartCoroutine(HpBarDuration());
        }

        StartCoroutine(ColorDelay(allyGFXcollection, new Color(0.5f, 1, 0.5f)));

        GameObject hb = Instantiate(healEffect, gameObject.transform);
        Destroy(hb, 0.37f);
    }
    private IEnumerator HpBarDuration()
    {
        healthBarObject.SetActive(true);
        while (tempDuration != 0)
        {
            yield return new WaitForSeconds(1);
            tempDuration--;
        }
        healthBarObject.SetActive(false);
        isHpBarShowed = false;
    }
    public bool FullHp()
    {
        if (healthCurrent == healthMax)
            return true;
        else
            return false;
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
        FindObjectOfType<AllySpawner>().potionAllyEvent -= PotionEffect;
        gameObject.transform.SetParent(null);
        healthBarObject.SetActive(false);
        rb.velocity = new Vector2(0, 0);
        StopCoroutine(nameof(Attack));
        shadow.SetActive(false);
        if (animator != null)
            animator.SetBool("isDie", isDie);
        gameObject.GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject, 2f);
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
    private IEnumerator HealDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackSpeed);
            GameObject d = Instantiate(damageBox, transform);
            d.GetComponent<DamageBox>().DamageValue(damage);
        }
    }
    private void DetectEnemy()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), attackRange, LayerMask.GetMask("Enemy"));
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

    private void DetectAlly()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.TransformDirection(Vector2.right), attackRange, LayerMask.GetMask("Enemy"));
        if (hit && isOnceAttack)
        {
            isOnceAttack = false;
            isAttack = true;

            if (animator != null)
                animator.SetBool("isAttack", isAttack);
        }
        if (!hit)
        {
            isOnceAttack = true;
            isAttack = false;

            if (animator != null)
                animator.SetBool("isAttack", isAttack);
        }

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.TransformDirection(Vector2.right), attackRange, LayerMask.GetMask("Ally"));
        int healerCheker = 0;
        foreach (RaycastHit2D obj in hits)
        {
            if (obj.collider.gameObject.tag == "Ally" && isOnceHeal && !obj.collider.gameObject.GetComponent<Ally>().FullHp())
            {
                isOnceHeal = false;
                isHeal = true;

                if (animator != null)
                    animator.SetBool("isHeal", isHeal);

                StartCoroutine(nameof(HealDelay));
            }
            if(obj.collider.gameObject.name == "Healer(Clone)" || obj.collider.gameObject.name != "Healer(Clone)" && obj.collider.gameObject.GetComponent<Ally>().FullHp())
            {
                healerCheker++;                
            }
        }
        if (hits.Length == healerCheker)
        {
            isOnceHeal = true;
            isHeal = false;

            if (animator != null)
                animator.SetBool("isHeal", isHeal);

            StopCoroutine(nameof(HealDelay));
        }
    }
    private void TankNear()
    {
        int tankChecker = 0;
        RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector3(transform.position.x + 0.45f, transform.position.y, transform.position.z), transform.TransformDirection(Vector2.right), 0.1f, LayerMask.GetMask("Ally"));
        foreach (RaycastHit2D obj in hits)
        {
            if (obj.collider.gameObject.name == "Tank(Clone)")
            {
                if (gameObject.name != "Tank(Clone)")
                    tankChecker++;
            }
        }
        if (tankChecker != 0)
            gameObject.GetComponent<Collider2D>().enabled = false;
        else
            gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
