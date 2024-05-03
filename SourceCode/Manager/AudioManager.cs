using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public List<AudioSource> sources = new List<AudioSource>();

    [Header("ÊÂ¼þ¼àÌý")]
    public SetGoalTimeSO setBGMIndex;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        setBGMIndex.setGoalTime += PlayBGMByindex;
    }

    private void OnDisable()
    {
        setBGMIndex.setGoalTime -= PlayBGMByindex;
    }

    void PlayBGMByindex(int index)
    {
        sources[index].Play();
    }

    public void StopAllBGM()
    {
        foreach (AudioSource source in sources)
        {
            if (source.volume > 0f)
                source.volume -= 0.02f;
            else
                gameObject.SetActive(false);
        }
    }

    public void PlayAllBGM()
    {
        foreach (AudioSource source in sources)
        {
            if (source.volume <=1)
                source.volume += 0.02f;
        }
    }
}
