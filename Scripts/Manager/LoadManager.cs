using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadManager : Singleton<LoadManager>
{
    [Header("基本参数")]
    public SceneData firstSceneData;
    public SceneData currentSceneData;
    public Animator sceneFadeAnim;
    public Transform playerTans;
    public GameObject playerCamera;

    private void Start()
    {
        StartCoroutine(UnLoadScene(firstSceneData));
    }

    public void SceneLoadAction(SceneData sceneData) => StartCoroutine(UnLoadScene(sceneData));

    IEnumerator UnLoadScene(SceneData sceneData)
    {
        yield return SceneFade(true);
        AudioManager.instance.StopAllBGM();

        playerTans.gameObject.SetActive(false);
        playerCamera.gameObject.SetActive(false);

        if (currentSceneData != null)
            yield return SceneManager.UnloadSceneAsync(currentSceneData.sceneIndex);

        StartCoroutine(LoadNewScene(sceneData));
    }

    IEnumerator LoadNewScene(SceneData sceneData)
    {

        yield return SceneManager.LoadSceneAsync(sceneData.sceneIndex, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneData.sceneIndex));

        currentSceneData = sceneData;

        if (currentSceneData.sceneIndex == 2)
        {
            playerTans.gameObject.SetActive(true);
            playerCamera.gameObject.SetActive(true);
            playerTans.position = Vector3.zero;
        }


        EventsHandler.CallAfterSceneLoadEvent();
        AudioManager.instance.PlayBGM(0);

        yield return SceneFade(false);

    }

    IEnumerator SceneFade(bool isFade)
    {
        sceneFadeAnim.SetBool("Fade", isFade);
        yield return new WaitForSeconds(2);
    }
}