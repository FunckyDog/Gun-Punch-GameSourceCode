using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;//�˺�ֵ

    public int attackForce;//����������

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Hurt Area")
        {
            other.GetComponentInParent<Character>()?.TakeDamage(this);
        }
    }
}
