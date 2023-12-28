using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SubModeSelectManager : MonoBehaviour
{
    //���͏��
    public class InputInfo
    {
        public float nowInputX;
        public float nowInputY;
        public float beforeInputX;
        public float beforeInputY;
    }

    //�摜
    [SerializeField] private List<SubModeImageInfo> image;

    //�v���C���[�̏����I���摜
    [SerializeField] private List<SubModeImageInfo> playerInitializSelectImafe;

    //�v���C���[�̐F
    [SerializeField] public List<Color> playerColor;

    //�v���C���[���I�����Ă���摜��傫���\��������
    [SerializeField] public List<Image> playerSelectImageBig;

    //�t�F�[�h�p
    [SerializeField] private Fade fade;                     

    //���ۊǂ��Ă���Ƃ���
    private Dictionary<byte, SubModeImageInfo> playerSelectImage = new Dictionary<byte, SubModeImageInfo>();
    private Dictionary<byte, InputInfo> inputXY = new Dictionary<byte, InputInfo>();
    private Dictionary<byte, bool> isPlayerSelect = new Dictionary<byte, bool>();

    void Start()
    {
        //������
        for (byte i = 0; i < PlayerManager.PLAYER_MAX; i++)
        {
            //���͒l��������
            InputInfo input = new InputInfo();
            input.nowInputX = 0;
            input.nowInputY = 0;
            input.beforeInputX = 0;
            input.beforeInputY = 0;
            inputXY[(byte)(i + 1)] = input;

            //�v���C���[�̏����I���摜��������
            playerSelectImage[(byte)(i + 1)] = playerInitializSelectImafe[i];
            playerSelectImage[(byte)(i + 1)].playerSelectMyNum.Add((byte)(i + 1));
            playerSelectImage[(byte)(i + 1)].ImageColorChange();
            playerSelectImageBig[i].sprite = playerInitializSelectImafe[i].GetComponent<Image>().sprite;
            isPlayerSelect[(byte)(i + 1)] = false;
        }

        //�t�F�[�h����񂠂�̂Ȃ�
        if (fade)
            fade.FadeOut(1.0f);


        /////////////////////////////////////Test////////////////////////////////////////////
        
        isPlayerSelect[2] = true;
        isPlayerSelect[3] = true;
        isPlayerSelect[4] = true;

        //OK�摜��\��
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
            //����̓��͒l���擾
            inputXY[i].nowInputX = Input.GetAxis("L_Stick_H" + i);
            inputXY[i].nowInputY = Input.GetAxis("L_Stick_V" + i);

            SelectImageChange(i);  //�I���摜�ύX
            SelectImageDecide(i);  //�I���摜����
            SelectImageUnlock(i);  //�I���摜����


            //����̓��͒l��ۑ�
            inputXY[i].beforeInputX = inputXY[i].nowInputX;
            inputXY[i].beforeInputY = inputXY[i].nowInputY;
        }

    }

    //�I���摜�ύX
    private void SelectImageChange(byte playerNum)
    {
        //�I�𒆂Ȃ炱�̐揈�����Ȃ�
        if (isPlayerSelect[playerNum]) return;

        //�ύX���ł������`�F�b�N
        SubModeImageInfo info = playerSelectImage[playerNum].SelectImageChange(playerNum, inputXY);
        if (info)
        {
            //�ύX�ł����̂Ȃ�e�摜�̏���ύX
            playerSelectImage[playerNum].playerSelectMyNum.Remove(playerNum);
            info.playerSelectMyNum.Add(playerNum);
            playerSelectImage[playerNum].ImageColorChange();
            info.ImageColorChange();
            playerSelectImage[playerNum] = info;

            //�傫���摜���ύX
            playerSelectImageBig[playerNum - 1].sprite = playerSelectImage[playerNum].GetComponent<Image>().sprite;
        }
    }

    //�I���摜����
    private void SelectImageDecide(byte playerNum)
    {
        //����
        if (Input.GetButtonDown("Abutton" + playerNum) && !isPlayerSelect[playerNum])
        {
            isPlayerSelect[playerNum] = true;

            //OK�摜��\��
            playerSelectImageBig[playerNum - 1].transform.GetChild(0).gameObject.SetActive(true);
            playerSelectImageBig[playerNum - 1].transform.GetChild(1).gameObject.SetActive(true);

            //�S��OK�������ʊO�Ɉړ�������
            if (isAllPlayerOK()) imageMoveScreenOut();
        }

    }

    //�I���摜����
    private void SelectImageUnlock(byte playerNum)
    {
        //����
        if (Input.GetButtonDown("Bbutton" + playerNum) && isPlayerSelect[playerNum])
        {
            isPlayerSelect[playerNum] = false;

            //OK�摜���\��
            playerSelectImageBig[playerNum - 1].transform.GetChild(0).gameObject.SetActive(false);
            playerSelectImageBig[playerNum - 1].transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    //�S�������ł������m�F
    private bool isAllPlayerOK()
    {
        for (byte i = 1; i < PlayerManager.PLAYER_MAX; i++)
        {
            //�I�����Ă��Ȃ����A�j���[�V�����̍Œ��Ȃ�false��Ԃ�
            if (!isPlayerSelect[i]) return false;
        }

        return true;
    }

    //�摜��ʊO�Ɉړ�
    private void imageMoveScreenOut()
    {
        foreach (var i in image)
            i.ImageMoveScreenOut();
    }
}
