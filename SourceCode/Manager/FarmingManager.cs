using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FarmingManager : MonoBehaviour
{
    public static FarmingManager instance;

    [Header("基本参数")]
    public float farmingRange;
    public float farmingTime = 2;
    public GameObject[] enemys;
    private float waitTime;
    public GameObject player;

    [Header("概率区间")]
    public int randomCount;
    public float intervalMax, intervalMin;

    [Header("区间分界（小数占比）")]
    public List<float> intervalCuttingPoint=new List<float>();

    private void Awake()
    {

        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        waitTime = farmingTime;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position,farmingRange);
    }

    private void Start()
    {
        if(farmingTime >=0.5f)
        InvokeRepeating("FarmTime", 0, 1f);

    }

    private void Update()
    {
        intervalMin += Time.deltaTime*0.5f;
        intervalMax = player.GetComponent<Character>().currentHealth + intervalMin;

        if (waitTime>=0)
            waitTime-=Time.deltaTime;
        else if(waitTime < 0&&player.GetComponent<Character>().currentHealth!=0)
            FarmEneny();
    }

    void FarmTime()
    {
        farmingTime -= 0.01f;

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
        if (Vector3.Distance(enemyPos, transform.position) >= farmingRange-0.5f)
        {
            float randomCount = Random.Range(intervalMin, intervalMax);
            for (int i = 0; i < intervalCuttingPoint.Count; i++)
                if (randomCount >= intervalMin && randomCount <= intervalCuttingPoint[i]*intervalMax)
                {
                    GameObject enemy  =Instantiate(enemys[i], enemyPos, Quaternion.identity);
                    SceneManager.MoveGameObjectToScene(enemy, SceneManager.GetSceneByName("GameScene"));
                    waitTime = farmingTime;
                    return;
                }
        }
    }
}
