using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("ÊÂ¼þ¼àÌý")]
    public VoidEventSO afterLoadEvent;

    public CinemachineConfiner2D cc2d;

    private void OnEnable()
    {
        afterLoadEvent.onEventRiased += GetBound;
    }

    private void OnDisable()
    {
        afterLoadEvent.onEventRiased -= GetBound;
    }

    private void GetBound()
    {
        cc2d.m_BoundingShape2D = GameObject.FindGameObjectWithTag("Bound")?.GetComponent<PolygonCollider2D>();
    }

}
