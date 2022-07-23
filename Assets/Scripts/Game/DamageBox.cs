using UnityEngine;

public class DamageBox : MonoBehaviour
{
    [SerializeField] private string charType;
    [SerializeField] private bool range;

    private bool isOnce = true;
    private float damageValue;
    private void Start()
    {
        if (range && charType == "ally")
            InvokeRepeating(nameof(Range), 0.02f, 0.02f);

        if (range && charType == "enemy")
            InvokeRepeating(nameof(RangeE), 0.02f, 0.02f);

        Destroy(gameObject, .2f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (charType == "ally")
        {
            if (collision.gameObject.tag == "Enemy" && isOnce)
            {
                isOnce = false;
                collision.gameObject.GetComponent<Enemy>().Damaged(damageValue);
                Destroy(gameObject);
            }
            if (collision.gameObject.name == "CastleEnemy" && isOnce)
            {
                isOnce = false;
                collision.gameObject.GetComponent<Castle>().Damaged(damageValue);
                Destroy(gameObject);
            }
        }
        if (charType == "healer")
        {
            if (collision.gameObject.tag == "Ally" && isOnce && !collision.gameObject.GetComponent<Ally>().FullHp())
            {
                isOnce = false;
                collision.gameObject.GetComponent<Ally>().Heal(damageValue);
                Destroy(gameObject);
            }
        }

        if (charType == "enemy")
        {
            if (collision.gameObject.tag == "Ally" && isOnce)
            {
                isOnce = false;
                collision.gameObject.GetComponent<Ally>().Damaged(damageValue);
                Destroy(gameObject);
            }
            if (collision.gameObject.tag == "AllyHealer" && isOnce)
            {
                isOnce = false;
                collision.gameObject.GetComponent<Ally>().Damaged(damageValue);
                Destroy(gameObject);
            }
            if (collision.gameObject.name == "CastleAlly" && isOnce)
            {
                isOnce = false;
                collision.gameObject.GetComponent<Castle>().Damaged(damageValue);
                Destroy(gameObject);
            }
        }
        if (charType == "enemyAOE")
        {
            if (collision.gameObject.tag == "Ally")
            {
                isOnce = false;
                collision.gameObject.GetComponent<Ally>().Damaged(damageValue);
                Destroy(gameObject);
            }
            if (collision.gameObject.tag == "AllyHealer")
            {
                isOnce = false;
                collision.gameObject.GetComponent<Ally>().Damaged(damageValue);
                Destroy(gameObject);
            }
            if (collision.gameObject.name == "CastleAlly")
            {
                isOnce = false;
                collision.gameObject.GetComponent<Castle>().Damaged(damageValue);
                Destroy(gameObject);
            }
        }
    }
    public void DamageValue(float value)
    {
        damageValue = value;
    }
    private void Range()
    {
        transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, transform.position.z);
    }
    private void RangeE()
    {
        transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, transform.position.z);
    }
}
