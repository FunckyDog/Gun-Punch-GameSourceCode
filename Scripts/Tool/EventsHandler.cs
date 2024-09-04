using System;

public static class EventsHandler
{
    public static event Action AfterSceneLoadEvent;
    public static void CallAfterSceneLoadEvent()
    {
        AfterSceneLoadEvent?.Invoke();
    }

    public static event Action GameFinish;
    public static void CallGameFinish()
    {
        GameFinish?.Invoke();
    }

    public static event Action GameLose;
    public static void CallGameLose()
    {
        GameLose?.Invoke();
    }

    public static event Action ResetGame;
    public static void CallResetGame()
    {
        ResetGame?.Invoke();
    }
}
