using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class SurfboardPlayer : MonoBehaviour
{
    [SerializeField] private int playerNum;                   // プレイヤー番号
    private Rigidbody rb;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
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

        transform.Rotate(new Vector3(0, horizontalInput, 0));
    }
}
