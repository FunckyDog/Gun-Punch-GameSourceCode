using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Scene Data",menuName ="Data/Scene Data")]
public class SceneData : ScriptableObject
{
    public int sceneIndex;
    public AudioClip[] audioClips;
    public float clipTime;
}
