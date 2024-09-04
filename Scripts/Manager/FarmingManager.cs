using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FarmingManager : Singleton<FarmingManager>
{

    [Header("��������")]
    public float farmingRange;//���ɷ�Χ
    public float farmingTime = 5;//���ɵ�ʱ����
    public GameObject[] enemys;
    private float waitTime;

    [Header("��������")]
    public int randomCount;
    public float intervalMax, intervalMin;

    [Header("����ֽ磨С��ռ�ȣ�")]
    public List<float> intervalCuttingPoint = new List<float>();

    private GameObject player;
    private float currentFarmingtime;

    protected override void Awake()
    {
        base.Awake();
        waitTime = currentFarmingtime = farmingTime;
    }

    private void OnEnable()
    {
        EventsHandler.AfterSceneLoadEvent += ResetValve;
        EventsHandler.AfterSceneLoadEvent += FarmTime;
    }

    private void OnDisable()
    {
        EventsHandler.AfterSceneLoadEvent -= ResetValve;
        EventsHandler.AfterSceneLoadEvent -= FarmTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, farmingRange);
    }

    private void Start()
    {
        player = PlayerController.instance.gameObject;
    }

    private void Update()
    {
        if (LoadManager.instance.currentSceneData?.sceneIndex == 2)
        {
            intervalMin += Time.deltaTime * 0.5f;
            intervalMax = player.GetComponent<Character>().currentHealth + 30 + intervalMin;

            if (waitTime >= 0)
                waitTime -= Time.deltaTime;
            else if (waitTime < 0)
            {
                FarmEneny();
            }
        }

    }

    void FarmTimeAction()
    {
        if (LoadManager.instance.currentSceneData?.sceneIndex == 2)
            InvokeRepeating("FarmTime", 0, 1);
    }

    void FarmTime()
    {
        currentFarmingtime -= 0.01f;
    }//����ˢ�µ�ʱ����

    public void IntervalDamage(Attack attacker)
    {
        intervalMin -= attacker.damage;
        intervalMax -= attacker.damage;
        if (farmingTime < 2)
            farmingTime += 0.2f;
    }//����ʱ�ı�����

    void FarmEneny()
    {
        Vector2 enemyPos = Random.insideUnitSphere * farmingRange;
        if (Vector3.Distance(enemyPos, transform.position) >= farmingRange - 0.5f)
        {
            randomCount = (int)Random.Range(intervalMin, intervalMax);
            for (int i = 0; i < intervalCuttingPoint.Count; i++)
                if (randomCount >= intervalMin && randomCount <= intervalCuttingPoint[i] * intervalMax)
                {
                    GameObject enemy = Instantiate(enemys[i], enemyPos, Quaternion.identity);
                    enemy.GetComponent<Enemy>().currentHealth += TimeManager.instance.totalTime / 20;
                    SceneManager.MoveGameObjectToScene(enemy, SceneManager.GetSceneByBuildIndex(LoadManager.instance.currentSceneData.sceneIndex));
                    waitTime = currentFarmingtime;
                    return;
                }
        }
    }

    void ResetValve()
    {
        waitTime = currentFarmingtime = farmingTime;
        intervalMin = intervalMax = 0;
    }
}
