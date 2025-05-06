using UnityEngine;

public class ClickPosition3D : MonoBehaviour
{
    [SerializeField]
    ClickEvent _clickEvent;

    void Awake()
    {
        _clickEvent.OnClick += GetMousePosition;
    }

    public void GetMousePosition(){
        // 1) 画面上のクリック位置からレイを飛ばす
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // 2) レイキャストして当たったポイントを取得
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100f))
        {
            Vector3 hitPoint = hitInfo.point;
            Debug.Log($"クリック（3D）したワールド座標: {hitPoint}");
            Debug.Log($"当たったオブジェクト: {hitInfo.collider.name}");
            // return hitPoint;
        }
        else
        {
            Debug.Log("何にも当たりませんでした");
            // return new Vector3(0, 0, 0);
        }
    }
}
