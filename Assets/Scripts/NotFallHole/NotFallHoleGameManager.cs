using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotFallHoleGameManager : MonoBehaviour
{

    //プレイヤー番号
    [SerializeField] private int playerNum = 1;

    //回転に使う符号
    public int rotateSign;

    // Start is called before the first frame update
    void Start()
    {
        rotateSign = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("LBbutton" + playerNum)) rotateSign *= -1;
        if (Input.GetButtonDown("RBbutton" + playerNum)) rotateSign *= -1;
    }
}
