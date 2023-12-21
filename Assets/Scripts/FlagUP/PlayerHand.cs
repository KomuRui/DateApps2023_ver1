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
        public int count;
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
    [SerializeField] private int flagCount = 3;    // 
    [SerializeField] private int flagTurn;    // 
    [SerializeField] private int turnMax;    // 
    [SerializeField] private bool isFirstFlag;    // 
    [SerializeField] private bool isRank;    // 
    [SerializeField] public int[] flagState = new int[5];    // �����
    [SerializeField] public int ranking;    // 
    [SerializeField] private GameObject GMOb;  // 1
    private int count;  // 1
    private Dictionary<Flag, FlagUpInfo> flagInfo = new Dictionary<Flag, FlagUpInfo>();
    private Vector3 initializeRotate; 
    private bool isDead; 


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
        flagState = new int[5];
        flagTurn = 0;
        turnMax = 3;
        ranking = 0;
        isFirstFlag = true;
        isRank = false;
        isDead = false;

        //����
        initializeRotate = new Vector3(0,180,0);
        count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.nowMiniGameManager.IsFinish()) return;

        //���グ
        FlagUp();
    }

    private void FlagUp()
    {
        //���グ�����Ȃ��Ȃ炱�̐揈�����Ȃ�
        if (!((FlagUpGameManager)GameManager.nowMiniGameManager).isFlagUpPermit) return;

        //�^�[���ƃv���C���[�������Ă��Ȃ��̂Ȃ炱�̐揈�����Ȃ�
        if (((FlagUpGameManager)GameManager.nowMiniGameManager).turn == FlagUpGameManager.Turn.ONE_PLAYER && !isOnePlayer) return;
        if (((FlagUpGameManager)GameManager.nowMiniGameManager).turn == FlagUpGameManager.Turn.THREE_PLAYER && isOnePlayer) return;

        //���グ����
        if (Input.GetButtonDown("LBbutton" + this.transform.GetChild(5).GetComponent<PlayerNum>().playerNum))
            FlagUp(Flag.LEFT);
        if (Input.GetButtonDown("RBbutton" + this.transform.GetChild(5).GetComponent<PlayerNum>().playerNum))
            FlagUp(Flag.RIGHT);
    }

    private void FlagUp(Flag f)
    {
        if (flagInfo[f].count >= 1) return;
        //�����オ���Ă���̂Ȃ�
        if (flagInfo[f].isUp)
            flagInfo[f].flag.transform.DORotate(initializeRotate, 0.1f);
        else
            flagInfo[f].flag.transform.DORotate(new Vector3(0, initializeRotate.y,1 * 90) * flagInfo[f].flagSign, 0.1f);

        flagInfo[f].isUp = !(flagInfo[f].isUp);
        flagInfo[f].count++;
    }

    public void TurnReset()
    {
        flagTurn = 0;
        isFirstFlag = true;

        if ((int)((FlagUpGameManager)GameManager.nowMiniGameManager).nowRound >= 3)
            turnMax = 5;
    }

    public void AllFlagDown()
    {
        if (isDead) return;
        flagInfo[Flag.RIGHT].count = 0;
        flagInfo[Flag.LEFT].count = 0;
        if (isOnePlayer)
        //if(((FlagUpGameManager)GameManager.nowMiniGameManager).turn == 0)
        {
            //if (isOnePlayer)
            {
                if (flagTurn < turnMax && isFirstFlag == false)
                {
                    flagState[flagTurn] = 0;

                    if (flagInfo[Flag.RIGHT].isUp)
                        flagState[flagTurn] += 1;

                    if (flagInfo[Flag.LEFT].isUp)
                        flagState[flagTurn] += 2;
                    flagTurn++;


                }
                else
                {
                    isFirstFlag = false;
                }
            }
                


        }
        else
        {
            if (!isOnePlayer)
            {
                if (flagTurn < turnMax && isFirstFlag == false)
                {

                    flagState[flagTurn] = 0;

                    if (flagInfo[Flag.RIGHT].isUp)
                        flagState[flagTurn] += 1;

                    if (flagInfo[Flag.LEFT].isUp)
                        flagState[flagTurn] += 2;

                    if (((FlagUpGameManager)GameManager.nowMiniGameManager).oneFlagState[flagTurn] != flagState[flagTurn])
                    {
                        ((FlagUpGameManager)GameManager.nowMiniGameManager).rankInfo[this.transform.GetChild(5).GetComponent<PlayerNum>().playerNum] = ((FlagUpGameManager)GameManager.nowMiniGameManager).nowFlagUp;
                        GameManager.nowMiniGameManager.PlayerDead(this.transform.GetChild(5).GetComponent<PlayerNum>().playerNum);
                        GameManager.nowMiniGameManager.PlayerFinish(this.transform.GetChild(5).GetComponent<PlayerNum>().playerNum);
                        Rigidbody rb;
                        rb = this.GetComponent<Rigidbody>();
                        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
                        rb.useGravity = true;
                        isDead = true;

                    }

                    flagTurn++;
                }
                else
                {
                    isFirstFlag = false;
                }
            }
                



        }

        flagInfo[Flag.LEFT].flag.transform.DORotate(initializeRotate, 0.1f);
        flagInfo[Flag.RIGHT].flag.transform.DORotate(initializeRotate, 0.1f);
        flagInfo[Flag.LEFT].isUp = false;
        flagInfo[Flag.RIGHT].isUp = false;


    }

    //��l���̎�
    public int GetFlagState(int flagTurn)
    {
        return flagState[flagTurn];
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
