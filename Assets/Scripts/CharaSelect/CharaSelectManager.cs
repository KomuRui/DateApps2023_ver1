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
    //���C����
    public enum LineNum
    {
        ONE = 1,
        TWO = 2,
        THREE = 3,
        MAX_LINE
    }

    //���
    public enum Direction
    {
        RIGHT,
        LEFT,
        UP,
        DOWN,
        MAX_DIRECTION
    }

    //�v���C���[���
    class PlayerInfo
    {
        public Color selectColor;                             //�I��F
        public CharaSelectOutlineInfo charaSelectOutlineInfo; //�L�����I���A�E�g���C�����
        public GameObject barunn;                             //����
        public Image playerImage;                             //�v���C���[�摜
        public LineNum line;                                  //�ǂ̃��C����
        public int num;                                       //���Ԗڂ�
        public bool isSelect;                                 //�I�����Ă��邩�ǂ���
    }

    //���͏��
    class InputInfo
    {
        public float nowInputX;
        public float nowInputY;
        public float beforeInputX;
        public float beforeInputY;
    }

    //�t�F�[�h
    [SerializeField] private Fade fade;

    //Player�ɕK�v�ȏ��
    [SerializeField] private List<CharaSelectOutlineInfo> line1Chara;
    [SerializeField] private List<CharaSelectOutlineInfo> line2Chara;
    [SerializeField] private List<CharaSelectOutlineInfo> line3Chara;
    [SerializeField] private List<Color> playerColor;
    [SerializeField] private List<CharaSelectOutlineInfo> playerInitializOutlineInfo;
    [SerializeField] private List<GameObject> playerUkiwa;
    [SerializeField] private List<Image> playerImage;
    [SerializeField] private List<GameObject> playerOKImage;

    //���ۊǂ��Ă���Ƃ���
    private Dictionary<byte, List<CharaSelectOutlineInfo>> lineCharaTable = new Dictionary<byte, List<CharaSelectOutlineInfo>>();
    private Dictionary<byte, PlayerInfo> playerInfo = new Dictionary<byte, PlayerInfo>();
    private Dictionary<byte, InputInfo> inputXY = new Dictionary<byte, InputInfo>();

    private bool isSceneChange = false;

    // Start is called before the first frame update
    void Start()
    {

        //�e���C��������
        lineCharaTable[(byte)LineNum.ONE] = line1Chara;
        lineCharaTable[(byte)LineNum.TWO] = line2Chara;
        lineCharaTable[(byte)LineNum.THREE] = line3Chara;

        //�e����ݒ�
        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
        {
            //���͒l��������
            InputInfo input = new InputInfo();
            input.nowInputX = 0;
            input.nowInputY = 0;
            input.beforeInputX = 0;
            input.beforeInputY = 0;
            inputXY[(byte)(i + 1)] = input;

            //�v���C���[���
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

        //�t�F�[�h����񂠂�̂Ȃ�
        if (fade)
            fade.FadeOut(1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
        {
            //����̓��͒l���擾
            inputXY[i].nowInputX = Input.GetAxis("L_Stick_H" + i);
            inputXY[i].nowInputY = Input.GetAxis("L_Stick_V" + i);

            //�L�����ύX�E����E����
            CharaChange(i);
            CharaDecide(i);
            CharaUnlock(i);

            //����̓��͒l��ۑ�
            inputXY[i].beforeInputX = inputXY[i].nowInputX;
            inputXY[i].beforeInputY = inputXY[i].nowInputY;
        }

        //�����S�������ł����̂Ȃ�t�F�[�h�C��
        if (isAllPlayerOK() && !isSceneChange)
        {
            //�v���C���[�}�l�[�W���[�Ɋe�v���C���[�̏���ݒ�
            for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
            {
                PlayerManager.SetPlayerVisual(i, playerInfo[i].charaSelectOutlineInfo.myName);
                PlayerManager.SetPlayerVisualImage(i, playerInfo[i].charaSelectOutlineInfo.myImageName);
            }

            //�t�F�[�h
            fade.FadeIn(1.0f);
            StartCoroutine(SceneChange(1.0f));
            isSceneChange = true;
        }
    }

    //�L�����ύX
    private void CharaChange(byte playerNum)
    {
        //�I�����Ă���̂Ȃ�
        if (playerInfo[playerNum].isSelect || playerInfo[playerNum].charaSelectOutlineInfo.isAnimation) return;

        //�L�����I���̈ړ�
        foreach (var dir in Enum.GetValues(typeof(Direction)).Cast<Direction>())
        {
            //�ړ��ł������Ȃ�
            if (IsInputOK(playerNum, dir))
            {
                if (IsNextCharaNotSelect(playerNum, dir, 1))
                    SelectRelease(playerNum, dir, 1);
                else if (IsNextCharaNotSelect(playerNum, dir, 2))
                    SelectRelease(playerNum, dir, 2);

                //�ړ��ł����炱�̐揈�����Ȃ�
                break;
            }
        }
    }

    //���͂�OK���ǂ���
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

    //���ɑI�т����L�������I������Ă��Ȃ���
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

    //�I������
    private void SelectRelease(byte playerNum, Direction dir, int plusNum)
    {
        //�I������
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

        //�X�N���v�g�X�V
        playerInfo[playerNum].charaSelectOutlineInfo = lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num) - 1].GetComponent<CharaSelectOutlineInfo>();

        //�摜�ύX
        playerInfo[playerNum].playerImage.sprite = playerInfo[playerNum].charaSelectOutlineInfo.playerImage;

        //�v���C���[�|�W�V����
        Vector3 playerPos = lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num) - 1].transform.position;
        playerInfo[playerNum].barunn.transform.parent = lineCharaTable[(byte)playerInfo[playerNum].line][(playerInfo[playerNum].num) - 1].transform;
        playerInfo[playerNum].barunn.transform.position = new Vector3(playerPos.x, playerInfo[playerNum].barunn.transform.position.y, playerPos.z);
    }

    //�L��������
    private void CharaDecide(byte playerNum)
    {
        //����
        if (Input.GetButtonDown("Abutton" + playerNum) && !playerInfo[playerNum].isSelect && !playerInfo[playerNum].charaSelectOutlineInfo.isAnimation)
        {
            playerInfo[playerNum].isSelect = true;

            //���肵���L�����𓮂���
            playerInfo[playerNum].charaSelectOutlineInfo.Select();

            //OK�摜��\��
            playerOKImage[playerNum - 1].SetActive(true);
        }

    }

    //�L��������
    private void CharaUnlock(byte playerNum)
    {
        //����
        if (Input.GetButtonDown("Bbutton" + playerNum) && playerInfo[playerNum].isSelect && !playerInfo[playerNum].charaSelectOutlineInfo.isAnimation)
        {
            playerInfo[playerNum].isSelect = false;

            //���������L�����𓮂���
            playerInfo[playerNum].charaSelectOutlineInfo.Release();

            //OK�摜���\��
            playerOKImage[playerNum - 1].SetActive(false);
        }
    }

    //�S�������ł������m�F
    private bool isAllPlayerOK()
    {
        for(byte i = 1; i < PlayerManager.PLAYER_MAX; i++)
        {
            //�I�����Ă��Ȃ����A�j���[�V�����̍Œ��Ȃ�false��Ԃ�
            if (!playerInfo[i].isSelect || playerInfo[i].charaSelectOutlineInfo.isAnimation) return false;
        }

        return true;
    }

    //����
    IEnumerator SceneChange(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ModeSelect");
    }
}
