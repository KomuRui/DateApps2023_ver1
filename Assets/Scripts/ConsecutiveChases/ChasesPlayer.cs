using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private int playerNum;                           // プレイヤー番号
    [SerializeField] private float crossKeyDeadzone = 0.5f;           // 十字キーのデッドゾーン
    [SerializeField] private List<COMMAND_TYPE> nextCommand = new List<COMMAND_TYPE>(); //次のコマンドのリスト
    [SerializeField] private List<Image> nextCommandImageList = new List<Image>(); //次のコマンドの画像を表示する場所のリスト
    [SerializeField] private List<Sprite> commandImageList = new List<Sprite>(); //コマンドの画像のリスト（何の画像を使うか）


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
        CheckOnCommandButton();

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
        return (COMMAND_TYPE)Random.Range(0f, ((int)COMMAND_TYPE.COMMAND_MAX));
    }

    //次のコマンドのボタンが押されたかどうか調べる
    public bool CheckOnCommandButton()
    {

        //十字キーの入力を受け取る
        float crossAxisV = Input.GetAxis("D_Pad_V" + playerNum);
        float crossAxisH = Input.GetAxis("D_Pad_H" + playerNum);

        COMMAND_TYPE priorityCommand = COMMAND_TYPE.NONE;
        float priorityLevel;

        nextCommand.Add(COMMAND_TYPE.CROSS_BUTTON_UP);
        //コマンドチェック
        if (nextCommand[0] == COMMAND_TYPE.CROSS_BUTTON_UP && Input.GetAxis("D_Pad_V" + playerNum) > 0 + crossKeyDeadzone)
        {
            Debug.Log("上");
            return true;
        }
        nextCommand.RemoveAt(0);

        nextCommand.Add(COMMAND_TYPE.CROSS_BUTTON_DOWN);
        if (nextCommand[0] == COMMAND_TYPE.CROSS_BUTTON_DOWN && Input.GetAxis("D_Pad_V" + playerNum) < 0 - crossKeyDeadzone)
        {
            Debug.Log("下");
            return true;
        }
        nextCommand.RemoveAt(0);

        nextCommand.Add(COMMAND_TYPE.CROSS_BUTTON_LEFT);
        if (nextCommand[0] == COMMAND_TYPE.CROSS_BUTTON_LEFT && Input.GetAxis("D_Pad_H" + playerNum) < 0 - crossKeyDeadzone)
        {
            Debug.Log("左");
            return true;
        }
        nextCommand.RemoveAt(0);

        nextCommand.Add(COMMAND_TYPE.CROSS_BUTTON_RIGHT);
        if (nextCommand[0] == COMMAND_TYPE.CROSS_BUTTON_RIGHT && Input.GetAxis("D_Pad_H" + playerNum) > 0 + crossKeyDeadzone)
        {
            Debug.Log("右");
            return true;
        }
        nextCommand.RemoveAt(0);
        return false;
    }
}