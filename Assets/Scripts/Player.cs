using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody rb;
    private bool isJump = false;
    private int nowStageNum = 2;

    [SerializeField]
    private GameObject[] stage;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField] private float jumpPower = 200;
    [SerializeField] private float a = 200;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //�ړ�
        Move();

        //�W�����v
        Jump();
    }

    //�W�����v
    private void Jump()
    {
        //�W�����v���Ă���Ȃ炱�̐揈�����Ȃ�
        if (isJump) return;

        //�ʏ�
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector3.up * jumpPower);
            isJump = true;
        }

        int beforeStage = nowStageNum;

        //�����W�����v(�ʂ̑����)
        if (Input.GetAxis("L_Stick_V2") > 0.8f)
        {
            nowStageNum++;
            nowStageNum = Math.Min(nowStageNum, stage.Length - 1);
            if (beforeStage == nowStageNum) return;
            transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f);
            rb.AddForce(Vector3.up * a);
            isJump = true;
        }
        else if (Input.GetAxis("L_Stick_V2") < -0.8f)
        {
            nowStageNum--;
            nowStageNum = Math.Max(nowStageNum, 0);
            if (beforeStage == nowStageNum) return;
            transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f);
            rb.AddForce(Vector3.up * a);
            isJump = true;
        }

    }

    //�ړ�
    private void Move()
    {
        //�W�����v���Ă���Ȃ炱�̐揈�����Ȃ�
        if (isJump) return;

        transform.position += new Vector3(-Input.GetAxis("L_Stick_H2") * speed * Time.deltaTime, 0, 0);
    }
        
    void OnCollisionEnter(Collision collision)  
    {
        if (collision.transform.tag == "Stage")
            isJump = false;
    }
}
