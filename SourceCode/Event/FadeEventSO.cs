using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/FadeEvent")]
public class FadeEventSO : ScriptableObject
{
    public UnityAction<bool> isFade;

    public void SetFadeScreenBool(bool fade)
    {
        isFade?.Invoke(fade);
    }
}
