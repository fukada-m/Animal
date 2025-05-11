using UnityEngine;

public class MousePosition
{
    public Vector3 Destination { get; set;}

    // クリック位置の座標を返す。レイが何も当たらなかったときは引数でもらった現在の座標をそのまま返す。
    public Vector3 GetMousePosition(Transform currentTransform){
         // 1) 画面上のクリック位置からレイを飛ばす。レイキャストの最大距離は100f
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 2) レイキャストして当たったポイントを取得
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
        {
            Vector3 hitPoint = hitInfo.point;
            Debug.Log($"クリック（3D）したワールド座標: {hitPoint}");
            Debug.Log($"当たったオブジェクト: {hitInfo.collider.name}");
            Destination = hitPoint;
            return hitPoint;
        }
        else
        {
            Debug.Log("何にも当たりませんでした");
            // 現在の座標
            Destination = currentTransform.position;
            return currentTransform.position;
        }
    }
}
