using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    public List<AudioSource> sources = new List<AudioSource>();
    public AudioMixerGroup BGM;
    private void OnEnable()
    {
        EventsHandler.AfterSceneLoadEvent += SetAudioScource;
    }

    private void OnDisable()
    {
        EventsHandler.AfterSceneLoadEvent -= SetAudioScource;
    }

    private void Update()
    {
        if (sources.Count != 0)
            Loop();
    }

    void SetAudioScource()
    {
        foreach (AudioSource source in sources)
            Destroy(source);
        sources.Clear();
        for (int i = 0; i < LoadManager.instance.currentSceneData?.audioClips.Length; i++)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.loop = true;
            audioSource.outputAudioMixerGroup = BGM;
            audioSource.clip = LoadManager.instance.currentSceneData?.audioClips[i];
            audioSource.volume = 0;
            sources.Add(audioSource);
        }
    }


    public void StopAllBGM()
    {
        foreach (AudioSource source in sources)
        {
            if (source.volume > 0f)
                source.volume -= 0.02f;
            else
                source.Stop();
        }
    }

    public void PlayBGM(int index)
    {
        sources[index].volume = 0f;
        sources[index].Play();

        if (index != 0)
            sources[index].time = sources[index - 1].time;


        while (sources[index].volume < 1)
            sources[index].volume += 0.02f;

    }

    void Loop()
    {
        if (sources[0].time >= LoadManager.instance.currentSceneData.clipTime)
        {
            sources[0].time = 0;
            for (int i = 1; i < sources.Count; i++)
                PlayBGM(i);
        }
    }
}
