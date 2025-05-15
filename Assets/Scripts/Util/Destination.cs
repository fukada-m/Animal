using UnityEngine;
using UniRx;

public class Destination : MonoBehaviour
{
    ClickEvent clickEvent;
    MousePosition mousePosition = new MousePosition();
    public Vector3 Get{ get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        clickEvent = GameObject.Find("Event").GetComponent<ClickEvent>();
        if (clickEvent == null){
            throw new System.Exception("ClickEventが指定されていません。");
        }

        clickEvent.OnClick.Subscribe(_ => SetDestination()).AddTo(this);
        // 初期位置はアタッチしたオブジェクトの座標
        Get = transform.position;
    }
        // マウスポジションを移動先に指定
    void SetDestination(){
        var destination = mousePosition.GetMousePosition(transform);
        Get = new Vector3(destination.x, destination.y, destination.z);
    }


}
