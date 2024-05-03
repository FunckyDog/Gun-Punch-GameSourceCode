using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScene : MonoBehaviour
{
    [Header("ÊÂ¼þ¼àÌý")]
    public FadeEventSO fadeScene;

    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        fadeScene.isFade += FadeAnimation;
    }

    private void OnDisable()
    {
        fadeScene.isFade -= FadeAnimation;
    }

    private void FadeAnimation(bool fade)
    {
        anim.SetBool("Fade", fade);
    }
}
