using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void SeneLoad(SceneData sceneData) => LoadManager.instance.SceneLoadAction(sceneData);

    public void SetTime(int time) => TimeManager.instance.SetGoalTime(time);

    public void EnterMode(bool enterMode) => GameManager.instance.IsEnterMode(enterMode);

    public void Quit() => Application.Quit();
}
