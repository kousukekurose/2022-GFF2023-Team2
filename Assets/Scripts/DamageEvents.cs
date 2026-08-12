using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DamageEvents : MonoBehaviour
{
    [SerializeField]
    public UnityEvent onDamageEvent;
    public void DamageEvent()
    {
        onDamageEvent.Invoke();
    }
}
