using Unity.Mathematics;
using UnityEngine;
using UniRx;

public class OnClickMove : MonoBehaviour
{
    [SerializeField]
    ClickEvent clickEvent;

    [SerializeField]
    float moveSpeed;

    Vector3 movePosition;

    void Awake(){
        if (clickEvent == null){
            throw new System.Exception("ClickEventが指定されていません。");
        }
        clickEvent.OnClick.Subscribe(_ => SetMovePosition()).AddTo(this);
        if (moveSpeed == 0){
            throw new System.Exception("moveSpeedが0になっています。");
        }
        movePosition = transform.position;
        
    }

    // クリックされた座標を元にUpdate()内でターゲットを移動させている。
    void SetMovePosition(){
        movePosition = GetMousePosition();
    }

    // クリックした座標のVector3を返す。
    Vector3 GetMousePosition(){
        // 1) 画面上のクリック位置からレイを飛ばす。レイキャストの最大距離は100f
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 2) レイキャストして当たったポイントを取得
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
        {
            Vector3 hitPoint = hitInfo.point;
            Debug.Log($"クリック（3D）したワールド座標: {hitPoint}");
            Debug.Log($"当たったオブジェクト: {hitInfo.collider.name}");
            return hitPoint;
        }
        else
        {
            Debug.Log("何にも当たりませんでした");
            return transform.position;
        }
    }

    void Update(){
        Move();
    }

    // 実際にプレイヤーを移動させる処理。速度はmoveSpeedの値で変わる
    void Move(){
        // 目標への方向ベクトルを計算
        Vector3 dir = movePosition - transform.position;
        // y軸方向には回転させたくないから0にしている
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f && transform.CompareTag("Player")){
            transform.rotation = Quaternion.LookRotation(dir);
        }else if (dir.sqrMagnitude > 0.001f && transform.CompareTag("Player") == false){
            transform.position = new Vector3(transform.position.x, 20f, transform.position.z);
        }
        if (transform.position != movePosition) {
            transform.position = Vector3.MoveTowards(transform.position, movePosition, moveSpeed * Time.deltaTime);
        }
    }
}
