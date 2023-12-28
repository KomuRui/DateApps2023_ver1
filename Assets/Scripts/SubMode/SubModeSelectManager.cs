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

    //画像
    [SerializeField] private List<SubModeImageInfo> image;

    //プレイヤーの初期選択画像
    [SerializeField] private List<SubModeImageInfo> playerInitializSelectImafe;

    //プレイヤーの色
    [SerializeField] public List<Color> playerColor;

    //プレイヤーが選択している画像を大きく表示するやつ
    [SerializeField] public List<Image> playerSelectImageBig;

    //フェード用
    [SerializeField] private Fade fade;                     

    //情報保管しているところ
    private Dictionary<byte, SubModeImageInfo> playerSelectImage = new Dictionary<byte, SubModeImageInfo>();
    private Dictionary<byte, InputInfo> inputXY = new Dictionary<byte, InputInfo>();
    private Dictionary<byte, bool> isPlayerSelect = new Dictionary<byte, bool>();

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
            isPlayerSelect[(byte)(i + 1)] = false;
        }

        //フェードが情報あるのなら
        if (fade)
            fade.FadeOut(1.0f);


        /////////////////////////////////////Test////////////////////////////////////////////
        
        isPlayerSelect[2] = true;
        isPlayerSelect[3] = true;
        isPlayerSelect[4] = true;

        //OK画像を表示
        playerSelectImageBig[1].transform.GetChild(0).gameObject.SetActive(true);
        playerSelectImageBig[1].transform.GetChild(1).gameObject.SetActive(true);
        playerSelectImageBig[2].transform.GetChild(0).gameObject.SetActive(true);
        playerSelectImageBig[2].transform.GetChild(1).gameObject.SetActive(true);
        playerSelectImageBig[3].transform.GetChild(0).gameObject.SetActive(true);
        playerSelectImageBig[3].transform.GetChild(1).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
        {
            //今回の入力値を取得
            inputXY[i].nowInputX = Input.GetAxis("L_Stick_H" + i);
            inputXY[i].nowInputY = Input.GetAxis("L_Stick_V" + i);

            SelectImageChange(i);  //選択画像変更
            SelectImageDecide(i);  //選択画像決定
            SelectImageUnlock(i);  //選択画像解除


            //今回の入力値を保存
            inputXY[i].beforeInputX = inputXY[i].nowInputX;
            inputXY[i].beforeInputY = inputXY[i].nowInputY;
        }

    }

    //選択画像変更
    private void SelectImageChange(byte playerNum)
    {
        //選択中ならこの先処理しない
        if (isPlayerSelect[playerNum]) return;

        //変更ができたかチェック
        SubModeImageInfo info = playerSelectImage[playerNum].SelectImageChange(playerNum, inputXY);
        if (info)
        {
            //変更できたのなら各画像の情報を変更
            playerSelectImage[playerNum].playerSelectMyNum.Remove(playerNum);
            info.playerSelectMyNum.Add(playerNum);
            playerSelectImage[playerNum].ImageColorChange();
            info.ImageColorChange();
            playerSelectImage[playerNum] = info;

            //大きい画像も変更
            playerSelectImageBig[playerNum - 1].sprite = playerSelectImage[playerNum].GetComponent<Image>().sprite;
        }
    }

    //選択画像決定
    private void SelectImageDecide(byte playerNum)
    {
        //決定
        if (Input.GetButtonDown("Abutton" + playerNum) && !isPlayerSelect[playerNum])
        {
            isPlayerSelect[playerNum] = true;

            //OK画像を表示
            playerSelectImageBig[playerNum - 1].transform.GetChild(0).gameObject.SetActive(true);
            playerSelectImageBig[playerNum - 1].transform.GetChild(1).gameObject.SetActive(true);

            //全員OKしたら画面外に移動させる
            if (isAllPlayerOK()) imageMoveScreenOut();
        }

    }

    //選択画像解除
    private void SelectImageUnlock(byte playerNum)
    {
        //解除
        if (Input.GetButtonDown("Bbutton" + playerNum) && isPlayerSelect[playerNum])
        {
            isPlayerSelect[playerNum] = false;

            //OK画像を非表示
            playerSelectImageBig[playerNum - 1].transform.GetChild(0).gameObject.SetActive(false);
            playerSelectImageBig[playerNum - 1].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    //全員準備できたか確認
    private bool isAllPlayerOK()
    {
        for (byte i = 1; i < PlayerManager.PLAYER_MAX; i++)
        {
            //選択していないかアニメーションの最中ならfalseを返す
            if (!isPlayerSelect[i]) return false;
        }

        return true;
    }

    //画像画面外に移動
    private void imageMoveScreenOut()
    {
        foreach (var i in image)
            i.ImageMoveScreenOut();
    }
}
