using UnityEngine;
using System;
using UniRx;

public class ClickEvent : MonoBehaviour
{
    Subject<Unit> onClick = new Subject<Unit>();
    public IObservable<Unit> OnClick => onClick;

    void Update(){
        if (Input.GetMouseButtonDown(0)){
            onClick.OnNext(Unit.Default);
        }
    }
}
