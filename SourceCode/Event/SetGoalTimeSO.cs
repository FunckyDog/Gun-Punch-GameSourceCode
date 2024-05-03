using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/SetGoalTime")]
public class SetGoalTimeSO : ScriptableObject
{
    public UnityAction<int> setGoalTime;

    public void GoalTime(int time)
    {
        setGoalTime?.Invoke(time);
    }
}
