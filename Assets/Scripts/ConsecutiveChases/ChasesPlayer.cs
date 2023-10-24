using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
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

    [SerializeField] private Vector3 moveDirection = new Vector3(0.0f, 0.0f, 1.0f); //プレイヤーの移動方向

    private Material faceMaterial;

    [SerializeField] private float deceleration = 150.0f;       //減速率
    [SerializeField] private float addSpeed = 1.1f;             // ボタンを押したときプレイヤーの移動速度の上昇値
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
    [SerializeField] private float COMMAND_SIZE_MAX = 3;              //次のコマンドのリストの最大数
    [SerializeField] private int playerNum;                           // プレイヤー番号
    [SerializeField] private Queue<COMMAND_TYPE> nextCommand = new();                    //次のコマンドのキュー
    [SerializeField] private List<Image> nextCommandImageList = new List<Image>(); //次のコマンドの画像を表示する場所のリスト
    [SerializeField] private List<Sprite> commandImageList = new List<Sprite>(); //コマンドの画像のリスト（何の画像を使うか）
    private bool crossKeyContinuous = false;    //十字キー

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

        //コマンド画像を入れる
        SetCommandImage();

        //nextCommand.Enqueue(COMMAND_TYPE.CROSS_BUTTON_UP);
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
        //コマンド入力が成功していたら
        if (CheckOnCommandButton() == COMMAND_RESULT.SUCCESS)
        {
            buttonCount += addSpeed;

            //コマンドが成功した場合の処理
            SuccessCommand();
        }
        if (CheckOnCommandButton() == COMMAND_RESULT.MISS)
        {
            int a = 0;
            a += 1;
        }

        //速度が0ならば
        if (moveSpeed <= 0)
        {
            //通常状態に変更
            ChangeStateTo(SlimeAnimationState.Idle);
        }
        else
        {
            //歩き状態に変更
            ChangeStateTo(SlimeAnimationState.Walk);

            // 移動
            //アニメーションの速度に合わせるために遅くする
            Vector3 animationSpeed = new Vector3(0.0f, 0.0f, 0.002f);
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.position -= animationSpeed;
        }

        buttonCount -= addSpeed / deceleration;
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
        //Aボタンが押されてないのならこの先処理しない
        if (!Input.GetButtonDown("Abutton1")) return;

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

    //ランダムで今あるコマンド選択
    public COMMAND_TYPE RandCommand()
    {
       return (COMMAND_TYPE)Random.Range((int)COMMAND_TYPE.CROSS_BUTTON_UP, 4);
    }

    //コマンドが成功した場合の処理
    public void SuccessCommand()
    {
        //コマンドの削除
        nextCommand.Dequeue();

        //コマンドを一定数に保つ処理
        KeepCommand();

        //コマンド画像を入れる
        SetCommandImage();
    }

    //コマンドを一定数に保つ処理
    public void KeepCommand()
    {
        //コマンドの数が減っている時
        for (int i = nextCommand.Count; i < COMMAND_SIZE_MAX; i++)
        {
            //ランダムにコマンドを入れる
            nextCommand.Enqueue(RandCommand());
        }
    }

    //次のコマンドのボタンが押されたかどうか調べる
    public COMMAND_RESULT CheckOnCommandButton()
    {
        //十字キーの入力を受け取る
        crossAxisV = Input.GetAxis("D_Pad_V" + playerNum);
        crossAxisH = Input.GetAxis("D_Pad_H" + playerNum);

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
            else if (nextCommand.Peek() == priorityCommand)
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


    //コマンド画像を入れる
    public void SetCommandImage()
    {
        int num = 0;
        foreach (COMMAND_TYPE item in nextCommand)
        {
            if (item == COMMAND_TYPE.CROSS_BUTTON_UP)
            {
                nextCommandImageList[num].sprite = commandImageList[(int)COMMAND_TYPE.CROSS_BUTTON_UP];
            }
            if (item == COMMAND_TYPE.CROSS_BUTTON_DOWN)
            {
                nextCommandImageList[num].sprite = commandImageList[(int)COMMAND_TYPE.CROSS_BUTTON_DOWN];
            }
            if (item == COMMAND_TYPE.CROSS_BUTTON_LEFT)
            {
                nextCommandImageList[num].sprite = commandImageList[(int)COMMAND_TYPE.CROSS_BUTTON_LEFT];
            }
            if (item == COMMAND_TYPE.CROSS_BUTTON_RIGHT)
            {
                nextCommandImageList[num].sprite = commandImageList[(int)COMMAND_TYPE.CROSS_BUTTON_RIGHT];
            }
            num++;
        }
    }

}