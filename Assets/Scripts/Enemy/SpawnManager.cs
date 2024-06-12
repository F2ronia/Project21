using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] obstacles;
    public GameObject EnemyPrefab;

    private List<GameObject> enemys = new List<GameObject>();

    private void Start()
    {
        MapSetting();
    }

    private void MapSetting()
    {
        foreach (GameObject obstacle in obstacles)
        {
            int random = UnityEngine.Random.Range(0, 2);
            if (random == 0)
                obstacle.SetActive(true);
        }
        for (int i = 0; i < 5; i++)
        {
            if (spawnPoints[i].transform.childCount == 0)
                EnemySpawn(spawnPoints[i].transform ,i);
            else
                EnemySpawn(spawnPoints[i].transform.GetChild(0), i);
        }
        foreach (GameObject obstacle in obstacles)
        {
            if (obstacle.active == false)
            {
                int random = UnityEngine.Random.Range(0, 2);
                switch (obstacle.name)
                {
                    case "A":
                        if (random == 0)
                            EnemySpawn(spawnPoints[1].transform.GetChild(1), 1);
                        break;
                    case "B":
                        if (random == 0)
                            EnemySpawn(spawnPoints[1].transform.GetChild(2), 1);
                        break;
                    case "C":
                        if (random == 0)
                            EnemySpawn(spawnPoints[2].transform.GetChild(1), 2);
                        break;
                    case "D":
                        if (random == 0)
                            EnemySpawn(spawnPoints[2].transform.GetChild(2), 2);
                        break;
                }
            }
        }
    }

    private void EnemySpawn(Transform _tl ,int _i)
    {
        GameObject enemy = Instantiate(EnemyPrefab, _tl.position, _tl.rotation);
        enemy.GetComponent<EnemyMove>().SetUp(_i + 1);
        enemy.GetComponent<EnemyMove>().spawnManager = gameObject.GetComponent<SpawnManager>();

        enemys.Add(enemy);
    }

    public void EnemyOff(int _num)
    {
        for (int i = enemys.Count - 1;i >= 0; i--)
        {
            if (enemys[i].GetComponent<EnemyMove>().stageNum == _num)
                enemys[i].SetActive(false);
        }
    }
}
