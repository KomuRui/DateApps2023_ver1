using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ThreePlayer : MonoBehaviour
{

    //アニメーションに必要
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }
    Rigidbody rb;
    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;

    public Animator animator;
    public int damType;

    private Material faceMaterial;

    GameObject obj;

    [SerializeField] private float moveSpeed = 10.0f;          // プレイヤーの移動速度
    [SerializeField] private float rotationSpeed = 180.0f;    // プレイヤーの回転速度
    [SerializeField] private bool isHorizontalInput = true;   // 横の入力許可するか
    [SerializeField] private bool isVerticalInput = true;     // 縦の入力許可するか
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private bool isBound = false;
    [SerializeField] private int playerNum;                   // プレイヤー番号
    int a;

    bool isDestroy;
    private Transform mainCameraTransform; // メインカメラのTransform

    void Start()
    {
        //マテリアル設定
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        isDestroy = false;
        rb = this.GetComponent<Rigidbody>();  // rigidbodyを取得

        obj = GameObject.Find("GameManager");

        a = 0;
    }

    //顔のテクスチャ設定
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        if (GameManager.nowMiniGameManager.IsStart() && !GameManager.nowMiniGameManager.IsFinish() && isBound == false)
        {
            //動き
            Move();
        }
        else
        {
            a++;
            if(a > 2000)
            {
                isBound = false;
            }

        }
        //移動制限？
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -3.5f, 3.5f), Mathf.Clamp(transform.position.y, -13.5f, -0.8f), Mathf.Clamp(transform.position.z, -10.5f, -5.5f));
        
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
        //transform.position += moveDirection * moveSpeed * Time.deltaTime;
        rb.AddForce(moveDirection * moveSpeed * Time.deltaTime);

        //transform.position.x = Math.Clamp(transform.position.x, -3.5f, 3.5f);

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    ////ジャンプ
    //private void Jump()
    //{
    //    //Aボタンが押されてないのならこの先処理しない
    //    if (!Input.GetButtonDown("Abutton" + +playerNum)) return;

    //    //ジャンプ状態に変更    
    //    ChangeStateTo(SlimeAnimationState.Jump);
    //}

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

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Player")
        {
            Debug.Log("当たった!");
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            rb.useGravity = true;
            Invoke("DestroyPlayer",1.0f);

            this.enabled = false;
            isBound = true;
        }
    }


    void DestroyPlayer()
    {
        // ミニゲームに死んだことを伝える
        GameManager.nowMiniGameManager.PlayerDead(this.GetComponent<PlayerNum>().playerNum);
        GameManager.nowMiniGameManager.PlayerFinish(this.GetComponent<PlayerNum>().playerNum);

        //オブジェクトを削除
        Destroy(this.gameObject.transform.parent.gameObject);
    }



    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, -30, 0),ForceMode.Acceleration);
    }
}