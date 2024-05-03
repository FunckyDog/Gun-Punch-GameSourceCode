using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Event/GetObject")]
public class GetObjectSO : ScriptableObject
{
    public UnityAction<GameObject> gameObject;

    public void GetObject(GameObject obj)
    {
        gameObject?.Invoke(obj);
    }
}
