using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
	public GameObject enemy;
    public Transform[] spawnPoints;
    int randomSpawnPoint;
	public static bool spawnAllowed;
	public float spawnTime = 5f;
	private float myTime = 0.0f;
	private float nextSpawn = 0.5f;

    void Start () {
		spawnAllowed = true;
	}

    // Update is called once per frame
    void FixedUpdate()
    {
    	myTime = myTime + Time.deltaTime;

        if (myTime > nextSpawn)
        {
            nextSpawn = myTime + spawnTime;
            SpawnEnemy();
            nextSpawn -= myTime;
            myTime = 0.0F;
        }
        
    }

    void SpawnEnemy(){
        if (spawnAllowed) {
			randomSpawnPoint = Random.Range (0, spawnPoints.Length);
    	    Instantiate(enemy, spawnPoints [randomSpawnPoint].position, Quaternion.identity);
        }
    }
}
