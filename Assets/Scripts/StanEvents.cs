using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class StanEvents : MonoBehaviour
{
    [SerializeField]
    public UnityEvent onStanEvent;
    
    public void UnStanEvent()
    {
        onStanEvent.Invoke();
    }
}
