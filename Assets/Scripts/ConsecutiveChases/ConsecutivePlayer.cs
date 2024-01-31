using System.Collections.Generic;
using System.Linq;
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

    [SerializeField] public List<Image> nextCommandImage; //次のコマンドの画像を表示する場所のリスト
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
    [SerializeField] private ChasesManager chasesManager;     //マネージャー
    [SerializeField] private ConsecutiveSE se;                          //SE

    public int threePlayerNum = 0;
    public bool buttonFlag = true;
    public bool isGoal = false; //ゴールしたか
    public bool isDead = false; //死んでいるか
    public bool isMiss = false; //直線に入力を間違えたかどうか
    [SerializeField] private float limitAngle = 45f;
    [SerializeField] private float rotateSpeed = 0.05f;

    private Transform mainCameraTransform; // メインカメラのTransform

    private List<Palm> palmList = new List<Palm>();
    Rigidbody rb;
    private float grassSpeed = 1.0f;
    [SerializeField] private bool isStan;
    [SerializeField] private float stanTime = 0.5f;
    [SerializeField] private float stanSpeed = 0.5f;

    void Start()
    {
        //マテリアル設定
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        //プレイヤーの移動方向の正規化
        moveDirection.Normalize();

        buttonFlag = false;

        nextCommandImage[threePlayerNum].sprite = commandImageList[1];
        int a = 0;
        rb = this.GetComponent<Rigidbody>();  // rigidbodyを取得
    }

    //顔のテクスチャ設定
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //もしもゲームが始まっていて、終わっていく、ゴールしていなかったら
        if (GameManager.nowMiniGameManager.IsStart() && !GameManager.nowMiniGameManager.IsFinish() && !isGoal && !isStan)
        {
            //動き
            NewMove();

            //投げる
            Throw();
        }

        //状態更新
        StateUpdata();
    }

    //移動
    private void Move()
    {
        // 入力を取得
        bool isAbuttonClick = Input.GetButtonDown("Abutton" + this.gameObject.GetComponent<PlayerNum>().playerNum);
        bool isBbuttonClick = Input.GetButtonDown("Bbutton" + this.gameObject.GetComponent<PlayerNum>().playerNum);

        //入力が成功しているかどうか
        bool isInputSuccess = true;


        if (buttonFlag)
        {
            // Aボタンを押していたら
            if (isAbuttonClick)
            {
                buttonCount += addSpeed;
                buttonFlag = !buttonFlag;
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
            }
        }

        //もし入力に失敗していたら
        if (!isInputSuccess)
        {
            //減速する
            buttonCount -= missDeceleration;

            isMiss = true;
            Invoke(nameof(SetFalseMiss), 0.1f);
        }

        ImageChange();

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
            rb.AddForce(moveDirection * moveSpeed * Time.deltaTime * 400, ForceMode.Force);    // 力を加える
            //transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }

        //毎フレーム減速する
        buttonCount -= deceleration;

        //buttonCountが0なら
        if (buttonCount <= 0)
        {
            buttonCount = 0.0f;
        }

        //もしスピードが最大になったら
        if (SPEED_MAX <= buttonCount)
        {
            buttonCount = SPEED_MAX;
        }

        moveSpeed = buttonCount;
        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

    }

    //新しい仕様の移動
    private void NewMove()
    {
        // 入力を取得
        bool isAbuttonClick = Input.GetButtonDown("Abutton" + this.gameObject.GetComponent<PlayerNum>().playerNum);
        bool isBbuttonClick = Input.GetButtonDown("Bbutton" + this.gameObject.GetComponent<PlayerNum>().playerNum);

        //移動ベクトルの一時的な入れ物
        Vector3 tmpMoveDirection = Vector3.zero;

        // Aボタンを押していたら
        if (isAbuttonClick)
        {
            tmpMoveDirection = new Vector3(rotateSpeed, 0,0);
        }

        // Bボタンを押していたら
        if (isBbuttonClick)
        {
            tmpMoveDirection = new Vector3(-rotateSpeed, 0, 0);
        }

        //移動方向が変わっていたら
        if(tmpMoveDirection != Vector3.zero)
        {
            //移動方向
            moveDirection += tmpMoveDirection;

            if(moveDirection.normalized.x > limitAngle)
            {
                moveDirection = new Vector3 (limitAngle, moveDirection.normalized.y, moveDirection.normalized.z);
            }
            if (moveDirection.normalized.x < -limitAngle)
            {
                moveDirection = new Vector3(-limitAngle, moveDirection.normalized.y, moveDirection.normalized.z);
            }

            //加速
            buttonCount += addSpeed;
        }

        //ImageChange();

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
            rb.AddForce(moveDirection * moveSpeed * Time.deltaTime * 400 * grassSpeed, ForceMode.Force);    // 力を加える
            //transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //ジャンプ
            Jump();
        }

        //毎フレーム減速する
        buttonCount -= deceleration;

        //buttonCountが0なら
        if (buttonCount <= 0)
        {
            buttonCount = 0.0f;
        }

        //もしスピードが最大になったら
        if (SPEED_MAX <= buttonCount)
        {
            buttonCount = SPEED_MAX;
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
        //ヤシの実に当たったら
        if (other.gameObject.tag == "Palm")
        {
            Palm palm = other.GetComponent<Palm>();

            //ヤシの実が誰のものでもなかったら
            if (palm.throwObj == null)
            {
                palm.throwObj = this.gameObject;

                //リストに追加
                palmList.Add(palm);

                //ヤシの実を持っている状態にする
                palm.SetisPickUp(true);

                //ヤシの実を非アクティブ化
                palm.gameObject.SetActive(false);
            }
            else
            {
                //投げた本人でなければ
                if (this.gameObject != palm.throwObj)
                {
                    //スタン
                    SetStan(true);

                    //スタンしたときに減速する
                    addSpeed *= stanSpeed;

                    //ヤシの実を非アクティブ化
                    palm.gameObject.SetActive(false);
                }
            }
        }

        if (other.gameObject.tag == "Goal")
        {
            //Debug.Log(playerNum + "P Goal"); // ログを表示する
            //isGoal = true;

            //ゲームマネージャーに終わったことを伝える
            //chasesManager.PlayerGoal(this.GetComponent<PlayerNum>().playerNum);
            //GameManager.nowMiniGameManager.PlayerFinish(this.gameObject.GetComponent<PlayerNum>().playerNum);
        }
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Hit"); // ログを表示する

            Rigidbody rb = this.gameObject.GetComponent<Rigidbody>();  // rigidbodyを取得
            rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
            Vector3 force = new Vector3(0.0f, 15000.0f, 0.0f);  // 力を設定
            rb.AddForce(force * Time.deltaTime, ForceMode.Force);          // 力を加える

            isDead = true;
            GameManager.nowMiniGameManager.PlayerDead(this.GetComponent<PlayerNum>().playerNum);

            //se.MissAudio();
            //ゲームマネージャーに終わったことを伝える
            //GameManager.nowMiniGameManager.PlayerFinish(this.gameObject.GetComponent<PlayerNum>().playerNum);
        }
    }

    void OnTriggerStay(Collider other)
    {
        //草に当たったら
        if (other.gameObject.tag == "Grass")
        {
            grassSpeed = 0.5f;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            Debug.Log(playerNum + "P Goal"); // ログを表示する
            isGoal = true;

            ChasesManager a = GameManager.nowMiniGameManager.gameObject.GetComponent<ChasesManager>();
            a.goalPlayer.Add(gameObject.GetComponent<PlayerNum>().playerNum);
        }

        if (other.gameObject.tag == "Grass")
        {
            grassSpeed = 1.0f;
        }
    }

    /// <summary>
    /// 画像を変える関数
    /// </summary>
    public void ImageChange()
    {
        //直前に入力を間違っていたら
        if(isMiss)
        {
            if (buttonFlag) 
            {
                nextCommandImage[threePlayerNum].sprite = commandImageList[2];
            }
            else
            {
                nextCommandImage[threePlayerNum].sprite = commandImageList[3];
            }
        }
        else
        {
            if (buttonFlag)
            {
                nextCommandImage[threePlayerNum].sprite = commandImageList[0];
            }
            else
            {
                nextCommandImage[threePlayerNum].sprite = commandImageList[1];
            }
        }
        
    }

    //セッター
    public void SetFalseMiss()
    {
        isMiss = false;
    }

    //ヤシの実を投げる
    public void Throw()
    {
        //ボタンを押していなかったら戻る
        if (!Input.GetButton("RBbutton" + this.gameObject.GetComponent<PlayerNum>().playerNum))
            return;

        //ヤシの実を持っていたら
        if (palmList.Count != 0)
        {
            if (palmList.First() == null) return;

            //ヤシの実をアクティブに
            palmList.First().gameObject.SetActive(true);

            //ヤシの実の位置と向きをセット
            palmList.First().transform.position = transform.position;
            palmList.First().SetDir(transform.forward);

            //リストから削除
            palmList.Remove(palmList.First());
        }
    }

    public void SetStan(bool a)
    {
        isStan = a;
        Invoke("StanCancellation", stanTime);
    }

    //スタン解除
    public void StanCancellation()
    {
        isStan = false;
    }

}