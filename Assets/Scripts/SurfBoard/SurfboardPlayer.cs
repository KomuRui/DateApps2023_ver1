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
    Quaternion rot;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        rot = this.transform.rotation;
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

        //スティックによって回転
        transform.Rotate(new Vector3(0, horizontalInput, 0));
        transform.Rotate(new Vector3(verticalInput , 0, 0));

        if (transform.localEulerAngles.x >= 0)        
        {
            int a = 0;
        }
    }
}
