using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate1 : MonoBehaviour
{

    [SerializeField] private float speed = 1.0f;  // スピード
    [SerializeField] private int playerNum;       // プレイヤー番号

    private float power = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //現在の力を取得
        power += -Input.GetAxis("L_Stick_V1") * speed * Time.deltaTime;
        power = Math.Min(0.09f, Math.Abs(power)) * Math.Sign(power);

        //力が加えられてないのなら減速する
        if (Input.GetAxis("L_Stick_V1") == 0 && power != 0) power *= 0.997f;

        //角度を加算し、範囲内に収める
        transform.eulerAngles += new Vector3(0, power, 0);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x,Math.Clamp(transform.eulerAngles.y, -35, 35), transform.eulerAngles.z);
    }
}
