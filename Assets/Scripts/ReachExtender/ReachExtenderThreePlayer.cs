using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReachExtenderThreePlayer : MonoBehaviour
{

    //アニメーションに必要
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }

    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;

    public Animator animator;
    public int damType;

    private Material faceMaterial;

    private Vector3 move;

    [SerializeField] private float defeatedSpeed = 1f;

    private Vector3 gravity = new Vector3(0f, 0f, 0f);

    [SerializeField] private bool isStan;
    [SerializeField] private bool isInvincible = false;

    //Rigidbody rb;

    [SerializeField] private float moveSpeed = 5.0f;          // プレイヤーの移動速度
    [SerializeField] private float rotationSpeed = 180.0f;    // プレイヤーの回転速度
    [SerializeField] private bool isHorizontalInput = true;   // 横の入力許可するか
    [SerializeField] private bool isVerticalInput = true;     // 縦の入力許可するか
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private int playerNum;                   // プレイヤー番号
    private bool isMoving = false;
    private bool isDead = false;
    private float stanTime = 2f;
    [SerializeField] private float invincibleTime = 2;

    Rigidbody rb;

    private Transform mainCameraTransform; // メインカメラのTransform

    void Start()
    {
        //マテリアル設定
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        rb = this.GetComponent<Rigidbody>();  // rigidbodyを取得
    }

    //顔のテクスチャ設定
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //やられていたら
        if (isDead)
        {
            Defeated();
            return;
        }

        if (isStan && !isInvincible)
        {
            Invoke("StanCancellation", stanTime);
            isInvincible = true;
        }

        //動いていたら
        if (isMoving || isStan) return;

        //ボタンを押すとパンチする
        Action();

        //動き
        Move();

        //状態更新
        StateUpdata();
    }

    //移動
    private void Move()
    {
        // 入力を取得用
        float horizontalInput = 0;
        float verticalInput = 0;

        // 入力を取得
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + this.GetComponent<PlayerNum>().playerNum);
        if (isVerticalInput) verticalInput = -Input.GetAxis("L_Stick_V" + this.GetComponent<PlayerNum>().playerNum);

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
        //Aボタンが押されてないのならこの先処理しない
        if (!Input.GetButtonDown("Abutton" + +this.GetComponent<PlayerNum>().playerNum)) return;

        //ジャンプ状態に変更    
        ChangeStateTo(SlimeAnimationState.Jump);
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

    public void SetIsMoving(bool a)
    {
        isMoving = a;
    }

    public bool GetIsMoving()
    {
        return isMoving;
    }

    public void Action()
    {
        //Aボタンが押されてないのならこの先処理しない
        if (!Input.GetButtonDown("Abutton" + this.GetComponent<PlayerNum>().playerNum)) return;

        SetIsMoving(true);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "Vortex")
        {
        }
        else
        {
        }
    }

    public void SetMove(Vector3 dir)
    {
        move = dir;
    }

    public void SetIsDead(bool a)
    {
        isDead = a;
    }

    //倒された時の処理
    public void Defeated()
    {
        Vector3 vecUp = Vector3.up * 1f - gravity;
        transform.position += (move.normalized + vecUp) * defeatedSpeed * Time.deltaTime;
        gravity.y += 0.002f;

        //Vector3 vecUp = Vector3.up * 1f - gravity;
        //rb.AddForce((move.normalized + vecUp) * defeatedSpeed * Time.deltaTime);
        //gravity.y += 0.002f;
    }

    public void SetStan(bool a)
    {
        isStan = a;
    }

    //スタン解除
    public void StanCancellation()
    {
        isStan = false;
        Invoke("InvincibleCancellation", invincibleTime);
    }

    //無敵解除
    public void InvincibleCancellation()
    {
        isInvincible = false;
    }

    public void SetInvincible(bool a)
    {
        isInvincible = a;
    }
}