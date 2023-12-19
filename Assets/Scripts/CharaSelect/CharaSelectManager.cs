using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        public Outline charaOutline;                          //キャラアウトライン
        public LineNum line;                                  //どのラインか
        public int num;                                       //何番目か
    }

    //入力情報
    class InputInfo
    {
        public float nowInputX;
        public float nowInputY;
        public float beforeInputX;
        public float beforeInputY;
    }

    [SerializeField] private List<CharaSelectOutlineInfo> line1Chara;
    [SerializeField] private List<CharaSelectOutlineInfo> line2Chara;
    [SerializeField] private List<CharaSelectOutlineInfo> line3Chara;
    [SerializeField] private List<Color> playerColor;
    [SerializeField] private List<CharaSelectOutlineInfo> playerInitializOutlineInfo;
    [SerializeField] private List<Outline> outline;

    private Dictionary<byte, List<CharaSelectOutlineInfo>> lineCharaTable = new Dictionary<byte, List<CharaSelectOutlineInfo>>();
    private Dictionary<byte, PlayerInfo> playerInfo = new Dictionary<byte, PlayerInfo>();
    private Dictionary<byte, InputInfo> inputXY = new Dictionary<byte, InputInfo>();

    // Start is called before the first frame update
    void Start()
    {
        lineCharaTable[(byte)LineNum.ONE] = line1Chara;
        lineCharaTable[(byte)LineNum.TWO] = line2Chara;
        lineCharaTable[(byte)LineNum.THREE] = line3Chara;

        //各情報を設定
        for(int i = 0; i < PlayerManager.PLAYER_MAX; i++)
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
            info.selectColor = playerColor[i];
            info.charaSelectOutlineInfo = playerInitializOutlineInfo[i];
            info.charaOutline = outline[i];
            info.line = info.charaSelectOutlineInfo.line;
            info.num = info.charaSelectOutlineInfo.num;
            info.charaSelectOutlineInfo.SetSelect((byte)(i + 1), playerColor[i]);
            playerInfo[(byte)(i + 1)] = info;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //新しい入力値を取得
        for (int i = 0; i < inputXY.Count; i++)
        {
            inputXY[(byte)(i + 1)].nowInputX = Input.GetAxis("L_Stick_H" + (i + 1));
            inputXY[(byte)(i + 1)].nowInputY = Input.GetAxis("L_Stick_V" + (i + 1));
        }

        //キャラ変更
        CharaChange(1);
        CharaChange(2);
        CharaChange(3);
        CharaChange(4);

        //キャラ決定


        //入力値を取得
        for (int i = 0; i < inputXY.Count; i++)
        {
            inputXY[(byte)(i + 1)].beforeInputX = Input.GetAxis("L_Stick_H" + (i + 1));
            inputXY[(byte)(i + 1)].beforeInputY = Input.GetAxis("L_Stick_V" + (i + 1));
        }
    }

    //キャラ変更
    private void CharaChange(byte playerNum)
    {
        //右移動できそうなら
        if (IsInputOK(playerNum,Direction.RIGHT) && playerInfo[playerNum].num != 3 && IsNextCharaNotSelect(playerNum,Direction.RIGHT,1))
        {
            lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num) - 1].GetComponent<CharaSelectOutlineInfo>().SetSelectRelease(playerNum);
            playerInfo[playerNum].num++;
        }
        //左移動できそうなら
        else if (inputXY[playerNum].beforeInputX >= -0.75 && inputXY[playerNum].nowInputX <= -0.8 && playerInfo[playerNum].num != 1 &&
            lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num - 1) - 1].GetComponent<CharaSelectOutlineInfo>().SetSelect(playerNum, playerInfo[playerNum].selectColor))
        {
            lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num - 1)].GetComponent<CharaSelectOutlineInfo>().SetSelectRelease(playerNum);
            playerInfo[playerNum].num--;
        }
        //下移動できそうなら
        else if (inputXY[playerNum].beforeInputY <= 0.75 && inputXY[playerNum].nowInputY >= 0.8 && playerInfo[playerNum].line != LineNum.THREE &&
            lineCharaTable[(byte)(playerInfo[playerNum].line + 1)][(playerInfo[playerNum].num - 1)].GetComponent<CharaSelectOutlineInfo>().SetSelect(playerNum, playerInfo[playerNum].selectColor))
        {
            lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num - 1)].GetComponent<CharaSelectOutlineInfo>().SetSelectRelease(playerNum);
            playerInfo[playerNum].line++;
        }
        //上移動できそうなら
        else if (inputXY[playerNum].beforeInputY >= -0.75 && inputXY[playerNum].nowInputY <= -0.8 && playerInfo[playerNum].line != LineNum.ONE &&
            lineCharaTable[(byte)(playerInfo[playerNum].line - 1)][(playerInfo[playerNum].num - 1)].GetComponent<CharaSelectOutlineInfo>().SetSelect(playerNum, playerInfo[playerNum].selectColor))
        {
            lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num - 1)].GetComponent<CharaSelectOutlineInfo>().SetSelectRelease(playerNum);
            playerInfo[playerNum].line--;
        }    
    }

    //入力がOKかどうか
    private bool IsInputOK(byte playerNum,Direction dir)
    {
        switch(dir)
        {
            case Direction.RIGHT:
                if (inputXY[playerNum].beforeInputX <= 0.75 && inputXY[playerNum].nowInputX >= 0.8)
                    return true;
                else
                    return false;
            case Direction.LEFT:
                if (inputXY[playerNum].beforeInputX >= -0.75 && inputXY[playerNum].nowInputX <= -0.8)
                    return true;
                else
                    return false;
            case Direction.DOWN:
                if (inputXY[playerNum].beforeInputY <= 0.75 && inputXY[playerNum].nowInputY >= 0.8)
                    return true;
                else
                    return false;
            case Direction.UP:
                if (inputXY[playerNum].beforeInputY >= -0.75 && inputXY[playerNum].nowInputY <= -0.8)
                    return true;
                else
                    return false;
        }

        return false;
    }

    //次に選びたいキャラが選択されていないか
    private bool IsNextCharaNotSelect(byte playerNum, Direction dir,int plusNum)
    {
        switch (dir)
        {
            case Direction.RIGHT:
                    return playerInfo[playerNum].num + (plusNum - 1) < 3 && lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num + plusNum) - 1].GetComponent<CharaSelectOutlineInfo>().SetSelect(playerNum, playerInfo[playerNum].selectColor); 
            case Direction.LEFT:
                    return playerInfo[playerNum].num - (plusNum - 1) > 1 && lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num - plusNum) - 1].GetComponent<CharaSelectOutlineInfo>().SetSelect(playerNum, playerInfo[playerNum].selectColor);
            case Direction.DOWN:
                    return lineCharaTable[(byte)(playerInfo[playerNum].line + plusNum)][(playerInfo[playerNum].num - 1)].GetComponent<CharaSelectOutlineInfo>().SetSelect(playerNum, playerInfo[playerNum].selectColor);
            case Direction.UP:
                    return lineCharaTable[(byte)(playerInfo[playerNum].line - plusNum)][(playerInfo[playerNum].num - 1)].GetComponent<CharaSelectOutlineInfo>().SetSelect(playerNum, playerInfo[playerNum].selectColor);
        }

        return false;
    }

    //キャラ決定
    private void CharaDecide(byte playerNum)
    {

    }
}
