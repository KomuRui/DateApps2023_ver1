using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSurfboardPlayer : MonoBehaviour
{
    [SerializeField] private float rotateSpeed  = 0.3f;                   //傾く速さ
    [SerializeField] private float kajiRotateSpeed = 32.0f;                   //1秒間で舵が傾く角度
    [SerializeField] private int playerNum;                   // プレイヤー番号
    [SerializeField] private GameObject kaji;

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

        // 入力を取得
        horizontalInput = Input.GetAxis("L_Stick_H" + this.GetComponent<PlayerNum>().playerNum) * rotateSpeed;

        //舵を動かす
        KajiMove(horizontalInput);

        foreach (var item in GameManager.nowMiniGameManager.threePlayerObj)
        {
            if(item == null || item.transform.parent == null || item.transform.parent.gameObject.GetComponent<SurfboardPlayer>() == null) continue;
            
            SurfboardPlayer p = item.transform.parent.gameObject.GetComponent<SurfboardPlayer>();

            if (p.sumRotateX + horizontalInput <= p.LIMIT_ROTATE && p.sumRotateX + horizontalInput >= -p.LIMIT_ROTATE && !p.isDead)
            {
                p.sumRotateX += horizontalInput;

                //スティックによって回転
                p.transform.Rotate(new Vector3(0, horizontalInput, 0));
            }
        }
    }

    //舵を動かすプログラム
    public void KajiMove(float input)
    {
        float isPlus = 1;
        if (input < 0)
        {
            isPlus *= -1;
        }
        if (input != 0) 
        {
            //舵を回転させる
            kaji.transform.Rotate(kajiRotateSpeed * Time.deltaTime * 10 * isPlus, 0, 0);
        }
    }
}
