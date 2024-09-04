using Cinemachine;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isEnterMode;

    public CinemachineConfiner2D cc2d;

    private void OnEnable()
    {
        EventsHandler.AfterSceneLoadEvent += GetBound;
    }

    private void OnDisable()
    {
        EventsHandler.AfterSceneLoadEvent -= GetBound;
    }

    private void GetBound()
    {
        if (LoadManager.instance.currentSceneData?.sceneIndex == 2)
            cc2d.m_BoundingShape2D = GameObject.FindGameObjectWithTag("Bound")?.GetComponent<PolygonCollider2D>();
    }

    public void IsEnterMode(bool enterMode) => isEnterMode = enterMode;

}
