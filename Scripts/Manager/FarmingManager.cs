using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FarmingManager : Singleton<FarmingManager>
{

    [Header("基本参数")]
    public float farmingRange;//生成范围
    public float farmingTime = 5;//生成的时间间隔
    public GameObject[] enemys;
    private float waitTime;

    [Header("概率区间")]
    public int randomCount;
    public float intervalMax, intervalMin;

    [Header("区间分界（小数占比）")]
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
    }//敌人刷新的时间间隔

    public void IntervalDamage(Attack attacker)
    {
        intervalMin -= attacker.damage;
        intervalMax -= attacker.damage;
        if (farmingTime < 2)
            farmingTime += 0.2f;
    }//受伤时改变区间

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
