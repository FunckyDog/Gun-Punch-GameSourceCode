#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/SceneLoadEvent")]
public class SceneLoadEventSO : ScriptableObject
{
    public UnityAction<int, Vector2, bool> sceneLoad;

    public void RaiseSceneLoad(int sceneToGoIndex, Vector2 posToGO, bool isScreenFaded)
    {
        sceneLoad?.Invoke(sceneToGoIndex, posToGO,isScreenFaded);
    }
}
