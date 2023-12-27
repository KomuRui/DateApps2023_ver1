using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    //プレイヤーが選択している画像を大きく表示するやつ
    [SerializeField] public List<Image> playerSelectImageBig;

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
            playerSelectImage[(byte)(i + 1)].ImageColorChange();
            playerSelectImageBig[i].sprite = playerInitializSelectImafe[i].GetComponent<Image>().sprite;
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
            if (info)
            {
                //変更できたのなら各画像の情報を変更
                playerSelectImage[i].playerSelectMyNum.Remove(i);
                info.playerSelectMyNum.Add(i);
                playerSelectImage[i].ImageColorChange();
                info.ImageColorChange();
                playerSelectImage[i] = info;

                //大きい画像も変更
                playerSelectImageBig[i - 1].sprite = playerSelectImage[i].GetComponent<Image>().sprite;
            }

            //今回の入力値を保存
            inputXY[i].beforeInputX = inputXY[i].nowInputX;
            inputXY[i].beforeInputY = inputXY[i].nowInputY;
        }
    }
}
