using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class SurfboardPlayer : MonoBehaviour
{
    [SerializeField] private int playerNum;                   // プレイヤー番号
    [SerializeField] private float LIMIT_ROTATE = 45.0f;      //回転の限界
    private Rigidbody rb;
    private Vector3 pos;
    private Vector3 startRotate;    //初期の向き
    Quaternion rot;

    public float rotationSpeed = 10.0f;
    private float sumRotateX = 0.0f;
    private float sumRotateY = 0.0f;

    private float hp = 0.0f;


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

        if (sumRotateX + horizontalInput <= LIMIT_ROTATE && sumRotateX + horizontalInput >= -LIMIT_ROTATE)
        {
            sumRotateX += horizontalInput;

            //スティックによって回転
            transform.Rotate(new Vector3(0, horizontalInput, 0));
        }

        //if (sumRotateY + verticalInput <= LIMIT_ROTATE && sumRotateY + verticalInput >= -LIMIT_ROTATE)
        //{
        //    sumRotateY += verticalInput;

        //    //スティックによって回転
        //    transform.Rotate(new Vector3(verticalInput, 0, 0));
        //}
    }
}
