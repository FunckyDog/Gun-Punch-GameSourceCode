using UnityEngine;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("����ֵ")]
    public int maxHealth;
    public int currentHealth;

    [Tooltip("������Ч")]
    public GameObject AttackFXPrefab;

    [Header("�¼��㲥")]
    public GetObjectSO setEnemy;

    public UnityEvent<Character> onHealthChange;
    public UnityEvent<Character> onGetHit;

    private void Awake()
    {
        if (tag == "Enemy")
            currentHealth = maxHealth;
    }

    public void TakeDamage(Attack attacker)
    {
        if (tag == "Player")
        {
            if (!PlayerController.instance.isinvincibility)
            {
                currentHealth = Mathf.Max(currentHealth - attacker.damage, 0);

                FarmingManager.instance.IntervalDamage(attacker);
                onHealthChange?.Invoke(this);
            }

            if (PlayerController.instance.isBlock && tag == "AttackCollider")
                currentHealth += attacker.damage;
        }


        if (tag == "Enemy" && attacker.tag == "Player")
        {
            currentHealth = Mathf.Max(currentHealth - attacker.damage, 0);
            attacker.GetComponent<Character>().currentHealth += attacker.damage;
            onGetHit?.Invoke(this);
        }

        /*
        if (!PlayerController.instance.isBlock)
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            GetComponent<Rigidbody2D>().AddForce((transform.position - attacker.transform.position).normalized * attacker.GetComponent<Attack>().attackForce, ForceMode2D.Impulse);
        }*/

        if (tag == "AttackCollider" && attacker.tag == "Bullet")
        {
            Destroy(gameObject);
            GetComponent<Character>().currentHealth += attacker.damage;
        }//�����ӵ�

        ScreenImpulse.Instance.Impulse((transform.position - attacker.transform.position).normalized * 0.3f, 0.2f);
        GameObject attackFX = Instantiate(AttackFXPrefab, transform.position, Quaternion.identity);
        attackFX.transform.Rotate(0, 0, Vector3.Angle(-(transform.position - attacker.transform.position).normalized, Vector3.up));

        if (currentHealth == 0)
        {
            setEnemy?.GetObject(gameObject);
            GetComponent<Animator>().SetTrigger("Dead");
        }

    }
}

