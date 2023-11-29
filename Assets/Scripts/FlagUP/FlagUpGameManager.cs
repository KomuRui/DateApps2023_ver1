using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FlagUpGameManager;

public class FlagUpGameManager : MiniGameManager
{

    //�^�[��
    public enum Turn
    {
        ONE_PLAYER,
        THREE_PLAYER,
        MAX
    }

    //���E���h
    public enum Round
    {
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        MAX
    }

    //���E���h���
    public struct RoundInfo
    {
        public float flagUpTime;    //���グ�̎���
        public int flagUpMaxCount;  //���グ����s����
    }

    [SerializeField] private Transform mainCamera;             //���C���J�����̃g�����X�t�H�[��
    [SerializeField] private Vector3 onePlayerTurnCameraPos;   //1�l���^�[���̎��̃J�����ʒu
    [SerializeField] private Vector3 threePlayerTurnCameraPos; //3�l���^�[���̎��̃J�����ʒu
    [SerializeField] private float firstHalfFlagUpTime;        //�O���̊��グ����
    [SerializeField] private float secondHalfFlagUpTime;       //�㔼�̊��グ����
    [SerializeField] private int firstHalfFlagUpMax;           //�O���̊��グ��
    [SerializeField] private int secondHalfFlagUpMax;          //�㔼�̊��グ��
    [SerializeField] private SETable se;                       //SE

    public Turn turn;                                          //�ǂ����̃^�[����
    private Round nowRound;                                    //���݂̃��E���h��
    private int  nowFlagUpCount;                               //���݂̊��グ��
    public bool isFlagUpPermit;                                //���グ�����邩
    private Dictionary<Round, RoundInfo> roundInfo = new Dictionary<Round, RoundInfo>(); //���E���h���

    public override void SceneStart()
    {
        //������
        turn = Turn.ONE_PLAYER;
        nowRound = Round.ONE;
        isFlagUpPermit = false;
        nowFlagUpCount = 0;

        //���E���h���̏�����
        RoundInfo firstHalf = new RoundInfo();
        RoundInfo secondHalf = new RoundInfo();
        firstHalf.flagUpTime = 1.0f;
        secondHalf.flagUpTime = 0.5f;
        firstHalf.flagUpMaxCount = 3;
        secondHalf.flagUpMaxCount = 5;

        roundInfo[Round.ONE] = firstHalf;
        roundInfo[Round.TWO] = firstHalf;
        roundInfo[Round.THREE] = secondHalf;
        roundInfo[Round.FOUR] = secondHalf;
        roundInfo[Round.FIVE] = secondHalf;
    }

    //�Q�[���J�n���ɌĂ΂��
    public override void MiniGameStart() 
    {
        //�R���[�`��
        StartCoroutine(RoundStart(2.0f));
    }

    //���E���h�X�^�[�g
    IEnumerator RoundStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //�����J
        se.PlayLongFlute();
        StartCoroutine(FlagUpStart(2.0f));
    }

    //���グ�J�n
    IEnumerator FlagUpStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //���グ�񐔃v���X
        nowFlagUpCount++;

        //�Z���J
        se.PlayShortFlute();
        isFlagUpPermit = true;
        StartCoroutine(NextFlagUp(roundInfo[nowRound].flagUpTime));
    }
    
    //���̊��グ��
    IEnumerator NextFlagUp(float delay)
    {
        yield return new WaitForSeconds(delay);

        //1�l����3�l���v���C���[�̊��グ���I�������Ȃ�^�[���ύX
        if(roundInfo[nowRound].flagUpMaxCount <= nowFlagUpCount)
            ChangeTurn();
        else
            StartCoroutine(FlagUpStart(0.5f));

        isFlagUpPermit = false;
    }

    //�^�[���ύX
    public void ChangeTurn() {

        //���グ�񐔏�����
        nowFlagUpCount = 0;

        if (turn == Turn.ONE_PLAYER)
        {
            turn = Turn.THREE_PLAYER;
            mainCamera.position = threePlayerTurnCameraPos;
        }
        else
        {
            turn = Turn.ONE_PLAYER;
            mainCamera.position = onePlayerTurnCameraPos;
        }

    }
}
