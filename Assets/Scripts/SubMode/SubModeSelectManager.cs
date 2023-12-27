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

    //�v���C���[�̏����I���摜
    [SerializeField] private List<SubModeImageInfo> playerInitializSelectImafe;

    //�v���C���[�̐F
    [SerializeField] public List<Color> playerColor;

    //�v���C���[���I�����Ă���摜��傫���\��������
    [SerializeField] public List<Image> playerSelectImageBig;

    //���ۊǂ��Ă���Ƃ���
    private Dictionary<byte, SubModeImageInfo> playerSelectImage = new Dictionary<byte, SubModeImageInfo>();
    private Dictionary<byte, InputInfo> inputXY = new Dictionary<byte, InputInfo>();

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
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
        {
            //����̓��͒l���擾
            inputXY[i].nowInputX = Input.GetAxis("L_Stick_H" + i);
            inputXY[i].nowInputY = Input.GetAxis("L_Stick_V" + i);

            //�ύX���ł������`�F�b�N
            SubModeImageInfo info = playerSelectImage[i].SelectImageChange(i, inputXY);
            if (info)
            {
                //�ύX�ł����̂Ȃ�e�摜�̏���ύX
                playerSelectImage[i].playerSelectMyNum.Remove(i);
                info.playerSelectMyNum.Add(i);
                playerSelectImage[i].ImageColorChange();
                info.ImageColorChange();
                playerSelectImage[i] = info;

                //�傫���摜���ύX
                playerSelectImageBig[i - 1].sprite = playerSelectImage[i].GetComponent<Image>().sprite;
            }

            //����̓��͒l��ۑ�
            inputXY[i].beforeInputX = inputXY[i].nowInputX;
            inputXY[i].beforeInputY = inputXY[i].nowInputY;
        }
    }
}
