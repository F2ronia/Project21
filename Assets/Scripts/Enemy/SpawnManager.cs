using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;

public class SpawnManager : MonoBehaviour
{
    #region Singleton
    public static SpawnManager Instance { get; private set; }
    void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (Instance != this && Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    public GameObject enemyPrefab;

    public Mesh[] EnemyMesh;
    public Material[] EnemyMaterial;

    public Vector3 playerPos = Vector3.zero;
    public Quaternion playerRot;

    private Transform spawnPoint;
    private GameObject obstacle;

    private bool[] isSpawnNum = new bool[9];
    private bool[] isObstacles = new bool[4];

    bool isGameStart = false;

    private void Start()
    {
        DataSetting();
        StageSetting();
    }

    private void DataSetting()
    {
        for (int i = 0; i < 4; i++) 
        {
            int randomNum = UnityEngine.Random.Range(0, 2);
            if (randomNum == 0)
                isObstacles[i] = true;
            else
                isObstacles[i] = false;
        }
        for (int i = 0; i < 9; i++)
        {
            int randomNum = UnityEngine.Random.Range(0, 2);
            switch (i)
            {
                case 2:
                    if (randomNum == 0 && !isObstacles[0])
                        isSpawnNum[i] = true;
                    else
                        isSpawnNum[i] = false;
                    break;
                case 3:
                    if (randomNum == 0 && !isObstacles[1])
                        isSpawnNum[i] = true;
                    else
                        isSpawnNum[i] = false;
                    break;
                case 5:
                    if (randomNum == 0 && !isObstacles[2])
                        isSpawnNum[i] = true;
                    else
                        isSpawnNum[i] = false;
                    break;
                case 6:
                    if (randomNum == 0 && !isObstacles[3])
                        isSpawnNum[i] = true;
                    else
                        isSpawnNum[i] = false;
                    break;
                default:
                    isSpawnNum[i] = true;
                    break;
            }
        }
        isGameStart = true;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Build_Stage")
        {
            StageSetting();
            if (isSpawnNum[8] == false && isGameStart)
            {
                GameObject.Find("MainUI").transform.GetChild(1).gameObject.SetActive(true);
                isGameStart = false;
            }
        }
    }
    void StageSetting()
    {
        spawnPoint = GameObject.Find("SpawnPoint").transform;
        obstacle = GameObject.Find("Obstacles");

        for (int i = 0; i < 4; i++)
            if (isObstacles[i])
                obstacle.transform.GetChild(i).gameObject.SetActive(true);
        for (int i = 0; i < 9; i++)
        {
            if (isSpawnNum[i])
                EnemySpawn(i);
        }
    }

    void EnemySpawn(int _i)
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.GetChild(_i).position, spawnPoint.GetChild(_i).rotation);
        switch (_i)
        {
            case 0:
                enemy.GetComponent<EnemyMove>().SetUp(1, EnemyMesh[0], EnemyMaterial[0]);
                break;
            case 1: case 2: case 3:
                enemy.GetComponent<EnemyMove>().SetUp(2, EnemyMesh[0], EnemyMaterial[0]);
                break;
            case 4: case 5: case 6:
                enemy.GetComponent<EnemyMove>().SetUp(3, EnemyMesh[0], EnemyMaterial[0]);
                break;
            case 7:
                enemy.GetComponent<EnemyMove>().SetUp(4, EnemyMesh[1], EnemyMaterial[1]);
                break;
            case 8:
                enemy.GetComponent<EnemyMove>().SetUp(5, EnemyMesh[2], EnemyMaterial[2]);
                break;
        }
    }

    public void EnemyEnter(int _stageNum, Vector3 _playerPos, Quaternion _playerRot)
    {
        playerPos = _playerPos;
        playerRot = _playerRot;
        switch (_stageNum)
        {
            case 1:
                isSpawnNum[0] = false;
                break;
            case 2:
                isSpawnNum[1] = false;
                isSpawnNum[2] = false;
                isSpawnNum[3] = false;
                break;
            case 3:
                isSpawnNum[4] = false;
                isSpawnNum[5] = false;
                isSpawnNum[6] = false;
                break;
            case 4:
                isSpawnNum[7] = false;
                break;
            case 5:
                isSpawnNum[8] = false;
                break;
        }

    }
}
