using UnityEngine;

public class Eye : MonoBehaviour
{
    Enemy enemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemy = GetComponentInParent<Enemy>();
        if (enemy == null) throw new System.Exception("親にEnemyがアタッチされていません。");
    }

    // Frendを見つけたら追いかける
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Frend")
        {
            enemy.Destination = other.transform.position;
        }
    }
}
