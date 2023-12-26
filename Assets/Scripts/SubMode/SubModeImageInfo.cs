using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private List<Image> edgeImage;         //枠の画像
    [SerializeField] private List<Image> playerNumberImage; //プレイヤー番号の画像

    //どの方向に選択画像があるか
    private Dictionary<Direction, SubModeImageInfo> dirSelectImage = new Dictionary<Direction, SubModeImageInfo>();

    //何人のプレイヤーから選択されているか
    public List<byte> playerSelectMyNum;

    // Start is called before the first frame update
    void Start()
    {

        //枠の画像たちを透明に設定
        for(int i = 0; i < edgeImage.Count; i++)
        {
            Color a = edgeImage[i].color;
            Color b = playerNumberImage[i].color;
            a.r = 0;
            b.r = 0;
            edgeImage[i].color = a;
            playerNumberImage[i].color = b;
        }

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

    }
}
