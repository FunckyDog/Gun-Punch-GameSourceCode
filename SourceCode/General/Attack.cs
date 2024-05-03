using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;//ÉËº¦Öµ

    public int attackRate;//¹¥»÷ÆµÂÊ

    public int attackForce;//¹¥»÷»÷ÍËÁ¦

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (tag == "Player" && PlayerController.instance.isBlock && other.tag == "Bullet")
        {
            Destroy(other.gameObject);
        }//ÎüÊÕÅĞ¶¨

            other.GetComponent<Character>()?.TakeDamage(this);

    }
}
