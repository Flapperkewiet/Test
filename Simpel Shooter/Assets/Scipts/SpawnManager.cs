using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public GameObject SpawnPointHolder;
    public GameObject EnemyPrefab;
    static Vector3[] SpawnPoints;
    public int EnemyCount;
    public LayerMask SpawnLayerMask;
    static LayerMask mask;

    void Awake()
    {
        SpawnPoints = new Vector3[SpawnPointHolder.transform.childCount];
        for (int i = 0; i < SpawnPointHolder.transform.childCount; i++)
            SpawnPoints[i] = SpawnPointHolder.transform.GetChild(i).position;
        mask = SpawnLayerMask;
        for (int i = 0; i < EnemyCount; i++)
            Instantiate(EnemyPrefab, FindSpawnPoint(), Quaternion.identity);
    }

    static Vector3 FindSpawnPoint()
    {
        for (int i = 0; i < SpawnPoints.Length; i++)
            if (Physics.OverlapSphere(SpawnPoints[i], 1.5f, mask).Length == 0)
                return SpawnPoints[i];
        throw new System.NotImplementedException();
    }

    public static void Respawn(GameObject objectToRespawn)
    {
        objectToRespawn.transform.position = FindSpawnPoint();
    }
}
