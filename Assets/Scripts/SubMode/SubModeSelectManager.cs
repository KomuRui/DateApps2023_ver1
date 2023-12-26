using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if (info) playerSelectImage[i] = info;

            //����̓��͒l��ۑ�
            inputXY[i].beforeInputX = inputXY[i].nowInputX;
            inputXY[i].beforeInputY = inputXY[i].nowInputY;
        }
    }
}
