using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("基本参数")]
    public float speed;
    public GameObject bulletFire;
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
        if (other.tag == "Hurt Area" && !other.GetComponent<Enemy>())
        {
            BulletFire();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        BulletFire();
    }
}
