using System.Collections;
using UnityEngine;

public class SpawnFrend : MonoBehaviour
{
    [SerializeField]
    float spawnInterval;
    GameObject frendPrefab;
    readonly Vector3 minSpawnPos = new Vector3(-13f, 1f, -13f);
    readonly Vector3 maxSpawnPos = new Vector3(13f, 1f, 13f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        frendPrefab = Resources.Load<GameObject>("Prefabs/Frend");
        if(frendPrefab == null) Debug.LogError("Frend prefabが見つかりません。");
        StartCoroutine(SpawnRooutine());
    }

    Vector3 RandomPos(Vector3 minSpawnPos, Vector3 maxSpawnPos){
        float x = Random.Range(minSpawnPos.x, maxSpawnPos.x);
        float y = Random.Range(minSpawnPos.y, maxSpawnPos.y);
        float z = Random.Range(minSpawnPos.z, maxSpawnPos.z);
        return new Vector3(x, y, z);
    }

    // spawnIntervalおきにランダムな場所にフレンドをスポーンさせる
    IEnumerator SpawnRooutine(){
        while(true) {
            var spawnPos = RandomPos(minSpawnPos, maxSpawnPos);
            Instantiate(frendPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }

    }
}
