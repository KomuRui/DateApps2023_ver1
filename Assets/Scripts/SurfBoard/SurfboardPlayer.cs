using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEditor.PlayerSettings;

public class SurfboardPlayer : MonoBehaviour
{
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }

    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;

    public Animator animator;
    public int damType;


    private Material faceMaterial;

    [SerializeField] private int playerNum;                   // プレイヤー番号
    [SerializeField] public float LIMIT_ROTATE = 25.0f;      //回転の限界
    [SerializeField] private float LIFE_LIMIT = 10.0f;        //HPの最大量
    [SerializeField] private float HP_ANGLE = 10.0f;          //HPが減るようになる角度
    [SerializeField] private float HP_REDUCTIONE = 1.0f;     //HPの減少量
    [SerializeField] private float HP_INCREASE = 1.0f;       //HPの増加量
    [SerializeField] private float rotateSpeed = 0.5f;                   //傾く速さ

    private Rigidbody rb;
    private Vector3 pos;
    private Vector3 startRotate;    //初期の向き
    Quaternion rot;

    [SerializeField] public float rotationSpeed = 0.5f; //一秒間に傾く角度
    public float sumRotateX = 0.0f;
    public float sumRotateZ = 0.0f;

    private float hp = 10.0f;
    public bool isDead = false;
    


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rot = this.transform.rotation;

        startRotate = transform.localEulerAngles;

        InvokeRepeating(nameof(LifeControll), 1.0f, 0.1f);

        hp = 10.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && GameManager.nowMiniGameManager.IsStart() && !GameManager.nowMiniGameManager.IsFinish())
        {
            Move();
        }
    }

    //体力制御
    private  void LifeControll()
    {
        if (Math.Abs(sumRotateX) <= HP_ANGLE) 
        {
            //HPを回復
            hp += HP_INCREASE;

            //HPがMAXを超えないようにする
            hp = Mathf.Min(hp, LIFE_LIMIT);
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

       // Debug.Log(hp);
    }

    //落下処理
    private void Fall()
    {
        if (isDead) return;

        isDead = true;
         
         Debug.Log("a");
         Rigidbody childRb = this.transform.GetChild(0).GetComponent<Rigidbody>();
         
         //重力を使用する
         childRb.useGravity = true;
         
         GameManager.nowMiniGameManager.PlayerDead(this.transform.GetChild(0).GetComponent<PlayerNum>().playerNum);
         GameManager.nowMiniGameManager.PlayerFinish(this.transform.GetChild(0).GetComponent<PlayerNum>().playerNum);
        this.transform.GetChild(0).transform.parent = null;
    }

    //プレイヤーの動き

    public void Move()
    {
        // 入力を取得用
        float horizontalInput = 0;

        // 入力を取得
        horizontalInput = Input.GetAxis("L_Stick_H" + this.transform.GetChild(0).GetComponent<PlayerNum>().playerNum) * rotationSpeed * Time.deltaTime * 300;

        if (sumRotateX + horizontalInput <= LIMIT_ROTATE && sumRotateX + horizontalInput >= -LIMIT_ROTATE)
        {
            sumRotateX += horizontalInput;

            //スティックによって回転
            transform.Rotate(new Vector3(0, horizontalInput, 0));
        }

        //if (sumRotateZ + verticalInput <= LIMIT_ROTATE && sumRotateZ + verticalInput >= -LIMIT_ROTATE)
        //{
        //    sumRotateZ += verticalInput;

        //    //スティックによって回転
        //    transform.Rotate(new Vector3(verticalInput, 0, 0));
        //}
    }
}
