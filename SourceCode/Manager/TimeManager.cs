using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour
{
    public static TimeManager instance;

    public List<Sprite> timeNumber = new List<Sprite>();
    public List<Image> timeImage = new List<Image>();
    public int totalTime;
    public int goalTime;


    [Header("事件广播")]
    public VoidEventSO gameFinished;
    public SetGoalTimeSO setRecordTime;

    [Header("事件监听")]
    public SetGoalTimeSO getGoalTime;
    public VoidEventSO gameOver;
    public VoidEventSO afterLoadEvent;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        getGoalTime.setGoalTime += GetGoalTime;
        gameOver.onEventRiased += SetRecordTime;

        afterLoadEvent.onEventRiased += StartCount;
    }

    private void OnDisable()
    {
        getGoalTime.setGoalTime -= GetGoalTime;
        gameOver.onEventRiased -= SetRecordTime;
        afterLoadEvent.onEventRiased -= StartCount;
    }

    void StartCount()
    {
        if (LoadManager.instance.currentSceneIndex == 2)
        {
            totalTime = 0;
            InvokeRepeating("CountTime", 0, 1);
        }

    }

    private void SetRecordTime()
    {
        setRecordTime.GoalTime(totalTime);
    }

    private void GetGoalTime(int time)
    {
        goalTime = time;
    }

    void CountTime()
    {
        totalTime++;
        int minutes = totalTime / 60;
        int seconds = totalTime % 60;
        if (totalTime > goalTime - 10)
        {
            foreach (Image image in timeImage)
                image.color = Color.red;
        }

        timeImage[0].sprite = timeNumber[minutes / 10];
        timeImage[1].sprite = timeNumber[minutes % 10];
        timeImage[2].sprite = timeNumber[seconds / 10];
        timeImage[3].sprite = timeNumber[seconds % 10];
        if (totalTime == goalTime)
        {
            CancelInvoke("CountTime");
            gameFinished.EventRaised();
        }

    }

}
