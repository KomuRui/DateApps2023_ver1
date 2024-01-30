using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharaSelectManager : MonoBehaviour
{
    //ライン数
    public enum LineNum
    {
        ONE = 1,
        TWO = 2,
        THREE = 3,
        MAX_LINE
    }

    //宝庫
    public enum Direction
    {
        RIGHT,
        LEFT,
        UP,
        DOWN,
        MAX_DIRECTION
    }

    //プレイヤー情報
    class PlayerInfo
    {
        public Color selectColor;                             //選択色
        public CharaSelectOutlineInfo charaSelectOutlineInfo; //キャラ選択アウトライン情報
        public GameObject barunn;                             //浮輪
        public Image playerImage;                             //プレイヤー画像
        public LineNum line;                                  //どのラインか
        public int num;                                       //何番目か
        public bool isSelect;                                 //選択しているかどうか
    }

    //入力情報
    class InputInfo
    {
        public float nowInputX;
        public float nowInputY;
        public float beforeInputX;
        public float beforeInputY;
    }

    //フェード
    [SerializeField] private Fade fade;

    //Playerに必要な情報
    [SerializeField] private List<CharaSelectOutlineInfo> line1Chara;
    [SerializeField] private List<CharaSelectOutlineInfo> line2Chara;
    [SerializeField] private List<CharaSelectOutlineInfo> line3Chara;
    [SerializeField] private List<Color> playerColor;
    [SerializeField] private List<CharaSelectOutlineInfo> playerInitializOutlineInfo;
    [SerializeField] private List<GameObject> playerUkiwa;
    [SerializeField] private List<Image> playerImage;
    [SerializeField] private List<GameObject> playerOKImage;

    //情報保管しているところ
    private Dictionary<byte, List<CharaSelectOutlineInfo>> lineCharaTable = new Dictionary<byte, List<CharaSelectOutlineInfo>>();
    private Dictionary<byte, PlayerInfo> playerInfo = new Dictionary<byte, PlayerInfo>();
    private Dictionary<byte, InputInfo> inputXY = new Dictionary<byte, InputInfo>();

    private bool isSceneChange = false;

    // Start is called before the first frame update
    void Start()
    {

        //各ライン初期化
        lineCharaTable[(byte)LineNum.ONE] = line1Chara;
        lineCharaTable[(byte)LineNum.TWO] = line2Chara;
        lineCharaTable[(byte)LineNum.THREE] = line3Chara;

        //各情報を設定
        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
        {
            //入力値を初期化
            InputInfo input = new InputInfo();
            input.nowInputX = 0;
            input.nowInputY = 0;
            input.beforeInputX = 0;
            input.beforeInputY = 0;
            inputXY[(byte)(i + 1)] = input;

            //プレイヤー情報
            PlayerInfo info = new PlayerInfo();
            info.isSelect = false;
            info.selectColor = playerColor[i];
            info.charaSelectOutlineInfo = playerInitializOutlineInfo[i];
            info.barunn = playerUkiwa[i];
            info.playerImage = playerImage[i];
            info.line = info.charaSelectOutlineInfo.line;
            info.num = info.charaSelectOutlineInfo.num;
            info.charaSelectOutlineInfo.SetSelect((byte)(i + 1), playerColor[i]);
            playerInfo[(byte)(i + 1)] = info;
        }

        PlayerManager.Initializ();

        //フェードが情報あるのなら
        if (fade)
            fade.FadeOut(1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
        {
            //今回の入力値を取得
            inputXY[i].nowInputX = Input.GetAxis("L_Stick_H" + i);
            inputXY[i].nowInputY = Input.GetAxis("L_Stick_V" + i);

            //キャラ変更・決定・解除
            CharaChange(i);
            CharaDecide(i);
            CharaUnlock(i);

            //今回の入力値を保存
            inputXY[i].beforeInputX = inputXY[i].nowInputX;
            inputXY[i].beforeInputY = inputXY[i].nowInputY;
        }

        //もし全員準備できたのならフェードイン
        if (isAllPlayerOK() && !isSceneChange)
        {
            //プレイヤーマネージャーに各プレイヤーの情報を設定
            for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
            {
                PlayerManager.SetPlayerVisual(i, playerInfo[i].charaSelectOutlineInfo.myName);
                PlayerManager.SetPlayerVisualImage(i, playerInfo[i].charaSelectOutlineInfo.myImageName);
            }

            //フェード
            fade.FadeIn(1.0f);
            StartCoroutine(SceneChange(1.0f));
            isSceneChange = true;
        }
    }

    //キャラ変更
    private void CharaChange(byte playerNum)
    {
        //選択しているのなら
        if (playerInfo[playerNum].isSelect || playerInfo[playerNum].charaSelectOutlineInfo.isAnimation) return;

        //キャラ選択の移動
        foreach (var dir in Enum.GetValues(typeof(Direction)).Cast<Direction>())
        {
            //移動できそうなら
            if (IsInputOK(playerNum, dir))
            {
                if (IsNextCharaNotSelect(playerNum, dir, 1))
                    SelectRelease(playerNum, dir, 1);
                else if (IsNextCharaNotSelect(playerNum, dir, 2))
                    SelectRelease(playerNum, dir, 2);

                //移動できたらこの先処理しない
                break;
            }
        }
    }

    //入力がOKかどうか
    private bool IsInputOK(byte playerNum, Direction dir)
    {
        switch (dir)
        {
            case Direction.RIGHT:
                if (inputXY[playerNum].beforeInputX <= 0.799 && inputXY[playerNum].nowInputX >= 0.8)
                    return true;
                else
                    return false;
            case Direction.LEFT:
                if (inputXY[playerNum].beforeInputX >= -0.799 && inputXY[playerNum].nowInputX <= -0.8)
                    return true;
                else
                    return false;
            case Direction.DOWN:
                if (inputXY[playerNum].beforeInputY <= 0.799 && inputXY[playerNum].nowInputY >= 0.8)
                    return true;
                else
                    return false;
            case Direction.UP:
                if (inputXY[playerNum].beforeInputY >= -0.799 && inputXY[playerNum].nowInputY <= -0.8)
                    return true;
                else
                    return false;
        }

        return false;
    }

    //次に選びたいキャラが選択されていないか
    private bool IsNextCharaNotSelect(byte playerNum, Direction dir, int plusNum)
    {
        switch (dir)
        {
            case Direction.RIGHT:
                return playerInfo[playerNum].num + (plusNum - 1) < 3 && lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num + plusNum) - 1].GetComponent<CharaSelectOutlineInfo>().SetSelect(playerNum, playerInfo[playerNum].selectColor);
            case Direction.LEFT:
                return playerInfo[playerNum].num - (plusNum - 1) > 1 && lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num - plusNum) - 1].GetComponent<CharaSelectOutlineInfo>().SetSelect(playerNum, playerInfo[playerNum].selectColor);
            case Direction.DOWN:
                return playerInfo[playerNum].line + (plusNum - 1) < LineNum.THREE && lineCharaTable[(byte)(playerInfo[playerNum].line + plusNum)][(playerInfo[playerNum].num - 1)].GetComponent<CharaSelectOutlineInfo>().SetSelect(playerNum, playerInfo[playerNum].selectColor);
            case Direction.UP:
                return playerInfo[playerNum].line - (plusNum - 1) > LineNum.ONE && lineCharaTable[(byte)(playerInfo[playerNum].line - plusNum)][(playerInfo[playerNum].num - 1)].GetComponent<CharaSelectOutlineInfo>().SetSelect(playerNum, playerInfo[playerNum].selectColor);
        }

        return false;
    }

    //選択解除
    private void SelectRelease(byte playerNum, Direction dir, int plusNum)
    {
        //選択解除
        playerInfo[playerNum].charaSelectOutlineInfo.SetSelectRelease(playerNum);

        switch (dir)
        {
            case Direction.RIGHT:
                playerInfo[playerNum].num += plusNum;
                break;
            case Direction.LEFT:
                playerInfo[playerNum].num -= plusNum;
                break;
            case Direction.DOWN:
                playerInfo[playerNum].line += plusNum;
                break;
            case Direction.UP:
                playerInfo[playerNum].line -= plusNum;
                break;
        }

        //スクリプト更新
        playerInfo[playerNum].charaSelectOutlineInfo = lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num) - 1].GetComponent<CharaSelectOutlineInfo>();

        //画像変更
        playerInfo[playerNum].playerImage.sprite = playerInfo[playerNum].charaSelectOutlineInfo.playerImage;

        //プレイヤーポジション
        Vector3 playerPos = lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num) - 1].transform.position;
        playerInfo[playerNum].barunn.transform.parent = lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num) - 1].transform;
        playerInfo[playerNum].barunn.transform.position = new Vector3(playerPos.x, playerInfo[playerNum].barunn.transform.position.y, playerPos.z);
    }

    //キャラ決定
    private void CharaDecide(byte playerNum)
    {
        //決定
        if (Input.GetButtonDown("Abutton" + playerNum) && !playerInfo[playerNum].isSelect && !playerInfo[playerNum].charaSelectOutlineInfo.isAnimation)
        {
            playerInfo[playerNum].isSelect = true;

            //決定したキャラを動かす
            playerInfo[playerNum].charaSelectOutlineInfo.Select();

            //OK画像を表示
            playerOKImage[playerNum - 1].SetActive(true);
        }

    }

    //キャラ解除
    private void CharaUnlock(byte playerNum)
    {
        //解除
        if (Input.GetButtonDown("Bbutton" + playerNum) && playerInfo[playerNum].isSelect && !playerInfo[playerNum].charaSelectOutlineInfo.isAnimation)
        {
            playerInfo[playerNum].isSelect = false;

            //解除したキャラを動かす
            playerInfo[playerNum].charaSelectOutlineInfo.Release();

            //OK画像を非表示
            playerOKImage[playerNum - 1].SetActive(false);
        }
    }

    //全員準備できたか確認
    private bool isAllPlayerOK()
    {
        for(byte i = 1; i < PlayerManager.PLAYER_MAX; i++)
        {
            //選択していないかアニメーションの最中ならfalseを返す
            if (!playerInfo[i].isSelect || playerInfo[i].charaSelectOutlineInfo.isAnimation) return false;
        }

        return true;
    }

    //生成
    IEnumerator SceneChange(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ModeSelect");
    }
}
