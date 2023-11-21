using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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

    [SerializeField] private Image nextCommandImage; //次のコマンドの画像を表示する場所のリスト
    [SerializeField] private List<Sprite> commandImageList = new List<Sprite>(); //コマンドの画像のリスト（何の画像を使うか）
    [SerializeField] private float SPEED_MAX = 14.0f;       //スピードの最大
    [SerializeField] private float deceleration = 150.0f;       //減速率
    [SerializeField] private float missDeceleration = 50.0f;       //減速率
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
    [SerializeField] private int playerNum;                   // プレイヤー番号
    
    public bool buttonFlag = true;
    public bool goolFlag = false;
    public bool isDead = false;

    private Transform mainCameraTransform; // メインカメラのTransform

    void Start()
    {
        //マテリアル設定
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        //プレイヤーの移動方向の正規化
        moveDirection.Normalize();

        buttonFlag = false;

        nextCommandImage.sprite = commandImageList[1];
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
        bool isAbuttonClick = Input.GetButtonDown("Abutton" + playerNum);
        bool isBbuttonClick = Input.GetButtonDown("Bbutton" + playerNum);

        //入力が成功しているかどうか
        bool isInputSuccess = true;


        if (buttonFlag)
        {
            // Aボタンを押していたら
            if (isAbuttonClick)
            {
                buttonCount += addSpeed;
                buttonFlag = !buttonFlag;
                nextCommandImage.sprite = commandImageList[1];
            }

            // Bボタンを押していたら
            if (isBbuttonClick)
            {
                isInputSuccess = false;
            }
        }
        else
        {
            // Aボタンを押していたら
            if (isAbuttonClick)
            {
                isInputSuccess = false;
            }

            // Bボタンを押していたら
            if (isBbuttonClick)
            {
                buttonCount += addSpeed;
                buttonFlag = !buttonFlag;
                nextCommandImage.sprite = commandImageList[0];
            }
        }
       
        //もし入力に失敗していたら
        if (!isInputSuccess)
        {
            buttonCount -= missDeceleration;
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

        //もしスピードが最大になったら
        if(SPEED_MAX <= buttonCount)
        {
            buttonCount = SPEED_MAX;
        }

        moveSpeed = buttonCount;
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

        //高くまで行くと消える
        if(this.transform.position.y > 100)
        {
           // Destroy(this.gameObject);
        }
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
            goolFlag = true;
        }
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit"); // ログを表示する

            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();  // rigidbodyを取得
            Vector3 force = new Vector3(0.0f, 800.0f, 1.0f);  // 力を設定
            rb.AddForce(force, ForceMode.Force);          // 力を加える

            isDead = true;
        }

       
    }
}