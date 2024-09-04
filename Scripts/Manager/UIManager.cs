using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [Header("基本参数")]
    public GameObject timeUI;
    public GameObject pauseUI;
    public GameObject winUI;
    public GameObject loseUI;
    public Text enemy_PistolCountText;
    public Text enemy_ShotGunCountText;
    public int enemy_PistolCount;
    public int enemy_ShotGunCount;
    public GameObject particlePrefab;
    public Camera playerCamera;

    private bool isPauseUIOpen = false, isWinUIOpen = false, isLoseUIOpen = false;

    private void OnEnable()
    {
        EventsHandler.AfterSceneLoadEvent += ResetUI;
        EventsHandler.AfterSceneLoadEvent += TimeUI;
        EventsHandler.GameFinish += WinUI;
        EventsHandler.GameLose += LoseUI;
    }

    private void OnDisable()
    {
        EventsHandler.AfterSceneLoadEvent -= TimeUI;
        EventsHandler.AfterSceneLoadEvent -= ResetUI;
        EventsHandler.GameFinish -= WinUI;
        EventsHandler.GameLose -= LoseUI;
    }

    private void Update()
    {

        if (LoadManager.instance.currentSceneData?.sceneIndex == 2 && (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
            PauseUI();

    }

    public void GetEnemyCount(GameObject enemy)
    {
        if (enemy.name == "Enemy_Pistol(Clone)")
            enemy_PistolCount++;
        if (enemy.name == "Enemy_ShotGun(Clone)")
            enemy_ShotGunCount++;
    }

    void TimeUI()
    {
        if (LoadManager.instance.currentSceneData?.sceneIndex == 2)
            timeUI.SetActive(true);
        else
            timeUI.SetActive(false);
    }

    public void PauseUI()
    {
        if (isWinUIOpen || isLoseUIOpen)
            return;

        isPauseUIOpen = !isPauseUIOpen;
        pauseUI.SetActive(isPauseUIOpen);
        Time.timeScale = isPauseUIOpen ? 0 : 1;
    }

    public void WinUI()
    {
        isWinUIOpen = !isWinUIOpen;
        winUI.SetActive(isWinUIOpen);
        winUI.transform.GetChild(5).gameObject.SetActive(GameManager.instance.isEnterMode);
        loseUI.transform.GetChild(4).GetComponent<Text>().text = string.Format("Your Record: {0:D2}:{1:D2}", TimeManager.instance.totalTime / 60, TimeManager.instance.totalTime % 60);
        Time.timeScale = isWinUIOpen ? 0 : 1;
        StartCoroutine(CountIncreasing(enemy_PistolCountText, enemy_PistolCount));
        StartCoroutine(CountIncreasing(enemy_ShotGunCountText, enemy_ShotGunCount));
    }

    public void LoseUI()
    {
        isLoseUIOpen = !isLoseUIOpen;
        loseUI.SetActive(isLoseUIOpen);
        Time.timeScale = isWinUIOpen ? 0 : 1;
        loseUI.transform.GetChild(4).GetComponent<Text>().text = string.Format("Your Record: {0:D2}:{1:D2}", TimeManager.instance.totalTime / 60, TimeManager.instance.totalTime % 60);
    }

    IEnumerator CountIncreasing(Text textArea, int toalCount)
    {
        int currentCount = 0;
        while (currentCount < toalCount)
        {
            currentCount++;
            textArea.text = currentCount.ToString();
            yield return new WaitForSecondsRealtime(0.1f);
        }
        ScreenImpulse.instance.Impulse(new Vector3(0, 0, 0.3f), 0.1f);

        Vector3 particlepos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(winUI.GetComponent<RectTransform>(), enemy_PistolCountText.rectTransform.anchoredPosition, playerCamera, out particlepos);
        particlepos.y = 0;
        Instantiate(particlePrefab, particlepos, Quaternion.identity);
    }

    public void BackToMainMenu() => LoadManager.instance.SceneLoadAction(LoadManager.instance.firstSceneData);//（按钮事件）

    public void ResetUI()
    {
        Time.timeScale = 1;
    }
}
