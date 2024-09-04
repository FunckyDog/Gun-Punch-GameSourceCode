using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : Singleton<TimeManager>
{

    public int totalTime;
    public int goalTime;
    public List<Sprite> timeNumber = new List<Sprite>();
    public List<Image> timeImage = new List<Image>();

    private void OnEnable()
    {
        EventsHandler.AfterSceneLoadEvent += StartCount;
    }

    private void OnDisable()
    {
        EventsHandler.AfterSceneLoadEvent -= StartCount;
    }

    public void SetGoalTime(int time) => goalTime = time;//设置目标时间（按钮事件）

    void StartCount()
    {
        if (LoadManager.instance.currentSceneData.sceneIndex == 2)
        {
            totalTime = 0;
            InvokeRepeating("CountTime", 0, 1);
        }
    }

    void CountTime()
    {
        totalTime++;
        int minutes = totalTime / 60;
        int seconds = totalTime % 60;
        if (totalTime > goalTime - 10 && goalTime > 0)
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
            EventsHandler.CallGameFinish();
        }

    }

    void StopCount() => CancelInvoke("StartCount");
}
