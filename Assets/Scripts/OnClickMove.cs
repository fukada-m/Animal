using Unity.Mathematics;
using UnityEngine;

public class OnClickMove : MonoBehaviour
{
    [SerializeField]
    ClickEvent clickEvent;

    [SerializeField]
    [Tooltip("移動させたいオブジェクト")]
    Transform target;

    [SerializeField]
    float moveSpeed;

    Vector3 movePosition;

    void Awake(){
        if (clickEvent == null){
            throw new System.Exception("ClickEventが指定されていません。");
        }
        clickEvent.OnClick += SetMovePosition ;
        if (target == null) {
            throw new System.Exception("targetが指定されていません。");
        }
        if (moveSpeed == 0){
            throw new System.Exception("moveSpeedが0になっています。");
        }
        movePosition = target.position;
        
    }

    // クリックされた座標を元にUpdate()内でターゲットを移動させている。
    public void SetMovePosition(){
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
            return target.position;
        }
    }

    void Update(){
        Move();
    }

    // 実際にプレイヤーを移動させる処理。速度はmoveSpeedの値で変わる
    void Move(){
        // 目標への方向ベクトルを計算
        Vector3 dir = movePosition - target.position;
        // y軸方向には回転させたくないから0にしている
        dir.y = 0;
        if (dir.sqrMagnitude > 0.001f && target.CompareTag("Player")){
            target.rotation = Quaternion.LookRotation(dir);
        }else if (dir.sqrMagnitude > 0.001f && target.CompareTag("Player") == false){
            target.position = new Vector3(target.position.x, 20f, target.position.z);
        }
        if (target.position != movePosition) {
            target.position = Vector3.MoveTowards(target.position, movePosition, moveSpeed * Time.deltaTime);
        }
    }
}
