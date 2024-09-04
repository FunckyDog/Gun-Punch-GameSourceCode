using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("��������")]
    public float speed;
    public float shootTime;
    private float shootWaitTime;
    private Animator anim;
    private BoxCollider2D hurtArea;

    [Header("�ӵ�")]
    public GameObject bulletPrefab;
    protected Vector2 shootDirection;

    [Header("�ܻ�")]
    private bool isHurt;

    [HideInInspector] public Rigidbody2D rb;
    protected Transform player;
    private Character enemyCharacter;

    [Header("��Ч")]
    CharacterAudio characterAudio;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyCharacter = GetComponent<Character>();
        hurtArea = GetComponent<BoxCollider2D>();
        characterAudio = GetComponent<CharacterAudio>();
    }

    private void OnDisable()
    {

    }

    private void Start()
    {
        player = PlayerController.instance.transform;
    }

    private void Update()
    {
        if (!isHurt)
        {
            Shoot();
            LookAtTarget();
            MoveToTarget();
        }

        AnimationSwtch();
    }

    private void AnimationSwtch()
    {
        anim.SetBool("isHurt", isHurt);
    }

    void LookAtTarget()
    {
        float posLocalToTagrget = shootDirection.normalized.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(posLocalToTagrget * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }

    void MoveToTarget()
    {
        rb.velocity = shootDirection * speed;
    }

    protected virtual void Shoot()
    {
        shootDirection = (player.position - transform.position).normalized;

        if (shootWaitTime <= shootTime)
            shootWaitTime += Time.deltaTime;

        else
        {
            shootWaitTime = 0;
            Fire();
        }
    }

    protected virtual void Fire()
    {
        characterAudio.PlayAudioFX(0);

        GameObject bullet = Instantiate(bulletPrefab, (Vector2)transform.position + shootDirection * 1f, Quaternion.identity);

        float randomAngle = Random.Range(-5f, 5f);//���������ƫת�Ƕ�

        bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(randomAngle, Vector3.forward) * shootDirection);
    }

    public void GetHurtAction()
    {
        StartCoroutine(GetHurt());
    }

    IEnumerator GetHurt()
    {
        characterAudio.PlayAudioFX(1);

        isHurt = true;
        rb.velocity = Vector2.zero;

        if (enemyCharacter.currentHealth <= 0)
        {
            isHurt = false;
            hurtArea.enabled = false;
            PlayerController.instance.character.currentHealth += enemyCharacter.initialHealth;
            UIManager.instance.GetEnemyCount(gameObject);

            anim.SetTrigger("dead");
            yield return new WaitForSeconds(0.4f);
        }
        else
        {
            yield return new WaitForSeconds(0.8f);
            isHurt = false;
        }

    }//����

    public void Destroy() => Destroy(gameObject);//�������壨�����¼���

}