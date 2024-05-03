using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyController : MonoBehaviour
{
    [Header("��������")]
    public float speed;
    protected float lastShootTime;
    private Animator anim;
    [HideInInspector]
    public Rigidbody2D rb;
    public AudioSource audioSource;

    [Header("�ӵ�")]
    public GameObject bulletPrefab;
    //protected GameObject bullet;
    protected Vector2 shootDirection;

    [Header("�¼�����")]
    public GetObjectSO getObject;
    public VoidEventSO breakLevel;
    public VoidEventSO gameOver;
    public VoidEventSO gameFinished;

    [Header("�ܻ�")]
    private bool isHurt;
    protected GameObject player;

    public EnemyAudioClips enemyAudioClips;
    private void Awake()
    {
        lastShootTime = bulletPrefab.GetComponent<Attack>().attackRate;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        breakLevel.onEventRiased += BreakLevel;
        gameOver.onEventRiased += Stop;
        gameFinished.onEventRiased += Stop;
    }

    private void OnDisable()
    {
        breakLevel.onEventRiased -= BreakLevel;
        gameOver.onEventRiased -= Stop;
        gameFinished.onEventRiased -= Stop;
    }

    private void Stop()
    {
        enabled = false;
    }

    private void BreakLevel() => GetComponent<Character>().currentHealth -= 10;

    private void FixedUpdate()
    {
        if (!isHurt || GetComponent<Character>().currentHealth != 0)
            MoveToTarget(player.transform);
    }

    private void Update()
    {

        if (!isHurt)
        {
            Shoot();
            LookAtTarget(player.transform);
        }

        AnimationSwtch();
    }

    private void AnimationSwtch()
    {
        anim.SetBool("isHurt", isHurt);
    }

    void LookAtTarget(Transform target)
    {
        float posLocalToTagrget = (target.position - transform.position).normalized.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(posLocalToTagrget, transform.localScale.y, transform.localScale.z);
    }

    void MoveToTarget(Transform target)
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    protected virtual void Shoot()
    {
        shootDirection = (player.transform.position - transform.position).normalized;

        if (lastShootTime >= 0)
            lastShootTime -= Time.deltaTime;

        if (lastShootTime < 0)
        {
            Fire();
            PlayAudio(enemyAudioClips.shootFX);

            lastShootTime = bulletPrefab.GetComponent<Attack>().attackRate;
        }
    }

    protected virtual void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab, (Vector2)transform.position + shootDirection * 0.8f, Quaternion.identity);

        float randomAngle = Random.Range(-5f, 5f);//���������ƫת�Ƕ�

        bullet.GetComponent<Bullet>().SetSpeed(Quaternion.AngleAxis(randomAngle, Vector3.forward) * shootDirection);
    }

    public void GetHit()
    {
        StartCoroutine(GGetHit());
    }

    IEnumerator GGetHit()
    {
        isHurt = true;
        yield return new WaitForSeconds(0.8f);
        isHurt = false;
    }//�ܻ�

    public void RecoverFoece() => rb.velocity = Vector3.zero;//�ָ������ٶȣ������¼���

    public void Destroy() => Destroy(gameObject);//�������壨�����¼���

    #region ��Ч
    public void PlayAudio(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public void PlayAudio(AudioClip[] clip)
    {
        int index = Random.Range(0, clip.Length);
        audioSource.clip = clip[index];
        audioSource.Play();
    }
    #endregion
}

[System.Serializable]
public class EnemyAudioClips
{
    public AudioClip shootFX;
    public AudioClip deadFx;
}
