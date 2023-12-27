using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class SubModeImageInfo : MonoBehaviour
{
    //���
    public enum Direction
    {
        RIGHT,
        LEFT,
        UP,
        DOWN,
        MAX_DIRECTION
    }

    [SerializeField] private SubModeImageInfo leftImage;    //�����̉摜
    [SerializeField] private SubModeImageInfo rightImage;   //�E���̉摜
    [SerializeField] private SubModeImageInfo upImage;      //�㑤�̉摜
    [SerializeField] private SubModeImageInfo downImage;    //�����̉摜
    [SerializeField] private List<UnityEngine.UI.Image> edgeImage;         //�g�̉摜
    [SerializeField] private List<UnityEngine.UI.Image> playerNumberImage; //�v���C���[�ԍ��̉摜
    [SerializeField] private SubModeSelectManager mana;                    //�v���C���[�ԍ��̉摜
    [SerializeField] private List<TextMeshProUGUI> text;                   //�v���C���[�ԍ��̕���

    //�ǂ̕����ɑI���摜�����邩
    private Dictionary<Direction, SubModeImageInfo> dirSelectImage = new Dictionary<Direction, SubModeImageInfo>();

    //���l�̃v���C���[����I������Ă��邩
    public List<byte> playerSelectMyNum = new List<byte>();

    // Start is called before the first frame update
    void Start()
    {
        //�e�����̉摜��ݒ�
        dirSelectImage[Direction.RIGHT] = rightImage;
        dirSelectImage[Direction.LEFT] = leftImage;
        dirSelectImage[Direction.UP] = upImage;
        dirSelectImage[Direction.DOWN] = downImage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //�I���摜��ύX(�ύX�ł����̂Ȃ�ύX���Ԃ�)
    public SubModeImageInfo SelectImageChange(byte playerNum, Dictionary<byte, SubModeSelectManager.InputInfo> input)
    {
        if (IsInputOK(playerNum, Direction.RIGHT, input) && dirSelectImage[Direction.RIGHT]) return dirSelectImage[Direction.RIGHT];
        else if (IsInputOK(playerNum, Direction.LEFT, input) && dirSelectImage[Direction.LEFT]) return dirSelectImage[Direction.LEFT];
        else if (IsInputOK(playerNum, Direction.UP, input) && dirSelectImage[Direction.UP]) return dirSelectImage[Direction.UP];
        else if (IsInputOK(playerNum, Direction.DOWN, input) && dirSelectImage[Direction.DOWN]) return dirSelectImage[Direction.DOWN];

        return null;
    }

    //���͂�OK���ǂ���
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

    //�摜�̐F�ύX
    public void ImageColorChange()
    {

        //�����J���[
        Color alpha = Color.black;
        alpha.a = 0;
        Color[] color = { alpha, alpha, alpha, alpha };

        //�I��ł���v���C���[�̐l���ɂ���Ă�����
        switch(playerSelectMyNum.Count)
        {
            case 0:

                //�v���C���[�ԍ��̐F��ς���
                playerNumberImage[0].color = alpha;
                playerNumberImage[1].color = alpha;
                playerNumberImage[2].color = alpha;
                playerNumberImage[3].color = alpha;

                //�����ύX
                text[0].text = "";
                text[1].text = "";
                text[2].text = "";
                text[3].text = "";

                break;
            case 1:

                //�g�̐F�����߂�
                color[0] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[1] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[2] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[3] = mana.playerColor[playerSelectMyNum[0] - 1];

                //�����ύX
                text[0].text = playerSelectMyNum[0].ToString() + "P";
                text[1].text = "";
                text[2].text = "";
                text[3].text = "";

                //�v���C���[�ԍ��̐F��ς���
                playerNumberImage[0].color = color[0];
                playerNumberImage[1].color = alpha;
                playerNumberImage[2].color = alpha;
                playerNumberImage[3].color = alpha;

                break;
            case 2:

                //�g�̐F�����߂�
                color[0] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[1] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[2] = mana.playerColor[playerSelectMyNum[1] - 1];
                color[3] = mana.playerColor[playerSelectMyNum[1] - 1];

                //�����ύX
                text[0].text = playerSelectMyNum[0].ToString() + "P";
                text[1].text = playerSelectMyNum[1].ToString() + "P";
                text[2].text = "";
                text[3].text = "";

                //�v���C���[�ԍ��̐F��ς���
                playerNumberImage[0].color = color[0];
                playerNumberImage[1].color = color[2];
                playerNumberImage[2].color = alpha;
                playerNumberImage[3].color = alpha;

                break;
            case 3:

                //�g�̐F�����߂�
                color[0] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[1] = mana.playerColor[playerSelectMyNum[1] - 1];
                color[2] = mana.playerColor[playerSelectMyNum[1] - 1];
                color[3] = mana.playerColor[playerSelectMyNum[2] - 1];

                //�����ύX
                text[0].text = playerSelectMyNum[0].ToString() + "P";
                text[1].text = playerSelectMyNum[1].ToString() + "P";
                text[2].text = playerSelectMyNum[2].ToString() + "P";
                text[3].text = "";

                //�v���C���[�ԍ��̐F��ς���
                playerNumberImage[0].color = color[0];
                playerNumberImage[1].color = color[1];
                playerNumberImage[2].color = color[3];
                playerNumberImage[3].color = alpha;

                break;
            case 4:

                //�g�̐F�����߂�
                color[0] = mana.playerColor[playerSelectMyNum[0] - 1];
                color[1] = mana.playerColor[playerSelectMyNum[1] - 1];
                color[2] = mana.playerColor[playerSelectMyNum[2] - 1];
                color[3] = mana.playerColor[playerSelectMyNum[3] - 1];

                //�����ύX
                text[0].text = playerSelectMyNum[0].ToString() + "P";
                text[1].text = playerSelectMyNum[1].ToString() + "P";
                text[2].text = playerSelectMyNum[2].ToString() + "P";
                text[3].text = playerSelectMyNum[3].ToString() + "P";

                //�v���C���[�ԍ��̐F��ς���
                playerNumberImage[0].color = color[0];
                playerNumberImage[1].color = color[1];
                playerNumberImage[2].color = color[2];
                playerNumberImage[3].color = color[3];

                break;
        }

        //�g�̉摜�̐F��ݒ�
        for (int i = 0; i < edgeImage.Count; i++)
            edgeImage[i].color = color[i];

    }
}
