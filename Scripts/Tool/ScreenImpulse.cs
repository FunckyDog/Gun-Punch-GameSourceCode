using Cinemachine;
using UnityEngine;

public class ScreenImpulse : Singleton<ScreenImpulse>
{
    private CinemachineImpulseSource cis;

    protected override void Awake()
    {
        base.Awake();
        cis = GetComponent<CinemachineImpulseSource>();
    }

    public void Impulse(Vector3 direction, float strength)
    {
        cis.GenerateImpulseWithVelocity(direction);
        cis.GenerateImpulseWithForce(strength);
    }
}
