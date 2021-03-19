using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour
{
    public bool toggled { get; set; } = false;

    public UnityEvent OnClick, ToggledOn, ToggledOff;

    public void Click() => OnClick?.Invoke();
    
    public void Toggle()
    {
        if (toggled)
            ToggledOn?.Invoke();
        else
            ToggledOff?.Invoke();

        toggled = !toggled;
    }

    public void Straighten() => transform.rotation = Quaternion.identity;
}
