using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;//ÉËº¦Öµ

    public int attackForce;//¹¥»÷»÷ÍËÁ¦

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hurt Area")
        {
            other.GetComponentInParent<Character>()?.TakeDamage(this);
        }
    }
}
