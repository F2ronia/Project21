using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMove : MonoBehaviour
{
    public SpawnManager spawnManager;
    public AudioClip search_sound;
    public AudioClip find_sound;
    private NavMeshAgent nvAgent;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private AudioSource audio;

    public int stageNum;

    bool isPlayerFind = false;

    private void Start()
    {
        nvAgent = GetComponent<NavMeshAgent>();
        audio = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        ViewRange();
    }

    public Vector3 boxSize = new Vector3(5, 2, 5);

    bool isFindSound = false;
    bool isSearchSound = false;
    void ViewRange()
    {
        Vector3 boxCentar = transform.position + transform.TransformDirection(Vector3.forward * 4);
        Collider[] viewBox = Physics.OverlapBox(boxCentar, boxSize, transform.rotation);

        bool isPlayerIn = false;

        foreach (Collider collider in viewBox)
        {
            if (collider.tag == "Player")
            {
                isPlayerIn = true;
                RaycastHit hit;
                bool currentIsHit = Physics.Raycast(transform.position, collider.transform.position - transform.position, out hit);
                Debug.DrawRay(transform.position, collider.transform.position - transform.position);

                if (currentIsHit && hit.collider.tag == "Player")
                {
                    isPlayerFind = true;
                    transform.GetChild(1).gameObject.SetActive(true);
                    if (!isFindSound)
                    {
                        audio.PlayOneShot(find_sound);
                        isFindSound = true;
                    }
                    nvAgent.destination = hit.transform.position;
                }
            }
        }

        if (isPlayerIn && isPlayerFind == false)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            if (!isSearchSound)
            {
                audio.PlayOneShot(search_sound);
                isSearchSound = true;
            }
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);
            isSearchSound = false;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero + (Vector3.forward * 4), boxSize * 2);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SpawnManager.Instance.EnemyEnter(stageNum, other.transform.position, other.transform.rotation);
            GameManager.Instance.LoadTriggerEnemy();
            GameManager.Instance.CallAnyScene("Build_Battle");
        }
    }

    public void SetUp(int _num, Mesh _mesh, Material _material)
    {
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();

        meshFilter.sharedMesh = _mesh;
        meshRenderer.material = _material;
        stageNum = _num;
    }
}