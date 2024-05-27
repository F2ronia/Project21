using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 destPos;
    private Vector3 dir;
    private Quaternion lookTarget;

    [SerializeField]
    private float speed = 5f;

    private bool isMove = false;

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            CheckRaycast();
        }
        if (isMove) {
            transform.position += dir.normalized * Time.deltaTime * speed;
            transform.rotation = Quaternion.Lerp(transform.rotation, lookTarget, 0.25f);
            isMove = (transform.position - destPos).magnitude > 0.05f;
        }
    }

    public void CheckRaycast() {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f)) {
            destPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            dir = destPos - transform.position;
            lookTarget = Quaternion.LookRotation(dir);
            isMove = true;
            UIManager.Inst.SelectStage(hit.transform.name);
        }
    }
}