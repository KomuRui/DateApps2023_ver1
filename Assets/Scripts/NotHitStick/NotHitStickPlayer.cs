using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class NotHitStickPlayer : MonoBehaviour
{
    //アニメーションに必要
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }

    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;
    public Animator animator;
    public int damType;
    private Material faceMaterial;

    [SerializeField] private float moveSpeed = 5.0f;          // プレイヤーの移動速度
    [SerializeField] private float rotationSpeed = 180.0f;    // プレイヤーの回転速度
    [SerializeField] private float gravitySpeed = 0.05f;      // 重力速度
    [SerializeField] private bool isHorizontalInput = true;   // 横の入力許可するか
    [SerializeField] private bool isVerticalInput = true;     // 縦の入力許可するか
    [SerializeField] private bool isAnimIdle = true;          // 通常時のアニメーション許可するか
    [SerializeField] private bool isAnimWalk = true;          // 歩くアニメーション許可するか 
    [SerializeField] private bool isAnimJump = true;          // ジャンプアニメーション許可するか
    [SerializeField] private bool isAnimAttack = true;        // 攻撃アニメーション許可するか
    [SerializeField] private bool isAnimDamage = true;        // ダメージアニメーション許可するか
    [SerializeField] private GameObject[] stage;              // 床のゲームオブジェクト
    [SerializeField] private float jumpPower;                 // ジャンプ力
    [SerializeField] private int nowStageNum;                 //乗っている床の番号
    
    //リジットボディ
    private Rigidbody rb;

    //ジャンプしているか
    private bool isJump = false;

    // メインカメラのTransform
    private Transform mainCameraTransform;

    void Start()
    {
        //マテリアル設定
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        //リジットボディ取得
        rb = GetComponent<Rigidbody>();
    }

    //顔のテクスチャ設定
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //状態更新
        StateUpdata();

        //動き
        Move();

        //ジャンプ
        Jump();
    }

    //移動
    private void Move()
    {

        //ジャンプしているならこの先処理しない
        if (isJump) return;

        // 入力を取得用
        float horizontalInput = 0;
        float verticalInput = 0;

        // 入力を取得
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H1");
        if (isVerticalInput) verticalInput = Input.GetAxis("L_Stick_V1");

        //入力がないのなら
        if (horizontalInput == 0 && verticalInput == 0)
        {
            //通常状態に変更
            ChangeStateTo(SlimeAnimationState.Idle);
            return;
        }

        //歩き状態に変更
        ChangeStateTo(SlimeAnimationState.Walk);

        // カメラの向きを基準にプレイヤーを移動
        Vector3 forwardDirection = mainCameraTransform.forward;
        Vector3 rightDirection = mainCameraTransform.right;
        forwardDirection.y = 0f; // Y軸成分を0にすることで水平方向に制限

        // 移動方向を計算
        Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        // 移動
        transform.position += moveDirection * moveSpeed * Time.deltaTime;

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    //ジャンプ
    private void Jump()
    {
        //ジャンプしているならこの先処理しない
        if (isJump) return;

        //通常
        if (Input.GetButtonDown("Abutton1"))
        {
            //通常状態に変更
            ChangeStateTo(SlimeAnimationState.Idle);

            //上に力を加える
            rb.AddForce(Vector3.up * jumpPower);
            isJump = true;
            return;
        }

        int beforeStage = nowStageNum;

        //自動ジャンプ(別の足場に)
        if (Input.GetAxis("L_Stick_V1") > 0.8f)
        {
            nowStageNum--;
            nowStageNum = Math.Max(nowStageNum, 0);
            if (beforeStage == nowStageNum) return;
            transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f);
            ChangeStateTo(SlimeAnimationState.Idle);
            rb.AddForce(Vector3.up * jumpPower);
            isJump = true;
        }
        else if (Input.GetAxis("L_Stick_V1") < -0.8f)
        {
            nowStageNum++;
            nowStageNum = Math.Min(nowStageNum, stage.Length - 1);
            if (beforeStage == nowStageNum) return;
            transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f);
            ChangeStateTo(SlimeAnimationState.Idle);
            rb.AddForce(Vector3.up * jumpPower);
            isJump = true;
        }

        //ジャンプ状態に変更    
        //ChangeStateTo(SlimeAnimationState.Jump);
    }

    //状態更新
    private void StateUpdata()
    {
        switch (currentState)
        {
            case SlimeAnimationState.Idle:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || !isAnimIdle) return;

                currentState = SlimeAnimationState.Idle;
                animator.SetFloat("Speed", 0);
                SetFace(faces.Idleface);
                break;

            case SlimeAnimationState.Walk:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Walk") || !isAnimWalk) return;

                currentState = SlimeAnimationState.Walk;
                animator.SetFloat("Speed", 1.0f);
                SetFace(faces.WalkFace);
                break;

            case SlimeAnimationState.Jump:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Jump") || !isAnimJump) return;

                SetFace(faces.jumpFace);
                animator.SetTrigger("Jump");
                break;

            case SlimeAnimationState.Attack:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || !isAnimAttack) return;
                SetFace(faces.attackFace);
                animator.SetTrigger("Attack");
                break;

            case SlimeAnimationState.Damage:

                if (animator.GetCurrentAnimatorStateInfo(0).IsName("Damage0")
                 || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage1")
                 || animator.GetCurrentAnimatorStateInfo(0).IsName("Damage2")
                 || !isAnimDamage) return;

                animator.SetTrigger("Damage");
                animator.SetInteger("DamageType", damType);
                SetFace(faces.damageFace);
                break;
        }
    }

    //状態変更
    public void ChangeStateTo(SlimeAnimationState state)
    {
        if (this == null) return;
        if (state == this.currentState) return;

        this.currentState = state;
    }
        
    void OnCollisionEnter(Collision collision)  
    {
        if (collision.transform.tag == "Stage")
            isJump = false;
    }
}
