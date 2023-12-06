using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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


    private Transform mainCameraTransform; // メインカメラのTransform

    // Start is called before the first frame update
    void Start()
    {
        nowPosX = startPosX;
        point = 0;
        // メインカメラを取得
        mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.nowMiniGameManager.IsFinish() && point < 3)
            //動き
            Move();

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.7f, 4.7f),transform.position.y, Mathf.Clamp(transform.position.z, -1.7f, 1.7f));

        //チェック
        if(nowPosX == startPosX && this.transform.position.x > checkPosX)
        {
            nowPosX = checkPosX;
        }

        if (nowPosX == checkPosX && this.transform.position.x < startPosX)
        {
            nowPosX = startPosX;
            point++;
            Debug.Log(point);
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
        transform.position += moveDirection * moveSpeed * Time.deltaTime;


        //transform.position.x = Math.Clamp(transform.position.x, -3.5f, 3.5f);

        Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.tag == "Hammer")
        {
            Debug.Log("当たった!");
            // ミニゲームに死んだことを伝える
            GameManager.nowMiniGameManager.PlayerDead(this.GetComponent<PlayerNum>().playerNum);
            GameManager.nowMiniGameManager.PlayerFinish(this.GetComponent<PlayerNum>().playerNum);

            //オブジェクトを削除
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
