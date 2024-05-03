using JetBrains.Annotations;
using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("基本参数")]
    public float speed;
    public GameObject bulletFire;
    //public Vector3 bulletdirection;
    private Rigidbody2D rb;
    float angle;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void SetSpeed(Vector2 direction)
    {
        rb.velocity = direction * speed;
        angle = Vector3.Angle(Vector3.right, direction);
        angle = direction.y > 0 ? angle : -angle;
        transform.Rotate(0, 0, angle);
        Destroy(gameObject, 3);
        BulletFire();
    }

    void BulletFire()
    {
        Instantiate(bulletFire, transform.position, Quaternion.identity);
        bulletFire.transform.Rotate(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            BulletFire();
            Destroy(gameObject);
        }
    }
}
