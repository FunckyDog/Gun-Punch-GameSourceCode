using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [HideInInspector]
    public Vector2 moveMent;
    [HideInInspector]
    public bool allowInput;
    [HideInInspector]
    public Rigidbody2D rb;

    private float currentSpeed;
    private Character character;
    private bool enteredSurvivorMode;

    [Header("基本设置")]
    public float speed;
    public float invincibilityTime;
    public AudioSource audioSource;

    [Header("动画参数")]
    public Animator anim;

    [HideInInspector]
    public bool isAttack, isCharge, isBlock, isSkill, isinvincibility, isBreakLevel;

    [Header("事件广播")]
    public GetObjectSO setPlayer;
    public VoidEventSO breakLevel;
    public VoidEventSO gameOver;
    public VoidEventSO gameFinished;

    [Header("事件监听")]
    public VoidEventSO enterSurvivorMode;

    public PlayerAudioClips playerAudioClips;

    private void OnEnable()
    {
        enterSurvivorMode.onEventRiased += EnteredSurvivorMode;
    }

    private void OnDisable()
    {
        enterSurvivorMode.onEventRiased -= EnteredSurvivorMode;
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
        allowInput = true;
        setPlayer.GetObject(gameObject);
    }

    private void Update()
    {
        if (allowInput)
            MoveInput();

        AnimationSwitch();

    }

    private void FixedUpdate()
    {
        if (allowInput)
            Move();
        /*if (isBreakLevel)
            DecreaseVolume(currentLevel);*/
    }

    private void EnteredSurvivorMode()
    {
        enteredSurvivorMode = true;
    }

    private void Move()
    {

        if (moveMent.x != 0)
            transform.localScale = new Vector3(moveMent.x, transform.localScale.y, transform.localScale.z);

        currentSpeed = Mathf.Lerp(currentSpeed, speed, 0.5f);
        rb.MovePosition((Vector2)transform.position + moveMent.normalized * currentSpeed * Time.deltaTime);//移动

        if (!Input.GetButton("Horizontal") && !Input.GetButton("Vertical"))
            currentSpeed = 0;
    }

    private void MoveInput()
    {
        moveMent.x = Input.GetAxisRaw("Horizontal");
        moveMent.y = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.LeftShift))//奔跑
            currentSpeed*=1.5f;

        if (Input.GetKeyUp(KeyCode.LeftShift))
            currentSpeed = speed;
    }

    private void AnimationSwitch()
    {
        anim.SetFloat("Move", moveMent.magnitude * currentSpeed);
        anim.SetBool("isAttack", isAttack);
        anim.SetBool("isCharge", isCharge);
        anim.SetBool("isBlock", isBlock);
        anim.SetBool("isSkill", isSkill);
        anim.SetBool("isBreakLevel", isBreakLevel);

        /* powerAnim.SetFloat("Level", level);
         powerAnim_pre.SetFloat("Level", level);*/
        //anim.SetBool("isinvincibility", isinvincibility);
    }

    #region 受伤&死亡
    public void Invincibility()
    {
        allowInput = false;
        if (character.currentHealth == 0)
            StartCoroutine(DeadTimeScale());
        else
        {
            isinvincibility = true;
            anim.SetTrigger("Invincibility");
            GetComponent<BoxCollider2D>().enabled = false;
            StartCoroutine(IInvincibility());
        }

    }//受伤（参与事件）

    IEnumerator IInvincibility()
    {
        var binkTime = 0.2f;
        for (float i = 0; i < invincibilityTime; i += binkTime)
        {
            GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(binkTime);
        }
        GetComponent<SpriteRenderer>().enabled = true;
        isinvincibility = false;
        GetComponent<BoxCollider2D>().enabled = true;
        allowInput = true;

    }

    IEnumerator DeadTimeScale()
    {
        GetComponent<AttackMoves>().allowAttack = false;
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(2);
        if (Time.timeScale <= 1)
        {
            Time.timeScale += 1f;
            yield return new WaitForSeconds(0.1f);
        }
    }

    void DeadEvent()
    {
        if (enteredSurvivorMode)
        {
            gameFinished.EventRaised();
            enteredSurvivorMode = false;
        }

        else
            gameOver.EventRaised();
    }//死亡事件（动画事件）
    #endregion

    #region 音效
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

    public void WalkStepAudio() => PlayAudio(playerAudioClips.walkStepFX);
    public void RunStepAudio() => PlayAudio(playerAudioClips.runStepFX);
    #endregion
}

[System.Serializable]
public class PlayerAudioClips
{
    public AudioClip[] walkStepFX;
    public AudioClip[] runStepFX;

}
