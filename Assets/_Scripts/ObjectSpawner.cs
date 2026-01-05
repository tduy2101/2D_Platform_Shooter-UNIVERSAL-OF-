using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private Transform minPos;
    [SerializeField] private Transform maxPos;

    [SerializeField] private int waveNumber;
    [SerializeField] private List<Wave> waves;
    
    [System.Serializable]
    public class Wave
    {
        public ObjectPooler pool;
        public float spawnTimer;
        public float spawnInterval;
        public int objectsPerWave;
        public int spawnedObjectCount;
    }

    // Update is called once per frame
    void Update()
    {   
        waves[waveNumber].spawnTimer -= GameManager.Instance.adjustedWorldSpeed;
        if (waves[waveNumber].spawnTimer <= 0)
        {
            waves[waveNumber].spawnTimer += waves[waveNumber].spawnInterval;
            SpawnObject();
        }
        if (waves[waveNumber].spawnedObjectCount >= waves[waveNumber].objectsPerWave)
        {
            waves[waveNumber].spawnedObjectCount = 0;
            waveNumber++;
            if (waveNumber >= waves.Count)
            {
                waveNumber = 0;
            }
        }
    }

    private void SpawnObject()
    {
        GameObject spawnObj = waves[waveNumber].pool.GetPooledObject();
        spawnObj.transform.position = RandomSpawnPoint();
        //spawnObj.transform.rotation = transform.rotation;
        spawnObj.SetActive(true);   
        waves[waveNumber].spawnedObjectCount++;
    }

    private Vector2 RandomSpawnPoint()
    {
        Vector2 spawnPoint;

        spawnPoint.x = minPos.position.x;
        spawnPoint.y = Random.Range(minPos.position.y, maxPos.position.y);

        return spawnPoint;
    }

}
