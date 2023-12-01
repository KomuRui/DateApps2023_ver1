using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate2 : MonoBehaviour
{
    [SerializeField] private float speed;         // スピード
    [SerializeField] public int playerNum;       // プレイヤー番号

    private float power = 0.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //開始していないか終わっているのなら
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;

        //現在の力を取得
        float nowPower = Input.GetAxis("L_Stick_V" + +playerNum) * speed * Time.deltaTime;
        power += nowPower;
        power = Math.Min(0.08f, Math.Abs(power)) * Math.Sign(power);

        //力が加えられてないのなら減速する
        if (Input.GetAxis("L_Stick_V" + playerNum) == 0 && power != 0.0f) power *= 0.99f;

        transform.eulerAngles += new Vector3(0, power, 0);
        //範囲内におさめる
        if (transform.eulerAngles.y > 35 && transform.eulerAngles.y < 300)
        {
            transform.eulerAngles = new Vector3(0, 35, 0);
            power = 0.0f;
        }
        if (transform.eulerAngles.y < 305 && transform.eulerAngles.y > 40)
        {
            transform.eulerAngles = new Vector3(0, 305, 0);
            power = 0.0f;
        }
    }
}
