using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Windows;
using Random = UnityEngine.Random;

public class ChasesPlayer : MonoBehaviour
{
    //アニメーションに必要
    public enum SlimeAnimationState { Idle, Walk, Jump, Attack, Damage }

    public Face faces;
    public GameObject SmileBody;
    public SlimeAnimationState currentState;

    public Animator animator;
    public int damType;

    
    private Material faceMaterial;

    [SerializeField] private Vector3 moveDirection = new Vector3(0.0f, 0.0f, 1.0f); //プレイヤーの移動方向
    [SerializeField] private float deceleration = 150.0f;       //減速率
    [SerializeField] private float missDeceleration = 50.0f;       //減速率
    [SerializeField] private float SPEED_MAX = 15.0f;           //スピードの最大
    [SerializeField] private float addSpeed = 1000.0f;             // ボタンを押したときプレイヤーの移動速度の上昇値
    [SerializeField] private float moveSpeed = 0.01f;           // プレイヤーの移動速度
    [SerializeField] private float rotationSpeed = 180.0f;      // プレイヤーの回転速度
    [SerializeField] private bool isHorizontalInput = true;     // 横の入力許可するか
    [SerializeField] private bool isVerticalInput = true;       // 縦の入力許可するか
    [SerializeField] private bool isAnimIdle = true;
    [SerializeField] private bool isAnimWalk = true;
    [SerializeField] private bool isAnimJump = true;
    [SerializeField] private bool isAnimAttack = true;
    [SerializeField] private bool isAnimDamage = true;
    [SerializeField] private float buttonCount = 0.0f;                // 入力を取得用
    [SerializeField] private float crossAxisV;                        //十字キーの縦の入力値
    [SerializeField] private float crossAxisH;                        //十字キーの横の入力値
    [SerializeField] private float COMMAND_SIZE_MAX = 4;              //次のコマンドのリストの最大数
    [SerializeField] private int playerNum;                           // プレイヤー番号
    [SerializeField] private Queue<COMMAND_TYPE> nextCommand = new();                    //次のコマンドのキュー
    [SerializeField] private List<Tuple<COMMAND_TYPE, COMMAND_STATE>> commandList = new();                    //次のコマンドのキュー
    [SerializeField] public List<Image> nextCommandImageList = new List<Image>(); //次のコマンドの画像を表示する場所のリスト
    [SerializeField] private List<Sprite> commandImageList = new List<Sprite>(); //コマンドの画像のリスト（何の画像を使うか）
    [SerializeField] private List<Sprite> newCommandImageList = new List<Sprite>(); //コマンドの画像のリスト（何の画像を使うか）

    private bool crossKeyContinuous = false;    //十字キー
    private bool isControll = false;            //操作しているかどうか
    bool isMiss = false;//直前にミスしたかどうか
    COMMAND_TYPE nextCommandButton = COMMAND_TYPE.COMMAND_MAX;                    //次のコマンド

    private Transform mainCameraTransform; // メインカメラのTransform

    //コマンドの種類
    public enum COMMAND_TYPE
    {
        CROSS_BUTTON_UP = 0,
        CROSS_BUTTON_DOWN,
        CROSS_BUTTON_LEFT,
        CROSS_BUTTON_RIGHT,
        NONE,
        COMMAND_MAX,
    }

    //コマンド入力の結果
    public enum COMMAND_RESULT
    {
        SUCCESS = 0,
        MISS,
        NONE,
        MAX,
    }

    public enum COMMAND_STATE
    {
        DEFAULT = 0,
        NEXT,
        SUCCESS,
        MISS,
        MAX,
    }

    void Start()
    {
        //マテリアル設定
        faceMaterial = SmileBody.GetComponent<Renderer>().materials[1];

        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        //プレイヤーの移動方向の正規化
        moveDirection.Normalize();

        //コマンドの初期化
        KeepCommand();

        //コマンドの状態の初期化
        KeepStateCommand(COMMAND_RESULT.NONE);

        //コマンドの画像を入れ替える
        SetCommandImage();

        ///////////改悪前/////////////
        //KeepCommand();

        //コマンド画像を入れる
        //SetCommandImage();
        ///////////////////////////////
    }

    //顔のテクスチャ設定
    void SetFace(Texture tex)
    {
        faceMaterial.SetTexture("_MainTex", tex);
    }

    void Update()
    {
        //もしもゲームが始まっていて、終わっていなかったら
        if (GameManager.nowMiniGameManager.IsStart() && !GameManager.nowMiniGameManager.IsFinish())
        {
            //コマンドの画像を入れ替える
            SetCommandImage();

            //動き
            Move();
        }

        //状態更新
        StateUpdata();
    }

    //移動
    private void Move()
    {
        //入力を受け付ける
        COMMAND_RESULT command = CheckOnCommandButton();

        //コマンド入力が成功していたら
        switch (command)
        {
            case COMMAND_RESULT.SUCCESS:

                //スピードを上げる
                buttonCount += addSpeed;
                /////////////////////改悪前/////////////////
                //コマンドが成功した場合の処理
                //SuccessCommand();
                ////////////////////////////////////////////

                break;
            case COMMAND_RESULT.MISS:

                //コマンドが失敗した場合の処理
                //一定量減速する
                buttonCount -= missDeceleration;

                break;
            case COMMAND_RESULT.NONE:

                break;
            default:
                break;
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
            //ChangeStateTo(SlimeAnimationState.Jump);

            // 移動
            Rigidbody rb = this.GetComponent<Rigidbody>();  // rigidbodyを取得
            rb.AddForce(moveDirection * moveSpeed * Time.deltaTime * 150, ForceMode.Force);    // 力を加える
            //transform.position += moveDirection * moveSpeed * Time.deltaTime;

            //ジャンプ
            Jump();
        }

        //コマンドの状態を変える
        KeepStateCommand(command);

        buttonCount -= deceleration;

        //buttonCountが0なら
        if (buttonCount <= 0)
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

    //次のコマンドのボタンが押されたかどうか調べる
    public COMMAND_RESULT CheckOnCommandButton()
    {
        //十字キーの入力を受け取る
        crossAxisV = UnityEngine.Input.GetAxis("D_Pad_V" + this.GetComponent<PlayerNum>().playerNum);
        crossAxisH = UnityEngine.Input.GetAxis("D_Pad_H" + this.GetComponent<PlayerNum>().playerNum);

        //十字キーが離されたらもう一回押せるようになる
        if (crossAxisV == 0 && crossAxisH == 0)
        {
            crossKeyContinuous = false;
        }

        if (!crossKeyContinuous)
        {
            COMMAND_TYPE priorityCommand = COMMAND_TYPE.NONE;   //優先コマンド
            float priorityLevel = 0.0f;     //優先度

            //コマンドチェック       
            if (crossAxisV > 0)
            {
                //優先コマンドを代入
                priorityCommand = COMMAND_TYPE.CROSS_BUTTON_UP;

                //優先度を代入
                priorityLevel = Mathf.Abs(crossAxisV);

                Debug.Log("上");
            }
            if (crossAxisV < 0 && priorityLevel < Mathf.Abs(crossAxisV))
            {
                //優先コマンドを代入
                priorityCommand = COMMAND_TYPE.CROSS_BUTTON_DOWN;

                //優先度を代入
                priorityLevel = Mathf.Abs(crossAxisV);
                Debug.Log("下");
            }
            if (crossAxisH < 0 && priorityLevel < Mathf.Abs(crossAxisH))
            {
                //優先コマンドを代入
                priorityCommand = COMMAND_TYPE.CROSS_BUTTON_LEFT;

                //優先度を代入
                priorityLevel = Mathf.Abs(crossAxisH);

                Debug.Log("左");
            }
            if (crossAxisH > 0 && priorityLevel < Mathf.Abs(crossAxisH))
            {
                //優先コマンドを代入
                priorityCommand = COMMAND_TYPE.CROSS_BUTTON_RIGHT;

                Debug.Log("右");
            }

            //コマンド入力してなかったら
            if (priorityCommand == COMMAND_TYPE.NONE)
            {
                crossKeyContinuous = false;
                return COMMAND_RESULT.NONE;
            }
            //コマンド入力に成功していたら
            else if (nextCommandButton == priorityCommand)
            {
                crossKeyContinuous = true;
                return COMMAND_RESULT.SUCCESS;
            }
            //コマンド入力に失敗したら
            else
            {
                crossKeyContinuous = true;
                return COMMAND_RESULT.MISS;
            }
        }
        return COMMAND_RESULT.NONE;
    }

    //ランダムで今あるコマンド選択
    public COMMAND_TYPE RandCommand()
    {
        switch (Random.Range(0, 4))
        {
            case 0 :
                return COMMAND_TYPE.CROSS_BUTTON_UP;
            case 1:
                return COMMAND_TYPE.CROSS_BUTTON_DOWN;
            case 2:
                return COMMAND_TYPE.CROSS_BUTTON_LEFT;
            case 3:
                return COMMAND_TYPE.CROSS_BUTTON_RIGHT;
            default:
                break;
        }
        return COMMAND_TYPE.CROSS_BUTTON_UP;
        ////////////////改悪前////////////////////////////////
        //return (COMMAND_TYPE)Random.Range((int)COMMAND_TYPE.CROSS_BUTTON_UP, 4);
    }

    //コマンドが成功した場合の処理
    public void SuccessCommand()
    {
        ////////////////改悪前////////////////////////////////
        ////コマンドの削除
        //nextCommand.Dequeue();

        ////コマンドを一定数に保つ処理
        //KeepCommand();

        ////コマンド画像を入れる
        //SetCommandImage();
    }

    //コマンドを一定数に保つ処理
    public void KeepCommand()
    {
        //コマンドを入れる処理
        for (int i = commandList.Count; i < COMMAND_SIZE_MAX; i++)
        {
            //ランダムにコマンドを入れる
            Tuple<COMMAND_TYPE, COMMAND_STATE> tmp = new(RandCommand(), COMMAND_STATE.DEFAULT);
            commandList.Add(tmp);
        }

        ////////////////改悪前////////////////////////////////
        ////コマンドの数が減っている時
        //for (int i = nextCommand.Count; i < COMMAND_SIZE_MAX; i++)
        //{
        //    //ランダムにコマンドを入れる
        //    nextCommand.Enqueue(RandCommand());
        //}
    }

    //コマンド画像を入れる
    public void SetCommandImage()
    {
        int num = 0;
        foreach (var item in commandList)
        {
            //ボタンの状態がデフォルトなら
            if(item.Item2 == COMMAND_STATE.DEFAULT)
            {
                //画像を入れ替える
                nextCommandImageList[num].sprite = newCommandImageList[((int)COMMAND_STATE.DEFAULT * 4) + (int)item.Item1];
            }
            //ボタンの状態がネクストなら
            if (item.Item2 == COMMAND_STATE.NEXT)
            {
                //画像を入れ替える
                nextCommandImageList[num].sprite =newCommandImageList[((int)COMMAND_STATE.NEXT * 4) + (int)item.Item1];
            }
            //ボタンの状態がサクセスなら
            if (item.Item2 == COMMAND_STATE.SUCCESS)
            {
                //画像を入れ替える
                nextCommandImageList[num].sprite = newCommandImageList[((int)COMMAND_STATE.SUCCESS * 4) + (int)item.Item1];
            }
            //ボタンの状態がミスなら
            if (item.Item2 == COMMAND_STATE.MISS)
            {
                //画像を入れ替える
                nextCommandImageList[num].sprite = newCommandImageList[((int)COMMAND_STATE.MISS * 4) + (int)item.Item1];
            }

            num++;
        }

        ////////////////改悪前////////////////////////////////
        //int num = 0;
        //foreach (COMMAND_TYPE item in nextCommand)
        //{
        //    if (item == COMMAND_TYPE.CROSS_BUTTON_UP)
        //    {
        //        nextCommandImageList[num].sprite = commandImageList[(int)COMMAND_TYPE.CROSS_BUTTON_UP];
        //    }
        //    if (item == COMMAND_TYPE.CROSS_BUTTON_DOWN)
        //    {
        //        nextCommandImageList[num].sprite = commandImageList[(int)COMMAND_TYPE.CROSS_BUTTON_DOWN];
        //    }
        //    if (item == COMMAND_TYPE.CROSS_BUTTON_LEFT)
        //    {
        //        nextCommandImageList[num].sprite = commandImageList[(int)COMMAND_TYPE.CROSS_BUTTON_LEFT];
        //    }
        //    if (item == COMMAND_TYPE.CROSS_BUTTON_RIGHT)
        //    {
        //        nextCommandImageList[num].sprite = commandImageList[(int)COMMAND_TYPE.CROSS_BUTTON_RIGHT];
        //    }
        //    num++;
        //}
    }

    // 当たった時に呼ばれる関数
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goal")
        {
            buttonCount = 0;
        }

        
    }

    //コマンドの状態を一定数に保つ処理
    public void KeepStateCommand(COMMAND_RESULT input)
    {
        bool isFirst = true;
        bool isAllSuccess = true;

        //直前にコマンド入力に成功していたら
        for (int i = 0; i < commandList.Count; i++)
        {
            if (input == COMMAND_RESULT.SUCCESS && commandList[i].Item2 != COMMAND_STATE.SUCCESS)
            {
                commandList[i] = new(commandList[i].Item1, COMMAND_STATE.SUCCESS);
                break;
            }
        }

        //コマンドの状態を変更
        for (int i = 0; i < commandList.Count; i++)
        {  
            if(commandList[i].Item2 == COMMAND_STATE.SUCCESS)
            {
            }
            else if(isMiss && isFirst)
            {
                //isMissを0.1秒後に解除する関数
                Invoke(nameof(SetFalseMiss), 0.1f);
                isFirst = false;
                isAllSuccess = false;
            }
            else if(input == COMMAND_RESULT.MISS && isFirst)
            {
                commandList[i] = new(commandList[i].Item1, COMMAND_STATE.MISS);
                isMiss = true;
                isFirst = false;
                isAllSuccess = false;
            }
            else if(isFirst)
            {
                commandList[i] = new(commandList[i].Item1, COMMAND_STATE.NEXT);
                isAllSuccess = false;
                isFirst = false;

                nextCommandButton = commandList[i].Item1;
            }
            else
            {
                commandList[i] = new(commandList[i].Item1, COMMAND_STATE.DEFAULT);
                isAllSuccess = false;
                isFirst = false;
            }
        }

        //コマンドが全部SUCCESSなら
        if (isAllSuccess)
        {
            //リストの中身を消す
            commandList.Clear();

            //コマンドを追加
            KeepCommand();

            //コマンドの状態を追加
            KeepStateCommand(COMMAND_RESULT.NONE);
        }
    }

    //セッター
    public void SetFalseMiss()
    {
        isMiss = false;
    }
}