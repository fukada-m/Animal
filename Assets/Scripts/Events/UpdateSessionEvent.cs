using UnityEngine;
using System;
using UniRx;

public class UpdateSessionEvent : MonoBehaviour
{
    Subject<Unit> updateSession = new Subject<Unit>();
    public IObservable<Unit> UpdateSession => updateSession;

    public void UpdateSeesion(){
        updateSession.OnNext(Unit.Default);
    }
}
