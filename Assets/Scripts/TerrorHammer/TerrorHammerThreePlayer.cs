using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class TerrorHammerThreePlayer : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;          // プレイヤーの移動速度
    [SerializeField] private float rotationSpeed = 180.0f;    // プレイヤーの回転速度
    [SerializeField] private bool isHorizontalInput = true;   // 横の入力許可するか
    [SerializeField] private bool isVerticalInput = true;     // 縦の入力許可するか
    [SerializeField] public float startPosX;     // スタート
    [SerializeField] public float checkPosX;     // チェック
    [SerializeField] private float nowPosX;     // プレイヤー
    [SerializeField] private int point;     // プレイヤー
    [SerializeField] public TextMeshProUGUI pointText;       //点数テキスト
    [SerializeField] private GameObject HammerOb;  // ハンマー
    
    private Vector3 initializeRotate;
    private Vector3 AttackRotate;
    private bool isAttack;
    private bool isSuper;
    

    private Rigidbody rBody;
    private Transform mainCameraTransform; // メインカメラのTransform

    // Start is called before the first frame update
    void Start()
    {
        nowPosX = startPosX;
        point = 0;
        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;

        //リジットボディ取得
        rBody = this.GetComponent<Rigidbody>();

        //初期
        initializeRotate = new Vector3(90, 0, 180);
        AttackRotate = new Vector3(0, 0, 180);
        isAttack = true;
        isSuper = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.nowMiniGameManager.IsFinish() && point < 1 && this.transform.localScale.y > 0.5f)
            //動き
            Move();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.7f, 4.7f),transform.position.y, Mathf.Clamp(transform.position.z, -1.0f, 1.0f));//1.7f

        //チェック
        if(nowPosX == startPosX && this.transform.position.x > checkPosX)
        {
            nowPosX = checkPosX;
        }

        if (nowPosX == checkPosX && this.transform.position.x < startPosX)
        {
            nowPosX = startPosX;
            point++;
            pointText.SetText(point.ToString());
            Debug.Log(point);
        }

        //攻撃
        if (Input.GetButtonDown("Abutton" + this.GetComponent<PlayerNum>().playerNum) && isAttack)
        {
            isAttack = false;
            HammerOb.transform.DORotate(AttackRotate, 0.5f).SetEase(Ease.InBack);

            //1.5秒後にあげる
            Invoke("HammerUp", 0.5f);
            Invoke("HammerAttack", 2.0f);
        }
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
            //ChangeStateTo(SlimeAnimationState.Idle);
            return;
        }
        
        // カメラの向きを基準にプレイヤーを移動
        Vector3 forwardDirection = mainCameraTransform.forward;
        Vector3 rightDirection = mainCameraTransform.right;
        forwardDirection.y = 0f; // Y軸成分を0にすることで水平方向に制限

        // 移動方向を計算
        Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        // 移動
        rBody.AddForce(moveDirection * moveSpeed * Time.deltaTime);

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    public void HammerUp()
    {
        HammerOb.transform.DORotate(new Vector3 (initializeRotate.x, initializeRotate.y,initializeRotate.z), 0.5f).SetEase(Ease.InQuad);
    }

    public void HammerAttack()
    {
        isAttack = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerHammer"/* && isSuper == false*/)
        {
            Debug.Log("つぶれた!");

            this.transform.localScale = new Vector3(1, 0.3f, 1);
            isSuper = true;
            Invoke("MovePlayer", 2f);
            Invoke("WeakPlayer", 3f);

            // ミニゲームに死んだことを伝える
            //GameManager.nowMiniGameManager.PlayerDead(this.transform.GetChild(1).GetComponent<PlayerNum>().playerNum);
            //GameManager.nowMiniGameManager.PlayerFinish(this.transform.GetChild(1).GetComponent<PlayerNum>().playerNum);

            //オブジェクトを削除
            //Destroy(this.gameObject);
        }
    }

    void WeakPlayer()
    {
        isSuper = false;
    }

    void MovePlayer()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
    }
}