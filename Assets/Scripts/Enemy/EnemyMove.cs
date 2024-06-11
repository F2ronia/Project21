using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyMove : MonoBehaviour
{
    private NavMeshAgent nvAgent;

    bool isPlayerFind = false;

    private void Start()
    {
        nvAgent = GetComponent<NavMeshAgent>();
    }

    private void FixedUpdate()
    {
        ViewRange();
    }

    public Vector3 boxSize = new Vector3(5, 2, 5);

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
                Physics.Raycast(transform.position, collider.transform.position - transform.position, out hit);
                Debug.DrawRay(transform.position, collider.transform.position - transform.position);

                if (hit.collider.tag == "Player")
                {
                    isPlayerFind = true;
                    transform.GetChild(1).gameObject.SetActive(true);
                    nvAgent.destination = hit.transform.position;
                }
            }
        }

        if (isPlayerIn && isPlayerFind == false)
            transform.GetChild(0).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(false);
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
            SceneManager.LoadScene("temp_battle");
            gameObject.SetActive(false);
            GameManager.Instance.LoadTriggerEnemy();
        }
    }
}