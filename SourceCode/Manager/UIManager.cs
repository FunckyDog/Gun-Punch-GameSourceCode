using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("基本参数")]
    public GameObject player;
    public GameObject pauseUI;
    public GameObject winUI;
    public GameObject loseUI;
    public Text enemy_PistolCountText;
    public Text enemy_ShotGunCountText;
    public Text recordTime;
    public int enemy_PistolCount;
    public int enemy_ShotGunCount;
    public GameObject particlePrefab;
    public Camera playerCamera;

    [Header("事件监听")]
    public VoidEventSO afterloadEvent;
    public GetObjectSO getEnemyCount;
    public VoidEventSO gameFinished;
    public SetGoalTimeSO gameOver;
    public VoidEventSO quitGame;

    bool isPauseUIOpen = false;

    private void OnEnable()
    {
        afterloadEvent.onEventRiased += ReCoverUI;
        getEnemyCount.gameObject += GetEnemyCount;
        gameFinished.onEventRiased += WinUI;
        gameOver.setGoalTime += LoseUI;
        quitGame.onEventRiased += Quit;
    }

    private void OnDisable()
    {
        afterloadEvent.onEventRiased -= ReCoverUI;
        getEnemyCount.gameObject -= GetEnemyCount;
        gameFinished.onEventRiased -= WinUI;
        gameOver.setGoalTime -= LoseUI;
        quitGame.onEventRiased-= Quit;
    }

    private void GetEnemyCount(GameObject enemy)
    {
        if (enemy.name == "Enemy_Pistol(Clone)")
            enemy_PistolCount++;
        if (enemy.name == "Enemy_ShotGun(Clone)")
            enemy_ShotGunCount++;
    }

    private void ReCoverUI()
    {
        isPauseUIOpen = false;
        winUI.SetActive(false);
        loseUI.SetActive(false);
    }

    private void Update()
    {

        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))&& player.activeSelf)
            PauseUI();
    }

    public void PauseUI()
    {

        isPauseUIOpen = !isPauseUIOpen;
        pauseUI.SetActive(isPauseUIOpen);
        if (isPauseUIOpen)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    void WinUI()
    {
        winUI.SetActive(true);
        StartCoroutine(CountIncreasing(enemy_PistolCountText, enemy_PistolCount));
        StartCoroutine(CountIncreasing(enemy_ShotGunCountText, enemy_ShotGunCount));
    }

    void LoseUI(int time)
    {
        loseUI.SetActive(true);
        recordTime.text = string.Format("Your Record: {0:D2}:{1:D2}", time / 60, time % 60);
    }

    IEnumerator CountIncreasing(Text textArea, int toalCount)
    {
        int currentCount = 0;
        while (currentCount <= toalCount)
        {
            currentCount++;
            textArea.text = currentCount.ToString();
            yield return new WaitForSeconds(0.1f);
        }
        ScreenImpulse.Instance.Impulse(new Vector3(0,0,0.3f),0.1f);
        Vector3 particlepos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(winUI.GetComponent<RectTransform>(), enemy_PistolCountText.rectTransform.anchoredPosition, playerCamera, out particlepos);
        particlepos.y = 0;
        Instantiate(particlePrefab,particlepos,Quaternion.identity);
    }

    private void Quit()=> Application.Quit();

}
