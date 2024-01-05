using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryToTheGoalPlayer : MonoBehaviour
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
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private int playerNum;                   // プレイヤー番号
    [SerializeField] private Vector3 localGravity;
    [SerializeField] private float jumpPower = 0.05f;         // ジャンプ力
    private Rigidbody rBody;
    private Transform mainCameraTransform; // メインカメラのTransform
    private bool isDamege = false;
    public bool isMuteki = false;          //大砲の玉当たらない無敵かどうか
    private bool isJump = false; 
    private bool isJumpInvoke = false;
    private bool isJumpMuteki = false;     //ジャンプ当たらない無敵かどうか
    private bool isStan = false;
    private Vector3 initialScale;
    private Vector3 stanScale;

    void Start()
    {
        //マテリアル設定
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        rBody = this.GetComponent<Rigidbody>();
        initialScale = transform.localScale;
        stanScale = transform.localScale;
        stanScale.y = 1.3f;
    }

    //顔のテクスチャ設定
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //開始していないか終わっているのなら
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish() || isDamege || isStan) return;

        //状態更新
        StateUpdata();

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
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + this.GetComponent<PlayerNum>().playerNum);
        if (isVerticalInput) verticalInput = -Input.GetAxis("L_Stick_V" + this.GetComponent<PlayerNum>().playerNum);

        //入力がないのなら
        if ((horizontalInput == 0 && verticalInput == 0) || isDamege)
        {
            //通常状態に変更
            ChangeStateTo(SlimeAnimationState.Idle);
            return;
        }

        // カメラの向きを基準にプレイヤーを移動
        Vector3 forwardDirection = mainCameraTransform.forward;
        Vector3 rightDirection = mainCameraTransform.right;
        forwardDirection.y = 0f; // Y軸成分を0にすることで水平方向に制限

        // 移動方向を計算
        Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        //歩き状態に変更
        if (!isJump)
        {
            ChangeStateTo(SlimeAnimationState.Walk);
            rBody.AddForce(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            ChangeStateTo(SlimeAnimationState.Idle);
            rBody.AddForce(moveDirection * moveSpeed * 10000 * Time.deltaTime);
        }

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    //ジャンプ
    private void Jump()
    {
        //Aボタンが押されてないのならこの先処理しない
        if (!Input.GetButtonDown("Abutton" + this.GetComponent<PlayerNum>().playerNum) || isJump) return;

        //エフェクトの発生位置を求める
        Vector3 efePos = transform.position;
        efePos.y += 0.2f;

        //エフェクト
        ((CarryToTheGoalGameManager)GameManager.nowMiniGameManager).JumpEffect(efePos);

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

    //ダメージ
    public void Damege()
    {
        isDamege = true;
        isMuteki = true;
        rBody.velocity = Vector3.zero;
        ChangeStateTo(SlimeAnimationState.Idle);
        StateUpdata();


        var children = this.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        for (int i = 0; i < children.Length; i++)
        {
            Color r = children[i].material.color;
            r.a = 0.6f;
            children[i].material.color = r;
        }

        var children2 = this.GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < children2.Length; i++)
        {
            Color r = children2[i].material.color;
            r.a = 0.6f;
            children2[i].material.color = r;
        }

        StartCoroutine(UnLook(2.0f));
        StartCoroutine(UnLookMuteki(4.0f));
    }

    IEnumerator UnLook(float delay)
    {
        yield return new WaitForSeconds(delay);

        isDamege = false;
    }

    IEnumerator UnLookMuteki(float delay)
    {
        yield return new WaitForSeconds(delay);

        var children = this.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        for (int i = 0; i < children.Length; i++)
        {
            Color r = children[i].material.color;
            r.a = 1.0f;
            children[i].material.color = r;
        }

        var children2 = this.GetComponentsInChildren<MeshRenderer>(true);
        for (int i = 0; i < children2.Length; i++)
        {
            Color r = children2[i].material.color;
            r.a = 1.0f;
            children2[i].material.color = r;
        }

        isMuteki = false;
    }

    //死亡
    public void Dead()
    {
        GameManager.nowMiniGameManager.PlayerFinish(this.gameObject.GetComponent<PlayerNum>().playerNum);
        GameManager.nowMiniGameManager.PlayerDead(this.gameObject.GetComponent<PlayerNum>().playerNum);
        Destroy(this.gameObject);
    }

    public void SetResetJump() { isJump = false; isJumpInvoke = false; }
    public void SetResetMuteki() { isJumpMuteki = false; }
    public void SetResetStan()
    {
        isStan = false;
        SetResetScale();
    }
    public void SetResetScale() { transform.localScale = initialScale; }
    public void SetOhNoScale() { transform.localScale = stanScale; }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "CarryStage" && isJump)
        {
            //エフェクトの発生位置を求める
            Vector3 efePos = transform.position;
            efePos.y += 0.2f;

            //エフェクト
            ((CarryToTheGoalGameManager)GameManager.nowMiniGameManager).JumpEffect(efePos);

            //ジャンプのインターバル開始
            isJumpInvoke = true;
            Invoke("SetResetJump", 0.3f);
        }

        if (other.transform.tag != "Player") return;

        //二段ジャンプの条件が成立しているのなら
        if (rBody.velocity.y < 100 && other.transform.position.y < transform.position.y && isJump)
        {
            //二段ジャンプ処理
            isJumpInvoke = true;
            rBody.AddForce(Vector3.up * (jumpPower * 0.8f));

            //エフェクトの発生位置を求める
            Vector3 efePos = other.contacts[0].point;
            efePos.x = transform.position.x;
            efePos.z = transform.position.z;

            //エフェクトを衝突位置に
            ((CarryToTheGoalGameManager)GameManager.nowMiniGameManager).HitEffect(efePos);

            //当たったプレイヤーを取得
            CarryToTheGoalPlayer targetPlayer = other.transform.GetComponent<CarryToTheGoalPlayer>();

            //無敵ならこの先処理しない
            if (targetPlayer.isJumpMuteki) return;
            targetPlayer.isJumpMuteki = true;
            targetPlayer.isStan = true;
            targetPlayer.Invoke("SetResetStan", 3.0f);
            targetPlayer.Invoke("SetResetMuteki", 4.0f);
            targetPlayer.SetOhNoScale();
        }
    }
}
