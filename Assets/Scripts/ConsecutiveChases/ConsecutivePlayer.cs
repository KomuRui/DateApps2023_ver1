using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ConsecutivePlayer : MonoBehaviour
{
    //アニメーションに必要
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }

    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;

    public Animator animator;
    public int damType;

    [SerializeField] private Vector3 moveDirection = new Vector3(0.0f, 0.0f, -1.0f); //プレイヤーの移動方向

    private Material faceMaterial;

    [SerializeField] private float deceleration = 150.0f;       //減速率
    [SerializeField] private float addSpeed = 100.0f;          // ボタンを押したときプレイヤーの移動速度の上昇値
    [SerializeField] private float moveSpeed = 0.0f;           // プレイヤーの移動速度
    [SerializeField] private float rotationSpeed = 180.0f;      // プレイヤーの回転速度
    [SerializeField] private bool isHorizontalInput = true;     // 横の入力許可するか
    [SerializeField] private bool isVerticalInput = true;       // 縦の入力許可するか
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private float buttonCount = 0.0f;           // 入力を取得用
    [SerializeField] private bool isDead = false;                   // プレイヤー番号
    [SerializeField] private int playerNum;                   // プレイヤー番号
    

    private Transform mainCameraTransform; // メインカメラのTransform

    void Start()
    {
        //マテリアル設定
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        //プレイヤーの移動方向の正規化
        moveDirection.Normalize();
    }

    //顔のテクスチャ設定
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //動き
        Move();       

        //状態更新
        StateUpdata();
    }

    //移動
    private void Move()
    {
        // 入力を取得
        if (Input.GetButtonDown("Abutton" + playerNum))
        {
            buttonCount += addSpeed;
        }
            
        //速度が0ならば
        if (moveSpeed <= 0)
        {
            //通常状態に変更
            ChangeStateTo(SlimeAnimationState.Idle);
        }
        else
        {
            //通常状態に変更
            ChangeStateTo(SlimeAnimationState.Idle);

            //ジャンプ状態に変更
            //ChangeStateTo(SlimeAnimationState.Walk);

            // 移動
            //アニメーションの速度に合わせるために遅くする
            Rigidbody rb = this.GetComponent<Rigidbody>();  // rigidbodyを取得
            rb.AddForce(moveDirection * moveSpeed, ForceMode.Force);    // 力を加える
            //transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //ジャンプ
            Jump();
        }

        //毎フレーム減速する
        buttonCount -= deceleration;

        //buttonCountが0なら
        if (buttonCount <= 0 )
        {
            buttonCount = 0.0f;
        }

        moveSpeed = buttonCount;
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    //ジャンプ
    private void Jump()
    {
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

    public void ChangeStateTo(SlimeAnimationState state)
    {
        if (this == null) return;
        if (state == this.currentState) return;

        this.currentState = state;
    }

    // 当たった時に呼ばれる関数
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            Debug.Log(playerNum + "P Goal"); // ログを表示する
        }
    }
}