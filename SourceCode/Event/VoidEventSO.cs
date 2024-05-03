using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Event/VoidEvent")]
public class VoidEventSO : ScriptableObject
{
    public UnityAction onEventRiased;

    public void EventRaised()
    {
        onEventRiased?.Invoke();
    }
}
