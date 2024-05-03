using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif
public class LoadManager : MonoBehaviour
{
    public static LoadManager instance;

    [Header("事件广播")]
    public FadeEventSO fadeScene;
    public VoidEventSO afterLoadEvent;
    public SetGoalTimeSO setBGMIndex;

    [Header("事件监听")]
    public SceneLoadEventSO sceneLoadEvent;

    private int sceneToLoadIndex;
    private Vector3 positionToGO;
    private bool isScreenFaded;

    [Header("基本参数")]
    public int firstSceneIndex;
    public int currentSceneIndex;
    public bool isLoading;
    public Transform playerTans;
    public GameObject playerUI;
    public GameObject playerCamera;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void OnEnable()
    {
        sceneLoadEvent.sceneLoad += SceneLoadEvent;
    }

    private void OnDisable()
    {
        sceneLoadEvent.sceneLoad -= SceneLoadEvent;
    }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if (isLoading)
            AudioManager.instance.StopAllBGM();
        else
            AudioManager.instance.PlayAllBGM();
    }

    private void NewGame()
    {
        sceneToLoadIndex = firstSceneIndex;
        SceneLoadEvent(sceneToLoadIndex, Vector2.zero, false);
    }

    private void SceneLoadEvent(int locationToLoadIndex, Vector2 posToGO, bool screenFade)
    {
        if (isLoading)
            return;

        isLoading = true;
        sceneToLoadIndex = locationToLoadIndex;
        positionToGO = posToGO;
        isScreenFaded = screenFade;
        if (screenFade)
            fadeScene.isFade(screenFade);

        if (currentSceneIndex != -1)
            StartCoroutine(UnLoadPrevioisScene());

        else
            LoadNewScene();
    }

    IEnumerator UnLoadPrevioisScene()
    {
        if (isScreenFaded)
            yield return new WaitForSeconds(1f);

        yield return SceneManager.UnloadSceneAsync(currentSceneIndex);
        LoadNewScene();
    }

    private void LoadNewScene()
    {
        playerTans.gameObject.SetActive(false);
        playerUI.SetActive(false);
        playerCamera.SetActive(false);

        var loadingOption = SceneManager.LoadSceneAsync(sceneToLoadIndex, LoadSceneMode.Additive);

        loadingOption.completed += SceneLoadCompleted;

    }

    private void SceneLoadCompleted(AsyncOperation handle)
    {
        currentSceneIndex = sceneToLoadIndex;
        playerTans.position = positionToGO;
        isLoading = false;
        fadeScene.isFade(false);
        AudioManager.instance.gameObject.SetActive(true);


        if (currentSceneIndex == 2)
        {
            setBGMIndex.setGoalTime(0);
            playerTans.gameObject.SetActive(true);
            playerUI.SetActive(true);
            playerCamera.SetActive(true);
        }

        else if (currentSceneIndex == 1)
            setBGMIndex.setGoalTime(4);

        afterLoadEvent.EventRaised();
    }
}