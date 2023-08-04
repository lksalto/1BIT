using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    [SerializeField] List<GameObject> enemyList;
    [SerializeField] List<Transform> spawnLocationList;
    [SerializeField] int waveCount = 5;
    [SerializeField] float spawnEnemy;
    [SerializeField] float spawnCooldown = 3;
    [SerializeField] float enemySpeed;
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnEnemy -= Time.deltaTime;
        if(spawnEnemy <= 0)
        {
            SpawnEnemy();
            spawnEnemy = spawnCooldown;
        }
    }

    void SpawnEnemy()
    {
        Transform posLoc;
        GameObject enemyPref;

        posLoc = spawnLocationList[Random.Range(0, spawnLocationList.Count-1)];
        enemyPref = enemyList[Random.Range(0, enemyList.Count - 1)];
        GameObject enemy;
        enemy = Instantiate(enemyPref, posLoc);
        if(enemy.transform.position.x > 0)
        {
            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(-enemySpeed, 0), ForceMode2D.Impulse);
        }
        else
        {
            enemy.GetComponent<Rigidbody2D>().AddForce(new Vector2(enemySpeed, 0), ForceMode2D.Impulse);
        }

    }
}
