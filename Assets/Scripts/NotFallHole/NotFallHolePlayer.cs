using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotFallHolePlayer : MonoBehaviour
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
    [SerializeField] private float jumpPower = 0.05f;         // ジャンプ力
    [SerializeField] private bool isHorizontalInput = true;   // 横の入力許可するか
    [SerializeField] private bool isVerticalInput = true;     // 縦の入力許可するか
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private int playerNum;                   // プレイヤー番号
    [SerializeField] private Vector3 localGravity;
    private bool isJump;
    private bool isJumpInvoke;
    private bool isMuteki;
    private bool isStan;
    private Vector3 initialScale;
    private Vector3 stanScale;
    public Rigidbody rBody;
    private Transform mainCameraTransform; // メインカメラのTransform
    private Ray ray; // Rayを生成

    void Start()
    {
        //マテリアル設定
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        initialScale = transform.localScale;
        stanScale = transform.localScale;
        stanScale.y = 1.3f;
        isJump = false;
        isJumpInvoke = false;
        isMuteki = false;
        isStan = false;
        rBody = this.GetComponent<Rigidbody>();
        playerNum = this.GetComponent<PlayerNum>().playerNum;
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

        //開始していないか終わっているのなら
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish() || isStan) return;

        //動き
        Move();

        //ジャンプ
        Jump();
    }

    private void FixedUpdate()
    {
        SetLocalGravity(); //重力をAddForceでかけるメソッドを呼ぶ。FixedUpdateが好ましい。
    }

    private void SetLocalGravity()
    {
        rBody.AddForce(localGravity, ForceMode.Acceleration);
    }

    //移動
    private void Move()
    {
        // 入力を取得用
        float horizontalInput = 0;
        float verticalInput = 0;

        // 入力を取得
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + playerNum);
        if (isVerticalInput) verticalInput = -Input.GetAxis("L_Stick_V" + playerNum);

        //入力がないのなら
        if (horizontalInput == 0 && verticalInput == 0)
        {
            //通常状態に変更
            ChangeStateTo(SlimeAnimationState.Idle);
            return;
        }

        //歩き状態に変更
       // ChangeStateTo(SlimeAnimationState.Walk);

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
        if (!Input.GetButtonDown("Abutton" + playerNum) || isJump) return;

        //ジャンプ状態に変更    
        rBody.AddForce(Vector3.up * jumpPower);
        isJump = true;
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

    public void SetResetJump() { isJump = false; isJumpInvoke = false; }
    public void SetResetMuteki() { isMuteki = false; }
    public void SetResetStan() { 
        isStan = false;
        this.GetComponent<BoxCollider>().enabled = false;
        this.GetComponent<CapsuleCollider>().enabled = true;
        SetResetScale();
    }
    public void SetResetScale() { transform.localScale = initialScale; }
    public void SetOhNoScale() { transform.localScale = stanScale; }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "Floor" && !other.transform.parent.GetComponent<FallRotateFloor>().isRotate && !isJumpInvoke && isJump)
        {
            //エフェクト
            ((NotFallHoleGameManager)GameManager.nowMiniGameManager).tyakutiEffect(new Vector3(other.contacts[0].point.x, other.contacts[0].point.y + 0.1f, other.contacts[0].point.z));

            //ジャンプのインターバル開始
            isJumpInvoke = true;
            Invoke("SetResetJump", 0.3f);
        }
    }

    void OnCollisionStay(Collision other)
    {
        if (other.transform.tag != "Player") return;
        
        //二段ジャンプの条件が成立しているのなら
        if (rBody.velocity.y < 0 && other.transform.position.y < transform.position.y)
        {
            //二段ジャンプ処理
            isJumpInvoke = true;
            rBody.AddForce(Vector3.up * (jumpPower * 0.8f));
            Invoke("SetResetJump", 0.3f);

            //エフェクトを衝突位置に
            ((NotFallHoleGameManager)GameManager.nowMiniGameManager).hitEffect(other.contacts[0].point);

            //無敵ならこの先処理しない
            if (other.transform.GetComponent<NotFallHolePlayer>().isMuteki) return;
            other.transform.GetComponent<NotFallHolePlayer>().isMuteki = true;
            other.transform.GetComponent<NotFallHolePlayer>().isStan = true;
            other.transform.GetComponent<NotFallHolePlayer>().Invoke("SetResetStan",3.0f);
            other.transform.GetComponent<NotFallHolePlayer>().Invoke("SetResetMuteki", 4.0f);
            other.transform.GetComponent<NotFallHolePlayer>().SetOhNoScale();
            other.transform.GetComponent<NotFallHolePlayer>().GetComponent<BoxCollider>().enabled = true;
            other.transform.GetComponent<NotFallHolePlayer>().GetComponent<CapsuleCollider>().enabled = false;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Sea")
        {
            GameManager.nowMiniGameManager.PlayerDead(this.GetComponent<PlayerNum>().playerNum);
            GameManager.nowMiniGameManager.PlayerFinish(this.GetComponent<PlayerNum>().playerNum);
        }
    }


}
