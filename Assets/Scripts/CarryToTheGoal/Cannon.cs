using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class LookOnTexture : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 5.0f;          // プレイヤーの移動速度
    [SerializeField] private GameObject Object;
    [SerializeField] private CarryBullet bullet;
    [SerializeField] private bool isHorizontalInput = true;   // 横の入力許可するか
    [SerializeField] private bool isVerticalInput = true;     // 縦の入力許可するか
    [SerializeField] private int playerNum;                   // プレイヤー番号

    private Vector3 beforePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //発射
        Shoot();

        //移動
        Move();

        //テクスチャ張り付けるためレイキャストする
        TextureRayCast();
    }

    //発射
    private void Shoot()
    {
        if(Input.GetButtonDown("Abutton" + playerNum))
            Instantiate(bullet, transform.position, Quaternion.identity);
    }

    //レイキャスト
    private void TextureRayCast()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, Vector3.down); // Rayを生成

        if (Physics.Raycast(ray, out hit, 10000))
        {
            if (hit.collider.gameObject.tag == "CarryStage")
                Object.transform.position = hit.point + new Vector3(0,1,0);
            else if (hit.collider.gameObject.tag == "Sea")
                transform.position = beforePos;
        }

    }

    //移動
    private void Move()
    {
        //移動前のポジションを覚えておく
        beforePos = transform.position;

        // 入力を取得用
        float horizontalInput = 0;
        float verticalInput = 0;

        // 入力を取得
        if (isHorizontalInput) horizontalInput = Input.GetAxis("L_Stick_H" + playerNum);
        if (isVerticalInput) verticalInput = -Input.GetAxis("L_Stick_V" + playerNum);

        //入力がないのなら
        if (horizontalInput == 0 && verticalInput == 0)
            return;


        // カメラの向きを基準にプレイヤーを移動
        Vector3 forwardDirection = Camera.main.transform.forward;
        Vector3 rightDirection = Camera.main.transform.right;
        forwardDirection.y = 0f; // Y軸成分を0にすることで水平方向に制限

        // 移動方向を計算
        Vector3 moveDirection = (forwardDirection.normalized * verticalInput + rightDirection.normalized * horizontalInput).normalized;

        // 移動
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
    }
}
