using UnityEngine;

public class ShotGun : EnemyController
{
    public int bulletNum;
    public float bulletAngle;

    protected override void Fire()
    {
        int median = bulletNum / 2;
        for (int i = 0; i < bulletNum; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, (Vector2)transform.position + shootDirection * 1f, Quaternion.identity);

            if (bulletNum % 2 == 1)
                bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis((i - median) * bulletAngle, Vector3.forward) * shootDirection);
            else
                bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis((i - median) * bulletAngle + bulletAngle / 2, Vector3.forward) * shootDirection);
        }
    }
}
