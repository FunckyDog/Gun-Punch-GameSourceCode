using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportPoint : MonoBehaviour
{
    [Header("ÊÂ¼þ¹ã²¥")]
    public SceneLoadEventSO sceneLoadEvent;

    public int sceneToLoadIndex;
    public Vector2 posToGO;

    public void TriggerAction()
    {
        if(sceneToLoadIndex==2)
        {
            posToGO.x = Random.Range(-20, 20);
            posToGO.y = Random.Range(-20, 20);
        }
        sceneLoadEvent.RaiseSceneLoad(sceneToLoadIndex, posToGO, true);
    }
}
