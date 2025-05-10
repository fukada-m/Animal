using UnityEngine;
using UniRx;
using Fusion;

public class Move : NetworkBehaviour
{
    ClickEvent clickEvent;

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float yAxis;

    MousePosition mousePosition = new MousePosition();

    Vector3 destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        clickEvent = GameObject.Find("Event").GetComponent<ClickEvent>();
        if (clickEvent == null){
            throw new System.Exception("ClickEventが指定されていません。");
        }
        if (moveSpeed == 0){
            throw new System.Exception("moveSpeedが0になっています。");
        }

        clickEvent.OnClick.Subscribe(_ => SetDestination()).AddTo(this);
        // 初期位置を指定
        destination = transform.position;
    }

    // マウスポジションを移動先に指定
    void SetDestination(){
        destination = mousePosition.GetMousePosition(transform);
        // 高さは常にyAxisの値を利用する
        destination = new Vector3(destination.x, yAxis, destination.z);
    }

    // Update is called once per frame
    public override void FixedUpdateNetwork()
    {
        // 移動する処理
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
    }
}