using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class SurfboardPlayer : MonoBehaviour
{
    [SerializeField] private int playerNum;                   // プレイヤー番号
    [SerializeField] private float LIMIT_ROTATE = 25.0f;      //回転の限界
    [SerializeField] private float LIFE_LIMIT = 10.0f;        //HPの最大量
    [SerializeField] private float HP_ANGLE = 10.0f;          //HPが減るようになる角度
    [SerializeField] private float HP_REDUCTIONE = 1.0f;     //HPの減少量
    [SerializeField] private float HP_INCREASE = 1.0f;       //HPの増加量

    private Rigidbody rb;
    private Vector3 pos;
    private Vector3 startRotate;    //初期の向き
    Quaternion rot;

    public float rotationSpeed = 10.0f;
    private float sumRotateX = 0.0f;
    private float sumRotateY = 0.0f;

    private float hp = 10.0f;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rot = this.transform.rotation;

        startRotate = transform.localEulerAngles;

        InvokeRepeating(nameof(LifeControll), 1.0f, 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        // 入力を取得用
        float horizontalInput = 0;
        float verticalInput = 0;

        // 入力を取得
        horizontalInput = Input.GetAxis("L_Stick_H" + playerNum);
        verticalInput = -Input.GetAxis("L_Stick_V" + playerNum);

        if (sumRotateX + horizontalInput <= LIMIT_ROTATE && sumRotateX + horizontalInput >= -LIMIT_ROTATE)
        {
            sumRotateX += horizontalInput;

            //スティックによって回転
            transform.Rotate(new Vector3(0, horizontalInput, 0));
        }

        //if (sumRotateY + verticalInput <= LIMIT_ROTATE && sumRotateY + verticalInput >= -LIMIT_ROTATE)
        //{
        //    sumRotateY += verticalInput;

        //    //スティックによって回転
        //    transform.Rotate(new Vector3(verticalInput, 0, 0));
        //}
    }

    //体力制御
    private  void LifeControll()
    {
        if (Math.Abs(sumRotateX) <= HP_ANGLE) 
        {
            //HPを回復
            hp += HP_INCREASE;

            //HPがMAXを超えないようにする
            Mathf.Min(sumRotateX, LIFE_LIMIT);
        }
        else
        {
            //HPを減少
            hp -= HP_REDUCTIONE;
        }

        //HPがなくなったら
        if (hp <= 0)
        {
            Fall();
        }

        Debug.Log(hp);
    }

    //落下処理
    private void Fall()
    {
        Rigidbody childRb = this.transform.GetChild(0).GetComponent<Rigidbody>();
        childRb.useGravity = true;

        this.transform.GetChild(0).transform.parent = null;
    }

    
}
