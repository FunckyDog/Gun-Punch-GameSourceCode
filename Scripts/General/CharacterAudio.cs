using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class CharacterAudio : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip[] clips;
    public AudioMixerGroup FX;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.outputAudioMixerGroup = FX;
    }

    public void PlayAudioFX(int index)
    {
        audioSource.PlayOneShot(clips[index]);
    }
}
