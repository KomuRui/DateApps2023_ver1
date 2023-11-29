using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSurfboardPlayer : MonoBehaviour
{
    [SerializeField] private float rotateSpeed  = 0.8f;                   //傾く速さ
    [SerializeField] private int playerNum;                   // プレイヤー番号
    [SerializeField] private List<SurfboardPlayer> threePlayer;                //3人側のオブジェクト

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //急いでいるためコメント一時的にない
        if (GameManager.nowMiniGameManager.IsStart() && !GameManager.nowMiniGameManager.IsFinish()) 
        {
            Move();
        }
    }

    public void Move()
    {
        // 入力を取得用
        float horizontalInput = 0;
        float verticalInput = 0;

        // 入力を取得
        horizontalInput = Input.GetAxis("L_Stick_H" + playerNum) * rotateSpeed;
        verticalInput = -Input.GetAxis("L_Stick_V" + playerNum) * rotateSpeed;

        foreach (var item in threePlayer)
        {
            if (item.sumRotateX + horizontalInput <= item.LIMIT_ROTATE && item.sumRotateX + horizontalInput >= -item.LIMIT_ROTATE && !item.isDead)
            {
                item.sumRotateX += horizontalInput;

                //スティックによって回転
                item.transform.Rotate(new Vector3(0, horizontalInput, 0));
            }
        }
    }
}
