using UnityEngine;
using System;

public class ClickEvent : MonoBehaviour
{
    public event Action OnClick = delegate {};

    void Update(){
        if (Input.GetMouseButtonDown(0)){
            OnClick();
        }
    }
}
