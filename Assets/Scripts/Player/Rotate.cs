using UnityEngine;
using UniRx;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    ClickEvent clickEvent;
    MousePosition mousePosition = new MousePosition();

    Vector3 destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (clickEvent == null){
            throw new System.Exception("ClickEventが指定されていません。");
        }       
        clickEvent.OnClick.Subscribe(_ => SetDestination()).AddTo(this);
        // 初期位置を指定
        destination = transform.position;
    }
    void SetDestination(){
        destination = mousePosition.GetMousePosition(transform);
    }
    // Update is called once per frame
    void Update()
    {
        // 目標への方向ベクトルを計算
        Vector3 dir = destination - transform.position;
        // y軸方向には回転させたくないから0にしている
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f){
            transform.rotation = Quaternion.LookRotation(dir);
        }
    }
}
