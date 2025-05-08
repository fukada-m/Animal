using UnityEngine;
using UniRx;

public class Move : MonoBehaviour
{
    [SerializeField]
    ClickEvent clickEvent;

    [SerializeField]
    float moveSpeed;

    MousePosition mousePosition = new MousePosition();

    Vector3 destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
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

    void SetDestination(){
        destination = mousePosition.GetMousePosition(transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, moveSpeed * Time.deltaTime);
    }
}
