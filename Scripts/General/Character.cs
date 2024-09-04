using UnityEngine;
using UnityEngine.Events;


public class Character : MonoBehaviour
{

    [Header("����ֵ")]
    public int initialHealth;
    public int currentHealth;

    [Header("����&����")]
    public UnityEvent Hurt;

    protected virtual void Awake()
    {
        currentHealth = initialHealth;
    }

    public virtual void TakeDamage(Attack attacker)
    {
        currentHealth -= attacker.damage;
        Hurt?.Invoke();
        transform.GetComponent<Rigidbody2D>().AddForce((transform.position - attacker.transform.position).normalized * attacker.attackForce);
    }


}

