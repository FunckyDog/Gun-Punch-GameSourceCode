using Cinemachine;
using UnityEngine;


public class AttackMoves : Singleton<AttackMoves>
{

    [Header("��������")]
    public Attack attack;

    public bool allowAttackInput;

    [Header("����")]
    public PlayerData currentData;

    [Header("����")]
    public AttackType attackType;
    public BoxCollider2D attackArea;

    [Header("��ҵȼ�")]
    [SerializeField] private float volume = 30;

    [Header("��Ч")]
    public SpriteRenderer powerImage;
    public ParticleSystem PowerFX;
    public CinemachineVirtualCamera cft;

    private Player playerCharacter;

    protected override void Awake()
    {
        base.Awake();
        playerCharacter = GetComponentInParent<Player>();
        allowAttackInput = true;
    }

    private void Update()
    {
        if (allowAttackInput)
            GetInput();

        PlayerController.instance.allowInput = attackType == AttackType.Null;


        CharacterLevel();

        if (PlayerController.instance.isBreakLevel)
        {
            DecreaseVolume();

            if (Input.GetKeyDown(KeyCode.L))
                AddVolume();
        }

    }

    void GetInput()
    {

        if (Input.GetKey(KeyCode.J) && !PlayerController.instance.isCharge)
        {
            attackType = AttackType.Sample;
            PlayerController.instance.isSampleAttack = true;
        }

        if (Input.GetKeyUp(KeyCode.J) && PlayerController.instance.isSampleAttack)
            SampleAttack();

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (PlayerController.instance.moveMent.x != 0 && PlayerController.instance.currentLevel >= 4)
                Skill();

            else if (PlayerController.instance.currentLevel >= 3)
            {
                PlayerController.instance.isBlock = true;
                attackType = AttackType.Block;
            }
        }

        if (Input.GetKeyUp(KeyCode.K))
        {
            attackType = AttackType.Null;
            PlayerController.instance.isBlock = false;
        }
    }

    #region ��ʽ
    void SampleAttack()
    {
        PlayerAnimation.instance.playerAnim.SetTrigger("sampleAttack");
        PlayerController.instance.characterAudio.PlayAudioFX(0);

        PlayerController.instance.isSampleAttack = false;
        PlayerController.instance.rb.AddForce(Vector2.right * PlayerController.instance.ScaleX * 10, ForceMode2D.Impulse);
        allowAttackInput = false;
    }//��ͨ����

    void Charge()
    {
        if (PlayerController.instance.currentLevel >= 2)
        {
            PlayerController.instance.characterAudio.PlayAudioFX(2);

            attackType = AttackType.Charge;
            PlayerController.instance.isCharge = true;
            PlayerController.instance.isSampleAttack = false;
            allowAttackInput = false;
        }
    }//�������̣������¼���

    void Skill()
    {
        attackType = AttackType.Skill;
        PlayerAnimation.instance.playerAnim.SetTrigger("skill");
        PlayerController.instance.rb.AddForce(PlayerController.instance.ScaleX * Vector2.right * 20, ForceMode2D.Impulse);
        allowAttackInput = false;
    }//���

    void AllowAttack()
    {



    }//������
    #endregion

    #region �ȼ�ͻ��
    void CharacterLevel()
    {
        float remainPower = currentData.volumeToNextLevel - playerCharacter.currentHealth;

        if (PlayerController.instance.currentLevel < PlayerController.instance.maxLevel)
        {
            if (remainPower <= 0)
            {
                PowerFX.gameObject.SetActive(true);
                if (Input.GetKeyDown(KeyCode.L) && !PlayerController.instance.isBreakLevel)
                {
                    PlayerController.instance.isBreakLevel = true;
                    Time.timeScale = 0.3f;
                }
            }

            else
            {
                PowerFX.gameObject.SetActive(false);
                if (PlayerController.instance.currentLevel > 1)
                    powerImage.color = new Color(1, 1, 1, 1 - remainPower / (PlayerController.instance.playerDatas[PlayerController.instance.currentLevel - 1].volumeToNextLevel - PlayerController.instance.playerDatas[PlayerController.instance.currentLevel - 2].volumeToNextLevel));
                else
                    powerImage.color = new Color(1, 1, 1, 1 - remainPower / (PlayerController.instance.playerDatas[PlayerController.instance.currentLevel - 1].volumeToNextLevel - 0));

            }
        }

    }

    void DecreaseVolume()
    {
        PlayerController.instance.allowInput = false;
        PlayerAnimation.instance.playerAnim.SetFloat("volume", volume);

        volume -= Time.deltaTime * PlayerController.instance.currentLevel / 2;

        if (volume >= 100)
        {
            PowerFX.gameObject.SetActive(false);

            volume = 30;
            PlayerController.instance.currentLevel++;
            PlayerController.instance.ChangeData(PlayerController.instance.playerDatas[PlayerController.instance.currentLevel - 1]);

            Time.timeScale = 1;
            PlayerController.instance.characterAudio.PlayAudioFX(4);
            AudioManager.instance.PlayBGM(PlayerController.instance.currentLevel - 1);

            PlayerController.instance.isBreakLevel = false;
        }//��ͻ�ơ��ɹ�

        else if (volume <= 0)
        {

            GetComponent<Character>().currentHealth -= 25 * (PlayerController.instance.currentLevel % 2);
            volume = 30;
            PlayerController.instance.isBreakLevel = false;
        }//��ͻ�ơ�ʧ��
    }

    void AddVolume()
    {
        PlayerAnimation.instance.playerAnim.SetFloat("volume", volume);
        ScreenImpulse.instance.Impulse(new Vector3(0, 0, -0.5f), 1);
        PlayerController.instance.characterAudio.PlayAudioFX(1);

        volume += 5;
    }

    #endregion

    public void EnableAttackArea()
    {
        AttackArea currentAttackArea = currentData.GetAttackArea(attackType);
        attackArea.offset = currentAttackArea.areaOffset;
        attackArea.size = currentAttackArea.areaSize;

        attack.damage = attackType switch
        {
            AttackType.Sample => currentData.damage,
            AttackType.Charge => currentData.damage * 2,
            AttackType.Skill => currentData.damage * 3,
            AttackType.Block => 0,
            _ => 0,
        };

        attackArea.enabled = true;
    }//���ù����ж���ײ���趨�˺�ֵ�������¼���

    public void DisenableAttackArea() => attackArea.enabled = false;//���ù����ж���ײ�������¼���


    #region ��Ч
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