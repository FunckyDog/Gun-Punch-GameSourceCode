using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;//�˺�ֵ

    public int attackRate;//����Ƶ��

    public int attackForce;//����������

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (tag == "Player" && PlayerController.instance.isBlock && other.tag == "Bullet")
        {
            Destroy(other.gameObject);
        }//�����ж�

            other.GetComponent<Character>()?.TakeDamage(this);

    }
}
