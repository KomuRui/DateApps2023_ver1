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
    
    private Quaternion initializeRotate;
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
        initializeRotate = HammerOb.transform.rotation;
        AttackRotate = new Vector3(0, 0, 180);
        isAttack = false;
        isSuper = false;
    }

    // Update is called once per frame
    void Update()
    {
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
            ((TerrorHammerGameManager)GameManager.nowMiniGameManager).goalPlayer.Add(this.GetComponent<PlayerNum>().playerNum);

            if (((TerrorHammerGameManager)GameManager.nowMiniGameManager).goalPlayer.Count >= 3)
                ((TerrorHammerGameManager)GameManager.nowMiniGameManager).SetMiniGameFinish();
        }

        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish() || point >= 1 || this.transform.localScale.y <= 0.5f)
            return;
        
        //動き
        Move();

        //攻撃
        if (Input.GetButtonDown("Abutton" + this.GetComponent<PlayerNum>().playerNum) && !HammerOb.GetComponent<ThreePlayerHammer>().isAttack)
        {
            HammerOb.GetComponent<ThreePlayerHammer>().Attack();
            //1.5秒後にあげる
            Invoke("HammerUp", 0.5f);
        }
    }

    //移動
    private void Move()
    {
        //攻撃中ならこの先処理しない
        if (HammerOb.GetComponent<ThreePlayerHammer>().isAttack) return;

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
        HammerOb.GetComponent<ThreePlayerHammer>().Return();
        //HammerOb.transform.DORotateQuaternion(initializeRotate, 0.5f).SetEase(Ease.InQuad).OnComplete(() => isAttack = false) ;
    }


    //プレイヤーのハンマーにヒット
    public void HitPlayerHammer()
    {
        if (isSuper) return;

        this.transform.localScale = new Vector3(1, 0.3f, 1);
        isSuper = true;
        Invoke("MovePlayer", 2f);
        Invoke("WeakPlayer", 3f);
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "PlayerHammer" && collision.gameObject.GetComponent<TerrorHammerThreePlayer>().isAttack)
        {
           
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Hammer"/* && isSuper == false*/)
        {
            this.transform.localScale = new Vector3(1, 0.3f, 1);
            isSuper = true;
            GameManager.nowMiniGameManager.PlayerDead(this.GetComponent<PlayerNum>().playerNum);
            GameManager.nowMiniGameManager.PlayerFinish(this.GetComponent<PlayerNum>().playerNum);
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