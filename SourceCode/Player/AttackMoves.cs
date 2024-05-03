using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.TextCore.Text;

public class AttackMoves : MonoBehaviour//可以考虑换一下继承脚本
{
    public static AttackMoves Instance;
    public enum AttackType { Sample, Charge, Block, Skill, Null }

    [Header("基本参数")]
    public LayerMask enemy;
    public AudioSource audioSource;

    [Header("攻击参数")]
    private Attack attack;

    [Header("玩家等级")]
    public float levelLimit;
    public float level = 0;
    private int currentLevel;
    private float volume = 30;
    public SpriteRenderer powerImage;
    public ParticleSystem PowerFX;
    public CinemachineVirtualCamera cft;

    [Header("动画参数")]
    public Animator powerAnim;
    public Animator powerAnim_pre;

    [Header("事件广播")]
    public SetGoalTimeSO setBGMIndex;

    [Header("事件监听")]
    public VoidEventSO afterloadEvent;

    public bool allowAttack;
    public AttackType attackType;

    public Collider2D currentCollider, sample, charge, skill, block;

    public AttackAudioClips attackAudioClips;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        attack = GetComponent<Attack>();
        cft.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = Vector3.zero;
    }

    private void OnEnable()
    {
        afterloadEvent.onEventRiased += Recover;
    }

    private void OnDisable()
    {
        afterloadEvent.onEventRiased -= Recover;
    }

    private void Recover()
    {
        level = 0;
        GetComponent<Character>().currentHealth = GetComponent<Character>().maxHealth;
    }

    private void FixedUpdate()
    {
        if (PlayerController.instance.isBreakLevel)
            DecreaseVolume(currentLevel);
    }

    private void Update()
    {
        CharacterLevel();
        PowerAnimationSwitch();

        if (PlayerController.instance.isBreakLevel)
            AddVolume();

        AllowAttack();

        if (allowAttack)
            AttackTypeSwitch();

        switch (attackType)
        {
            case AttackType.Sample:
                SampleAttack();
                break;
            case AttackType.Charge:
                break;
            case AttackType.Skill:
                Skill();
                break;
            case AttackType.Block:
                Block();
                break;

        }
    }

    void PowerAnimationSwitch()
    {
        powerAnim.SetFloat("Level", level);
        powerAnim_pre.SetFloat("Level", level);
    }

    void AttackTypeSwitch()
    {
        if (Input.GetKey(KeyCode.J))
            attackType = AttackType.Sample;

        //AttackType.Charge的切换在“Charge”函数里

        if (PlayerController.instance.moveMent != Vector2.zero && Input.GetKeyDown(KeyCode.K) &&/* PlayerController.instance.*/level >= 3)
            attackType = AttackType.Skill;

        if (PlayerController.instance.moveMent == Vector2.zero && Input.GetKey(KeyCode.K) && /*PlayerController.instance.*/level >= 2)
            attackType = AttackType.Block;

        if (attackType != AttackType.Null)
            PlayerController.instance.allowInput = false;

        GetComponent<BoxCollider2D>().enabled = !PlayerController.instance.isBreakLevel;
        GetComponent<BoxCollider2D>().enabled = !PlayerController.instance.isSkill;

        SetDamge();
    }

    #region 招式
    void SampleAttack()
    {
        PlayerController.instance.isAttack = true;
        if (Input.GetKeyUp(KeyCode.J))
        {
            PlayerController.instance.anim.SetTrigger("attack");
            PlayAudio(attackAudioClips.sampleAttackFX);
        }
        //TODO:普攻动作
    }//普攻

    void Charge()
    {
        if (/*PlayerController.instance.*/level >= 1)
            PlayerController.instance.isCharge = true;
        attackType = AttackType.Charge;
    }//蓄力过程（动画事件）

    void Block()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayAudio(attackAudioClips.blockFx);
            PlayerController.instance.isBlock = true;
        }


        if (Input.GetKeyUp(KeyCode.K))
            PlayerController.instance.isBlock = false;

        //TODO:招架的动画、判定
    }//招架

    void Skill()
    {
        PlayAudio(attackAudioClips.skillFX);
        PlayerController.instance.rb.AddForce(PlayerController.instance.moveMent * attack.attackForce, ForceMode2D.Impulse);
    }//技能

    void SetDamge()
    {
        int currentdamage = attackType switch
        {
            AttackType.Sample => 2,
            AttackType.Charge => 4,
            AttackType.Skill => 6,
            AttackType.Block => 0,
            AttackType.Null => 0,
            _ => 0
        };

        int currentforce = attackType switch
        {
            AttackType.Sample => 8,
            AttackType.Charge => 10,
            AttackType.Skill => 15,
            AttackType.Block => 0,
            AttackType.Null => 0,
            _ => 0
        };

        currentCollider = attackType switch
        {
            AttackType.Sample => sample,
            AttackType.Charge => charge,
            AttackType.Skill => skill,
            AttackType.Block => block,
            AttackType.Null => null,
            _ => null
        };

        attack.damage = /*PlayerController.instance.*/level > 0 ? currentdamage * (int)/*PlayerController.instance.*/level : currentdamage;
        attack.attackForce = currentforce;
    }//设定伤害值

    void AllowAttack()
    {

        if (allowAttack)
            return;

        if (attackType == AttackType.Null || !PlayerController.instance.isBreakLevel)
            allowAttack = true;

    }//允许攻击
    #endregion

    #region 等级突破
    void CharacterLevel()
    {
        float currentpower = GetComponent<Character>().currentHealth % levelLimit;
        currentLevel = (int)(GetComponent<Character>().currentHealth / levelLimit);

        if (currentLevel > level)
        {
            PowerFX.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.L))
                PlayerController.instance.isBreakLevel = true;

        }

        if (currentLevel < level)
        {
            level=currentLevel;
            PowerFX.gameObject.SetActive(false);
            powerImage.color = new Color(255, 255, 255, (float)currentpower / levelLimit);
        }
    }

    void DecreaseVolume(int currentLevel)
    {
        PlayerController.instance.allowInput = false;

        volume = currentLevel switch
        {
            1 => volume - 0.15f * currentLevel,
            2 => volume - 0.25f * currentLevel,
            3 => volume - 0.3f * currentLevel,
            _ => volume - 0.3f * currentLevel
        };

        if (volume >= 100)
        {
            PlayAudio(attackAudioClips.breakLevelFX);
            PlayerController.instance.anim.SetFloat("volume", volume);
            cft.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(0, 0, -level * 1.5f);
            PowerFX.gameObject.SetActive(false);

            volume = 30;
            level++;
            PlayerController.instance.isBreakLevel = false;
            PlayerController.instance.speed += level * 0.5f;
            setBGMIndex.setGoalTime((int)level);
            AudioManager.instance.sources[(int)level].time = AudioManager.instance.sources[(int)level - 1].time;

        }//“突破”成功

        else if (volume <= 0)
        {
            PlayerController.instance.anim.SetFloat("volume", volume);
            GetComponent<Character>().currentHealth -= 25;
            volume = 30;
            PlayerController.instance.isBreakLevel = false;
        }//“突破”失败
    }

    void AddVolume()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayAudio(attackAudioClips.addVolumeFX);
            ScreenImpulse.Instance.Impulse(new Vector3(0, 0, 0.3f), 0.4f);
            volume += 5;
        }

    }

    #endregion

    public void RecoverForce() => PlayerController.instance.rb.velocity = new Vector2(0, PlayerController.instance.rb.velocity.y);//恢复物体速度（动画事件）

    public void EnableCurrentCollider() => currentCollider.enabled = true;//启用攻击判定碰撞（动画事件）

    public void DisenableCurrentCollider()
    {
        if (currentCollider != null)
            currentCollider.enabled = false;
    }//禁用攻击判定碰撞（动画事件）

    #region 音效
    public void PlayAudio(AudioClip clip)
    {
        GetComponent<AudioSource>().clip = clip;
        GetComponent<AudioSource>().Play();
    }
    public void PlayAudio(AudioClip[] clip)
    {
        int index = Random.Range(0, clip.Length);
        GetComponent<AudioSource>().clip = clip[index];
        GetComponent<AudioSource>().Play();
    }
    #endregion

}

[System.Serializable]
public class AttackAudioClips
{
    [Header("音效")]
    public AudioClip[] sampleAttackFX;
    public AudioClip chargeAttackFX;
    public AudioClip skillFX;
    public AudioClip blockFx;
    public AudioClip addVolumeFX;
    public AudioClip breakLevelFX;

}