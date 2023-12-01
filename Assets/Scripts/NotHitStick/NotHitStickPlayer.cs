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
    [SerializeField] private float beforeInput = 0;           // 前回の入力値
    [SerializeField] private bool isHorizontalInput = true;   // 横の入力許可するか
    [SerializeField] private bool isVerticalInput = true;     // 縦の入力許可するか
    [SerializeField] private bool isAnimIdle = true;          // 通常時のアニメーション許可するか
    [SerializeField] private bool isAnimWalk = true;          // 歩くアニメーション許可するか 
    [SerializeField] private bool isAnimJump = true;          // ジャンプアニメーション許可するか
    [SerializeField] private bool isAnimAttack = true;        // 攻撃アニメーション許可するか
    [SerializeField] private bool isAnimDamage = true;        // ダメージアニメーション許可するか
    [SerializeField] private GameObject[] stage;              // 床のゲームオブジェクト
    [SerializeField] private float jumpPower;                 // ジャンプ力
    [SerializeField] private int nowStageNum;                 // 乗っている床の番号
    [SerializeField] private int playerNum;                   // プレイヤー番号

    //リジットボディ
    private Rigidbody rb;

    //ジャンプしているか
    private bool isJump = false;
    private bool isJump2 = false;

    // メインカメラのTransform
    private Transform mainCameraTransform;

    private Tweener tweener;

    void Start()
    {
        //マテリアル設定
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        //リジットボディ取得
        rb = GetComponent<Rigidbody>();

        //プレイヤー番号設定
        playerNum = this.GetComponent<PlayerNum>().playerNum;
    }

    //顔のテクスチャ設定
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //開始していないか終わっているのなら
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;

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
        if (isJump  || isJump2) return;

        // 入力を取得用
        float horizontalInput = 0;
        float verticalInput = 0;

        // 入力を取得
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + playerNum);
        if (isVerticalInput) verticalInput = Input.GetAxis("L_Stick_V" + playerNum);

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
        //ジャンプしているなら上にレイを飛ばす
        if (isJump)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, Vector3.up); // Rayを生成
            RaycastHit hit2;
            Ray ray2 = new Ray(new Vector3(transform.position.x + 0.30f, transform.position.y, transform.position.z), Vector3.up); // Rayを生成
            RaycastHit hit3;
            Ray ray3 = new Ray(new Vector3(transform.position.x - 0.30f, transform.position.y, transform.position.z), Vector3.up); // Rayを生成

            if (Physics.Raycast(ray, out hit, 10000) || Physics.Raycast(ray2, out hit2, 10000) || Physics.Raycast(ray3, out hit3, 10000))
            {
                //元に戻す
                StartCoroutine(Drop(0.3f));
            }

            return;
        }

        if (isJump2) return;

        //通常
        if (Input.GetButtonDown("Abutton" + playerNum))
        {
            //通常状態に変更
            ChangeStateTo(SlimeAnimationState.Idle);

            //上に力を加える
            rb.AddForce(Vector3.up * jumpPower);
            isJump2 = true;
            return;
        }

        int beforeStage = nowStageNum;
        float nowInput = Input.GetAxis("L_Stick_V" + playerNum);

        //自動ジャンプ(別の足場に)
        if (nowInput <= -0.8f && beforeInput > -0.8f)
        {
            nowStageNum--;
            nowStageNum = Math.Max(nowStageNum, 0);
            if (beforeStage == nowStageNum) return;
            tweener = transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f).OnComplete(() => { isJump2 = false; isJump = false; });
            tweener.Play();
            ChangeStateTo(SlimeAnimationState.Idle);
            rb.AddForce(Vector3.up * jumpPower);
            isJump = true;
            isJump2 = true;
        }
        else if (nowInput >= 0.8f && beforeInput < 0.8f)
        {
            nowStageNum++;
            nowStageNum = Math.Min(nowStageNum, stage.Length - 1);
            if (beforeStage == nowStageNum) return;
            tweener = transform.DOMoveZ(stage[nowStageNum].transform.position.z, 1.0f).OnComplete(() => { isJump2 = false; isJump = false; });
            tweener.Play();
            ChangeStateTo(SlimeAnimationState.Idle);
            rb.AddForce(Vector3.up * jumpPower);
            isJump = true;
            isJump2 = true;
        }

        beforeInput = nowInput;
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
        {
            isJump2 = false;

            if(rb != null)
                rb.velocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Sea")
        {
            StartCoroutine(Kill(1.0f));
        }
    }

    //死亡
    IEnumerator Kill(float delay)
    {
        yield return new WaitForSeconds(delay);

        GameManager.nowMiniGameManager.PlayerDead(this.GetComponent<PlayerNum>().playerNum);
        GameManager.nowMiniGameManager.PlayerFinish(this.GetComponent<PlayerNum>().playerNum);
        Destroy(this.gameObject);
    }

    //落とす
    IEnumerator Drop(float delay)
    {
        yield return new WaitForSeconds(delay);

        //力を止める
        rb.velocity = Vector3.zero;

        //点滅止める
        tweener.Pause();

    }
}
