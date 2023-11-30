using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHand : MonoBehaviour
{

    private enum Flag
    {
        LEFT, //����
        RIGHT //�E��
    }

    //���グ���
    private class FlagUpInfo
    {
        public GameObject flag; //��
        public int flagSign;    //���̕���
        public bool isUp;       //�オ���Ă��邩
    }

    private enum FlagState
    {
        ALL_DOWN,
        RIGHT_UP,
        LEFT_UP,
        ALL_UP,
    }



    [SerializeField] private int playerNum;       // �v���C���[�ԍ�
    [SerializeField] private GameObject leftOb;   // ���̊�
    [SerializeField] private GameObject rightOb;  // �E�̊�
    [SerializeField] private bool isOnePlayer;    // 1�l�v���C���[���ǂ���
    [SerializeField] public int oneFlagState;    // 1�l�v���C���[�����
    [SerializeField] public int threeFlagState;    // 3�l�v���C���[�����

    private Dictionary<Flag, FlagUpInfo> flagInfo = new Dictionary<Flag, FlagUpInfo>();
    private Vector3 initializeRotate; 


    // Start is called before the first frame update
    void Start()
    {
        //�����ݒ�
        FlagUpInfo leftFlagIngo = new FlagUpInfo();
        FlagUpInfo RightFlagIngo = new FlagUpInfo();
        leftFlagIngo.flagSign = (isOnePlayer) ? -1 : -1;
        RightFlagIngo.flagSign = (isOnePlayer) ? 1 : 1;
        leftFlagIngo.isUp = false;
        RightFlagIngo.isUp = false;
        leftFlagIngo.flag = leftOb;
        RightFlagIngo.flag = rightOb;

        flagInfo[Flag.LEFT] = leftFlagIngo;
        flagInfo[Flag.RIGHT] = RightFlagIngo;

        //����
        initializeRotate = new Vector3(0,180,0);

    }

    // Update is called once per frame
    void Update()
    {
        //���グ
        FlagUp();

        ////�X�g�b�v���ĂȂ�&�����̃^�[��
        //if (flagUpGameManager.isStop == false && flagUpGameManager.isAloneTurn == false)
        //{
        //    if (isInput)
        //    {
        //        if (Input.GetButtonDown("LBbutton" + playerNum))
        //        {
        //            leftOb.transform.DORotate(Vector3.forward * -90f, 0.1f);
        //        }
        //        else if (Input.GetButtonDown("RBbutton" + playerNum))
        //        {
        //            rightOb.transform.DORotate(Vector3.forward * 90f, 0.1f);
        //        }
        //        else if (Input.GetButtonDown("Abutton" + playerNum))
        //        {
        //            leftOb.transform.DORotate(Vector3.forward * 0, 0.1f);
        //            rightOb.transform.DORotate(Vector3.forward * 0, 0.1f);
        //        }

        //        if (isFirst)
        //        {
        //            isFirst = false;
        //            //�グ��鎞��
        //            Invoke("StopPlayer", 5.0f);
        //        }
        //    }
        //    else
        //    {
        //        if (flagMax > flagUpNum && isFirst)
        //        {
        //            isFirst = false;

        //            //1�l���^�[���ɂ���
        //            flagUpGameManager.isAloneTurn = true;

        //            if (flagMax == flagUpNum)
        //            {

        //                //���グ��0�ɖ߂�
        //                flagUpNum = 0;
        //                //���E���h3�ȏゾ��������グ��5
        //                if (flagUpGameManager.roundNum >= 2)
        //                {
        //                    flagMax = 5;
        //                }

        //                flagUpGameManager.roundNum++;
        //            }

        //            //������Ȃ�����
        //            Invoke("PlayPlayer", 5.0f);
        //        }

        //        if(flagUpGameManager.roundNum >= 3)
        //        {
        //            flagUpGameManager.isStop = true;
        //        }
        //    }
        //}
    }

    private void FlagUp()
    {
        //���グ�����Ȃ��Ȃ炱�̐揈�����Ȃ�
        if (!((FlagUpGameManager)GameManager.nowMiniGameManager).isFlagUpPermit) return;

        //�^�[���ƃv���C���[�������Ă��Ȃ��̂Ȃ炱�̐揈�����Ȃ�
        if (((FlagUpGameManager)GameManager.nowMiniGameManager).turn == FlagUpGameManager.Turn.ONE_PLAYER && !isOnePlayer) return;
        if (((FlagUpGameManager)GameManager.nowMiniGameManager).turn == FlagUpGameManager.Turn.THREE_PLAYER && isOnePlayer) return;

        //���グ����
        if (Input.GetButtonDown("LBbutton" + playerNum))
            FlagUp(Flag.LEFT);
        else if (Input.GetButtonDown("RBbutton" + playerNum))
            FlagUp(Flag.RIGHT);
    }

    private void FlagUp(Flag f)
    {
        //�����オ���Ă���̂Ȃ�
        if (flagInfo[f].isUp)
            flagInfo[f].flag.transform.DORotate(initializeRotate, 0.1f);
        else
            flagInfo[f].flag.transform.DORotate(new Vector3(0, initializeRotate.y,1 * 90) * flagInfo[f].flagSign, 0.1f);

        flagInfo[f].isUp = !(flagInfo[f].isUp);
    }

    public void AllFlagDown()
    {
        //if(flagInfo[Flag.LEFT].isUp)

            //if(flagInfo[Flag.RIGHT].isUp)



       flagInfo[Flag.LEFT].flag.transform.DORotate(initializeRotate, 0.1f);
       flagInfo[Flag.RIGHT].flag.transform.DORotate(initializeRotate, 0.1f);
       flagInfo[Flag.LEFT].isUp = false;
       flagInfo[Flag.RIGHT].isUp = false;
    }

    //�グ��Ȃ�
    void StopPlayer()
    {
        //isInput = false;
        //isFirst = true;
        //flagUpNum++;
        
        ////1�l���^�[���ɂ���
        //flagUpGameManager.isAloneTurn = true;

        //if (flagMax == flagUpNum)
        //{
            
        //    //���グ��0�ɖ߂�
        //    flagUpNum = 0;
        //    //���E���h3�ȏゾ��������グ��5
        //    if (flagUpGameManager.roundNum >= 2)
        //    {
        //        flagMax = 5;
        //    }
        //}
    }

    //�グ���
    void PlayPlayer()
    {
        //�J�炷


        //isInput = true;
        //isFirst = true;
    }
}
