using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SubModeImageInfo : MonoBehaviour
{
    //宝庫
    public enum Direction
    {
        RIGHT,
        LEFT,
        UP,
        DOWN,
        MAX_DIRECTION
    }

    [SerializeField] private SubModeImageInfo leftImage;    //左側の画像
    [SerializeField] private SubModeImageInfo rightImage;   //右側の画像
    [SerializeField] private SubModeImageInfo upImage;      //上側の画像
    [SerializeField] private SubModeImageInfo downImage;    //下側の画像
    [SerializeField] private List<UnityEngine.UI.Image> edgeImage;         //枠の画像
    [SerializeField] private List<UnityEngine.UI.Image> playerNumberImage; //プレイヤー番号の画像
    [SerializeField] private SubModeSelectManager mana;                    //プレイヤー番号の画像
    [SerializeField] private List<TextMeshProUGUI> text;                   //プレイヤー番号の文字

    //どの方向に選択画像があるか
    private Dictionary<Direction, SubModeImageInfo> dirSelectImage = new Dictionary<Direction, SubModeImageInfo>();

    //何人のプレイヤーから選択されているか
    public List<byte> playerSelectMyNum = new List<byte>();

    // Start is called before the first frame update
    void Start()
    {
        //各方向の画像を設定
        dirSelectImage[Direction.RIGHT] = rightImage;
        dirSelectImage[Direction.LEFT] = leftImage;
        dirSelectImage[Direction.UP] = upImage;
        dirSelectImage[Direction.DOWN] = downImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //選択画像を変更(変更できたのなら変更先を返す)
    public SubModeImageInfo SelectImageChange(byte playerNum, Dictionary<byte, SubModeSelectManager.InputInfo> input)
    {
        if (IsInputOK(playerNum, Direction.RIGHT, input) && dirSelectImage[Direction.RIGHT]) return dirSelectImage[Direction.RIGHT];
        else if (IsInputOK(playerNum, Direction.LEFT, input) && dirSelectImage[Direction.LEFT]) return dirSelectImage[Direction.LEFT];
        else if (IsInputOK(playerNum, Direction.UP, input) && dirSelectImage[Direction.UP]) return dirSelectImage[Direction.UP];
        else if (IsInputOK(playerNum, Direction.DOWN, input) && dirSelectImage[Direction.DOWN]) return dirSelectImage[Direction.DOWN];

        return null;
    }

    //入力がOKかどうか
    private bool IsInputOK(byte playerNum,Direction dir,Dictionary<byte, SubModeSelectManager.InputInfo> input)
    {
        switch (dir)
        {
            case Direction.RIGHT:
                if (input[playerNum].beforeInputX <= 0.799 && input[playerNum].nowInputX >= 0.8)
                    return true;
                else
                    return false;
            case Direction.LEFT:
                if (input[playerNum].beforeInputX >= -0.799 && input[playerNum].nowInputX <= -0.8)
                    return true;
                else
                    return false;
            case Direction.DOWN:
                if (input[playerNum].beforeInputY <= 0.799 && input[playerNum].nowInputY >= 0.8)
                    return true;
                else
                    return false;
            case Direction.UP:
                if (input[playerNum].beforeInputY >= -0.799 && input[playerNum].nowInputY <= -0.8)
                    return true;
                else
                    return false;
        }

        return false;
    }

    //画像の色変更
    public void ImageColorChange()
    {

        //初期カラー
        Color alpha = Color.black;
        alpha.a = 0;
        Color[] color = { alpha, alpha, alpha, alpha };

        //選んでいるプレイヤーの人数によってかえる
        switch(playerSelectMyNum.Count)
        {
            case 0:

                //プレイヤー番号の色を変える
                playerNumberImage[0].color = alpha;
                playerNumberImage[1].color = alpha;
                playerNumberImage[2].color = alpha;
                playerNumberImage[3].color = alpha;

                //文字変更
                text[0].text = "";
                text[1].text = "";
                text[2].text = "";
                text[3].text = "";

                break;
            case 1:

                //枠の色を決める
                color[0] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[1] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[2] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[3] = mana.playerColor[playerSelectMyNum[0] - 1];

                //文字変更
                text[0].text = playerSelectMyNum[0].ToString() + "P";
                text[1].text = "";
                text[2].text = "";
                text[3].text = "";

                //プレイヤー番号の色を変える
                playerNumberImage[0].color = color[0];
                playerNumberImage[1].color = alpha;
                playerNumberImage[2].color = alpha;
                playerNumberImage[3].color = alpha;

                break;
            case 2:

                //枠の色を決める
                color[0] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[1] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[2] = mana.playerColor[playerSelectMyNum[1] - 1];
                color[3] = mana.playerColor[playerSelectMyNum[1] - 1];

                //文字変更
                text[0].text = playerSelectMyNum[0].ToString() + "P";
                text[1].text = playerSelectMyNum[1].ToString() + "P";
                text[2].text = "";
                text[3].text = "";

                //プレイヤー番号の色を変える
                playerNumberImage[0].color = color[0];
                playerNumberImage[1].color = color[2];
                playerNumberImage[2].color = alpha;
                playerNumberImage[3].color = alpha;

                break;
            case 3:

                //枠の色を決める
                color[0] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[1] = mana.playerColor[playerSelectMyNum[1] - 1];
                color[2] = mana.playerColor[playerSelectMyNum[1] - 1];
                color[3] = mana.playerColor[playerSelectMyNum[2] - 1];

                //文字変更
                text[0].text = playerSelectMyNum[0].ToString() + "P";
                text[1].text = playerSelectMyNum[1].ToString() + "P";
                text[2].text = playerSelectMyNum[2].ToString() + "P";
                text[3].text = "";

                //プレイヤー番号の色を変える
                playerNumberImage[0].color = color[0];
                playerNumberImage[1].color = color[1];
                playerNumberImage[2].color = color[3];
                playerNumberImage[3].color = alpha;

                break;
            case 4:

                //枠の色を決める
                color[0] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[1] = mana.playerColor[playerSelectMyNum[1] - 1];
                color[2] = mana.playerColor[playerSelectMyNum[2] - 1];
                color[3] = mana.playerColor[playerSelectMyNum[3] - 1];

                //文字変更
                text[0].text = playerSelectMyNum[0].ToString() + "P";
                text[1].text = playerSelectMyNum[1].ToString() + "P";
                text[2].text = playerSelectMyNum[2].ToString() + "P";
                text[3].text = playerSelectMyNum[3].ToString() + "P";

                //プレイヤー番号の色を変える
                playerNumberImage[0].color = color[0];
                playerNumberImage[1].color = color[1];
                playerNumberImage[2].color = color[2];
                playerNumberImage[3].color = color[3];

                break;
        }

        //枠の画像の色を設定
        for (int i = 0; i < edgeImage.Count; i++)
            edgeImage[i].color = color[i];

    }
}
