using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    bool isPlayerFind = false;

    private void FixedUpdate()
    {
        ViewRange();
    }
    void ViewRange()
    {
        Collider[] viewBox = Physics.OverlapBox(transform.position + new Vector3(0, 0, 3), new Vector3(2, 1, 3), transform.rotation);

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
                    // 플레이어 추적 시작!!
                }
            }
        }

        if (isPlayerIn && isPlayerFind == false)
            transform.GetChild(0).gameObject.SetActive(true);
        else
            transform.GetChild(0).gameObject.SetActive(false);

    }

    void FindPlayer()
    {

    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero + new Vector3(0, 0, 3), new Vector3(2, 1, 3) * 2);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("플레이어 접촉 확인 및 전투 진입");
        }
    }
}