using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 destPos;
    private Vector3 dir;
    private Quaternion lookTarget;
    private Animator playerAnimator;

    [SerializeField]
    private float speed = 5f;

    private bool isMove = false;

    private void Start()
    {
        playerAnimator = transform.GetChild(0).GetComponent<Animator>();
    }
    void Update()
    {
        playerAnimator.SetBool("isPlayerMove", isMove);
        if (Input.GetMouseButtonDown(0))
        {
            CheckRaycast();
        }
        if (isMove)
        {
            transform.position += dir.normalized * Time.deltaTime * speed;
            transform.DORotate(lookTarget.eulerAngles, 1);
            isMove = (transform.position - destPos).magnitude > 0.05f;
        }
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    public void CheckRaycast()
    {
        RaycastHit hit;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Physics.Raycast(ray, out hit, 100f, 7))
        {
            destPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            dir = destPos - transform.position;
            lookTarget = Quaternion.LookRotation(dir);
            isMove = true;
        }
    }
}