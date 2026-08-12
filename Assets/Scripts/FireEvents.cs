using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class FireEvents : MonoBehaviour
{
    [SerializeField]
    public UnityEvent onFireEvent;
    public void FireEvent()
    {
        onFireEvent.Invoke();
    }
}
