using UnityEngine;

public class CameraMove : MonoBehaviour
{

    [SerializeField]
    float moveSpeed;

    [SerializeField]
    float yAxis;

    [SerializeField]        
    Destination destination;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (yAxis == 0){
            throw new System.Exception("yAxisが0です。");
        }
        if (moveSpeed == 0){
            throw new System.Exception("moveSpeedが0になっています。");
        }
        if (destination == null){
            throw new System.Exception("destinationがnullです。");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 行先を決定する。行先を管理するdestinaionクラスからxとzを取得してyはインスペクターで設定した値を使用。
        var newPosion = new Vector3(destination.Get.x, yAxis, destination.Get.z);
        // 移動する処理
        transform.position = Vector3.MoveTowards(transform.position, newPosion, moveSpeed * Time.deltaTime);
    }
}
