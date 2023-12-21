using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Canon : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10.0f;          // プレイヤーの移動速度
    [SerializeField] private float rotationSpeed = 180.0f;    // プレイヤーの回転速度
    [SerializeField] private bool isHorizontalInput = true;   // 横の入力許可するか
    [SerializeField] private bool isVerticalInput = false;     // 縦の入力許可するか
    Rigidbody rb;
    Vector3 movePos;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();  // rigidbodyを取得
        movePos = transform.right * 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.nowMiniGameManager.IsStart() && !GameManager.nowMiniGameManager.IsFinish())
        {
            //動き
            Move();
        }


        //if (Input.GetButtonDown("Abutton" + this.GetComponent<PlayerNum>().playerNum) && isAttack)
        //{
        //    isAttack = false;
        //    HammerOb.transform.DORotate(new Vector3(AttackRotate.x, -this.transform.localEulerAngles.y, -AttackRotate.z), 0.5f).SetEase(Ease.InBack);

        //    //1.5秒後にあげる
        //    Invoke("HammerUp", 0.5f);
        //    Invoke("HammerAttack", 2.0f);
        //}
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

        //歩き状態に変更
        //ChangeStateTo(SlimeAnimationState.Walk);

        // カメラの向きを基準にプレイヤーを移動
        //Vector3 forwardDirection = mainCameraTransform.forward;
        //Vector3 rightDirection = mainCameraTransform.right;
        //forwardDirection.y = 0f; // Y軸成分を0にすることで水平方向に制限

        // 移動方向を計算
        //Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        // 移動
        //transform.position += moveDirection * moveSpeed * Time.deltaTime;
        //rb.AddForce(moveDirection * moveSpeed * Time.deltaTime);
        if(horizontalInput > 0)
        {
            transform.position = new Vector3(transform.position.x + transform.right.x * moveSpeed, transform.position.y + transform.right.y * moveSpeed, transform.position.z + transform.right.z * moveSpeed);
            //rb.AddForce(transform.right * moveSpeed, ForceMode.Force);
            //transform.position = new Vector3(transform.position.x + movePos.x, transform.position.y + movePos.y, transform.position.z + movePos.z);
        }
        if(horizontalInput < 0)
        {
            transform.position = new Vector3(transform.position.x - transform.right.x * moveSpeed, transform.position.y - transform.right.y * moveSpeed, transform.position.z - transform.right.z * moveSpeed);
            //rb.AddForce(-transform.right * moveSpeed, ForceMode.Force);
        }

        //rb.velocity = -moveDirection;

        //transform.position.x = Math.Clamp(transform.position.x, -3.5f, 3.5f);

        //Quaternion newRotation = Quaternion.LookRotation(moveDirection);
        //transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "RightWall")
        {
            Debug.Log("当たった!");
            transform.Rotate(0, -90, 0);
            //this.transform.position = new Vector3(5.3f, this.transform.position.y, this.transform.position.z);
        }

        if (collision.gameObject.tag == "LeftWall")
        {
            transform.Rotate(0, 90, 0);
        }
    }
}
