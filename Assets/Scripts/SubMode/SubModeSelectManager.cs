using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubModeSelectManager : MonoBehaviour
{
    //入力情報
    public class InputInfo
    {
        public float nowInputX;
        public float nowInputY;
        public float beforeInputX;
        public float beforeInputY;
    }

    //プレイヤーの初期選択画像
    [SerializeField] private List<SubModeImageInfo> playerInitializSelectImafe;

    //プレイヤーの色
    [SerializeField] public List<Color> playerColor;

    //情報保管しているところ
    private Dictionary<byte, SubModeImageInfo> playerSelectImage = new Dictionary<byte, SubModeImageInfo>();
    private Dictionary<byte, InputInfo> inputXY = new Dictionary<byte, InputInfo>();

    void Start()
    {
        //初期化
        for (byte i = 0; i < PlayerManager.PLAYER_MAX; i++)
        {
            //入力値を初期化
            InputInfo input = new InputInfo();
            input.nowInputX = 0;
            input.nowInputY = 0;
            input.beforeInputX = 0;
            input.beforeInputY = 0;
            inputXY[(byte)(i + 1)] = input;

            //プレイヤーの初期選択画像を初期化
            playerSelectImage[(byte)(i + 1)] = playerInitializSelectImafe[i];
            playerSelectImage[(byte)(i + 1)].playerSelectMyNum.Add((byte)(i + 1));
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
        {
            //今回の入力値を取得
            inputXY[i].nowInputX = Input.GetAxis("L_Stick_H" + i);
            inputXY[i].nowInputY = Input.GetAxis("L_Stick_V" + i);

            //変更ができたかチェック
            SubModeImageInfo info = playerSelectImage[i].SelectImageChange(i, inputXY);
            if (info) playerSelectImage[i] = info;

            //今回の入力値を保存
            inputXY[i].beforeInputX = inputXY[i].nowInputX;
            inputXY[i].beforeInputY = inputXY[i].nowInputY;
        }
    }
}
