using UnityEngine;
using Fusion;

// プレイヤーの状態を管理するクラス
public class Player : NetworkBehaviour
{
    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float yAxis;
    public GameObject LastFrend { get; set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (moveSpeed == 0){
            throw new System.Exception("moveSpeedが0です。");
        }
        if(yAxis == 0){
            throw new System.Exception("yAxisが0です。");
        }
        LastFrend = this.gameObject;
    }

    // fusionから移動先の位置情報を受け取って移動する
    public override void FixedUpdateNetwork() {
        if(GetInput(out NetworkInputData data))
        {
            // 移動する処理
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(data.Direction.x, yAxis, data.Direction.z), moveSpeed * Time.deltaTime);
            // 回転する処理
            // 目標への方向ベクトルを計算
            Vector3 dir = data.Direction - transform.position;
            // y軸方向には回転させたくないから0にしている
            dir.y = 0;
            if (dir.sqrMagnitude > 0.001f){
                transform.rotation = Quaternion.LookRotation(dir);
            }
        }
    }
}
