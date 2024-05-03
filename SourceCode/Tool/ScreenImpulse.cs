using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenImpulse : MonoBehaviour
{
    public static ScreenImpulse Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        Impulse();
    }

    public void Impulse()
    {
        GetComponent<CinemachineImpulseSource>().GenerateImpulse();
    }

    public void Impulse(Vector3 direction, float strength)
    {
        GetComponent<CinemachineImpulseSource>().GenerateImpulseWithVelocity(direction);
        GetComponent<CinemachineImpulseSource>().GenerateImpulseWithForce(strength);
    }
}
