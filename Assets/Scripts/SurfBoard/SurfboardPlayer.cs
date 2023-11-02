using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class SurfboardPlayer : MonoBehaviour
{
    [SerializeField] private int playerNum;                   // プレイヤー番号
    [SerializeField] private float LIMIT_ROTATE = 90.0f;      //回転の限界
    private Rigidbody rb;
    private Vector3 pos;
    private Vector3 startRotate;    //初期の向き
    Quaternion rot;

    public float rotationSpeed = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rot = this.transform.rotation;

        startRotate = transform.localEulerAngles;
    }

    // Update is called once per frame
    void Update()
    {
        // 入力を取得用
        float horizontalInput = 0;
        float verticalInput = 0;

        // 入力を取得
        horizontalInput = Input.GetAxis("L_Stick_H" + playerNum);
        verticalInput = -Input.GetAxis("L_Stick_V" + playerNum);

        // 現在の回転角度を取得
        Vector3 currentRotation = transform.localEulerAngles;

        // 角度が-180〜180の範囲内になるように補正
        if (currentRotation.x > 180)
        {
            currentRotation.x = currentRotation.x - 360;
        }

        // 回転に制限をかける
        currentRotation.x += horizontalInput * rotationSpeed;
        currentRotation.x = Mathf.Clamp(currentRotation.x, -LIMIT_ROTATE, LIMIT_ROTATE);

        // オブジェクトを回転
        transform.localEulerAngles = new Vector3( currentRotation.x, -90, 0);

        //スティックによって回転
        //transform.Rotate(new Vector3(0, horizontalInput, 0));
        //transform.Rotate(new Vector3(verticalInput , 0, 0));

    }
}
