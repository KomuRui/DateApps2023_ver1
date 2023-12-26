using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private List<Image> edgeImage;         //�g�̉摜
    [SerializeField] private List<Image> playerNumberImage; //�v���C���[�ԍ��̉摜

    //�ǂ̕����ɑI���摜�����邩
    private Dictionary<Direction, SubModeImageInfo> dirSelectImage = new Dictionary<Direction, SubModeImageInfo>();

    //���l�̃v���C���[����I������Ă��邩
    public List<byte> playerSelectMyNum;

    // Start is called before the first frame update
    void Start()
    {

        //�g�̉摜�����𓧖��ɐݒ�
        for(int i = 0; i < edgeImage.Count; i++)
        {
            Color a = edgeImage[i].color;
            Color b = playerNumberImage[i].color;
            a.r = 0;
            b.r = 0;
            edgeImage[i].color = a;
            playerNumberImage[i].color = b;
        }

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

    }
}
