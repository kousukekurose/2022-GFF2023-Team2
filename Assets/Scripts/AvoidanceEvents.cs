using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class AvoidanceEvents : MonoBehaviour
{
    [SerializeField]
    MoveBehaviour moveBehaviour;
    [SerializeField]
    public UnityEvent onAvoidanceEvent;
    bool ondash = false;
    public void AvoidanceEvent()
    {
        moveBehaviour.walk();
        moveBehaviour.StateAvoidanceToLocomotion();
        onAvoidanceEvent.Invoke();
        ondash = false;
    }
    public void AcEvent()
    {
        ondash = true;
    }
    private void Update()
    {
        if(ondash == true)
        {
            moveBehaviour.dash();
        }
    }
}
