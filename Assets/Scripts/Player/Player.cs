using UnityEngine;
using Fusion;
using System.Linq;

// プレイヤーの状態を管理するクラス
public class Player : NetworkBehaviour
{
    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float yAxis;
    readonly int combinedNumber = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (moveSpeed == 0)
        {
            throw new System.Exception("moveSpeedが0です。");
        }
        if (yAxis == 0)
        {
            throw new System.Exception("yAxisが0です。");
        }
    }

    // fusionから移動先の位置情報を受け取って移動する
    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {
            // 移動する処理
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(data.Direction.x, yAxis, data.Direction.z), moveSpeed * Time.deltaTime);
            // 回転する処理
            // 目標への方向ベクトルを計算
            Vector3 dir = data.Direction - transform.position;
            // y軸方向には回転させたくないから0にしている
            dir.y = 0;
            if (dir.sqrMagnitude > 0.001f)
            {
                transform.rotation = Quaternion.LookRotation(dir);
            }
        }
    }

    void SizeUp()
    {
        transform.localScale *= 1.1f;
    }

    void OnTriggerEnter(Collider other)
    {
        // プレイヤーのフレンドの数が5人以上ならサイズアップ
        GameObject[] frends = GameObject.FindGameObjectsWithTag("Frend");
        if (CountFrend(frends) >= combinedNumber)
        {
            SizeUp();
            DespawnFrend(frends);
        }
    }

    // プレイヤーのフレンドの数を返す
    int CountFrend(GameObject[] frends)
    {
        GameObject[] playerHasFrends = frends.Where(f => f.GetComponent<Frend>().Target == this.transform).ToArray();
        return playerHasFrends.Length; ;
    }

    void DespawnFrend(GameObject[] frends)
    {
        GameObject[] playerHasFrends = frends.Where(f => f.GetComponent<Frend>().Target == this.transform).ToArray();
        foreach (var f in frends)
        {
            f.GetComponent<DespawnThis>().Despawn();
        }
    }

}
