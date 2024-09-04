using System.Collections;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{

    [Header("属性")]
    public PlayerData[] playerDatas;
    public PlayerData currentData;
    public BoxCollider2D hurtArea;

    [Header("等级")]
    public int maxLevel;
    public int currentLevel;

    [Header("状态")]
    #region
    public bool isRun;
    public bool isinvincibility;
    public bool isSampleAttack;
    public bool isCharge;
    public bool isBlock;
    public bool isSkill;
    public bool isBreakLevel;
    #endregion

    [Header("音效")]
    public CharacterAudio characterAudio;

    public bool allowInput;
    private SpriteRenderer spriteRenderer;
    [HideInInspector] public Character character;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Vector2 moveMent;
    [HideInInspector] public int ScaleX = 1;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        characterAudio = GetComponent<CharacterAudio>();
        allowInput = true;
        currentLevel = 1;
    }

    private void OnEnable()
    {
        EventsHandler.AfterSceneLoadEvent += ResetValue;
    }

    private void OnDisable()
    {
        EventsHandler.AfterSceneLoadEvent -= ResetValue;
    }

    private void Start()
    {
        ChangeData(playerDatas[currentLevel - 1]);
    }

    private void Update()
    {
        GetInput();

        if (allowInput)
        {
            Move();
            Direction();
        }

        else
            moveMent = Vector2.zero;
    }

    void ResetValue()
    {
        allowInput = true;
        currentLevel = 1;
        ChangeData(playerDatas[currentLevel - 1]);

    }

    void GetInput()
    {
        moveMent.x = Input.GetAxis("Horizontal");
        moveMent.y = Input.GetAxis("Vertical");

        isRun = Input.GetKey(KeyCode.LeftShift);
    }

    private void Move()
    {
        if (isRun)
            rb.velocity = moveMent * currentData.speed * 1.5f;

        else
            rb.velocity = moveMent * currentData.speed;

    }//移动

    void Direction()
    {
        if (moveMent.x != 0)
            ScaleX = moveMent.x > 0 ? 1 : -1;
        transform.localScale = new Vector3(ScaleX * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }//人物转向


    #region 受伤&死亡
    public void GetHurt()
    {
        allowInput = false;

        if (character.currentHealth <= -20)
            StartCoroutine(DeadCutScene());

        else
        {
            isinvincibility = true;
            PlayerAnimation.instance.playerAnim.SetTrigger("Invincibility");
            StartCoroutine(Invincibility());
        }

        characterAudio.PlayAudioFX(0);

    }//受伤（事件）

    IEnumerator Invincibility()
    {
        hurtArea.enabled = false;

        var binkSpeed = 0.2f;
        for (float i = 0; i < currentData.invincibilityTime; i += binkSpeed)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(binkSpeed);
        }

        spriteRenderer.enabled = true;
        isinvincibility = false;
        hurtArea.enabled = true;
        allowInput = true;
    }

    IEnumerator DeadCutScene()
    {

        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(2);
        while (Time.timeScale <= 1)
        {
            Time.timeScale += Time.unscaledDeltaTime;
            yield return null;
        }

        EventsHandler.CallGameLose();

    }

    #endregion


    //数据
    public void ChangeData(PlayerData playerData)
    {
        if (currentData == playerData)
            return;

        else
        {
            currentData = playerData;
            currentLevel = playerData.level;
            AttackMoves.instance.currentData = currentData;
        }
    }

}
