using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
public class Guide : MonoBehaviour
{
    public static Guide Instance;

    Action arrivedCallback;
    Vector3 destination;
    public float moveSpeed;
    bool moving;
    public Animator animator;
    public Transform rootTr;

    private void Awake()
    {
        Instance = this;
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        targetDirection = rootTr.forward;
    }

    public void MoveTo(Vector3 d, Action aCallback)
    {
        moving = true;
        arrivedCallback = aCallback;
        destination = d;
        animator.Play("Walking");
    }

    Vector3 targetDirection;
    public void LookAt(Vector3 dir)
    {
        targetDirection = dir;

        float angle = Vector3.Angle(rootTr.forward, dir);
        Debug.Log("angle :" + angle);

        if (angle > 0) 
        {
            animator.Play("LeftTurn");
        }
        if (angle < 0)
        {
            animator.Play("RightTurn");
        }
        Debug.Log("Guide LookAt()");
    }

    void Update()
    {
        if (moving)
        {
            float distance = Vector3.Distance(transform.position, destination);
            if (distance <= 0.01f)
            {
                moving = false;
                //도착한 상태!
                arrivedCallback?.Invoke();
                animator.Play("Idle");
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, destination, Time.deltaTime * moveSpeed);
            Vector3 vec = (destination - transform.position);
            //목적지를 바라보기
            Vector3 dir = vec.normalized;
            dir.y = 0;

            rootTr.rotation = Quaternion.Slerp(rootTr.rotation,
                                        Quaternion.LookRotation(dir), Time.deltaTime * 1f);

        }
        else
        {
            if (targetDirection.Equals(rootTr.forward))
                return;
            //플레이어 바라보기
            Vector3 dir = targetDirection;
            dir.y = 0;

            // 회전방향, 속도
            rootTr.rotation = Quaternion.Slerp(rootTr.rotation,
                                        Quaternion.LookRotation(dir), Time.deltaTime * 1f);

        }

    }



}
