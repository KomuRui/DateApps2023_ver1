using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static FlagUpGameManager;


//���E���h
public enum Round
{
    ONE = 1,
    TWO,
    THREE,
    FOUR,
    FIVE,
    MAX
}

public class FlagUpGameManager : MiniGameManager
{

    //�^�[��
    public enum Turn
    {
        ONE_PLAYER,
        THREE_PLAYER,
        MAX
    }

    //���E���h���
    public struct RoundInfo
    {
        public float flagUpTime;    //���グ�̎���
        public int flagUpMaxCount;  //���グ����s����
    }

    [SerializeField] private List<GameObject> player;             //�v���C���[
    [SerializeField] private Transform mainCamera;                //���C���J�����̃g�����X�t�H�[��
    [SerializeField] private Vector3 onePlayerTurnCameraPos;      //1�l���^�[���̎��̃J�����ʒu
    [SerializeField] private Vector3 onePlayerTurnCameraRotate;   //1�l���^�[���̎��̃J�����ʒu
    [SerializeField] private Vector3 threePlayerTurnCameraPos;    //3�l���^�[���̎��̃J�����ʒu
    [SerializeField] private Vector3 threePlayerTurnCameraRotate; //3�l���^�[���̎��̃J�����ʒu
    [SerializeField] private float firstHalfFlagUpTime;           //�O���̊��グ����
    [SerializeField] private float secondHalfFlagUpTime;          //�㔼�̊��グ����
    [SerializeField] private int firstHalfFlagUpMax;              //�O���̊��グ��
    [SerializeField] private int secondHalfFlagUpMax;             //�㔼�̊��グ��
    [SerializeField] private SETable se;                          //SE
    [SerializeField] private TextMeshProUGUI roundText;       //���E���h�e�L�X�g
    [SerializeField] private TextMeshProUGUI finishText;       //���E���h�e�L�X�g
    [SerializeField] public int[] oneFlagState;    // �����
    [SerializeField] public Dictionary<byte,int> rankInfo = new Dictionary<byte, int>(); //�����N���(�v���C���[�ԍ��A�ǂ̂��炢��������)
    [SerializeField] public int nowFlagUp;                 //�ǂ̂��炢�̉񐔊��グ������

    public Turn turn;                                          //�ǂ����̃^�[����
    public Round nowRound;                                    //���݂̃��E���h��
    private int nowFlagUpCount;                               //���݂̊��グ��
    public bool isFlagUpPermit;                                //���グ�����邩
    private Dictionary<Round, RoundInfo> roundInfo = new Dictionary<Round, RoundInfo>(); //���E���h���

    public override void SceneStart()
    {
        //������
        turn = Turn.ONE_PLAYER;
        nowRound = Round.ONE;
        isFlagUpPermit = false;
        nowFlagUpCount = 0;
        oneFlagState = new int[5];
        nowFlagUp = 0;

        for (int i = 0;i < 5;i++)
        {
            oneFlagState[i] = 0;
        }

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

        //�S�ĉ�����
        AllFlagDown();

        TurnReset();

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
        nowFlagUp++;

        //�S�ĉ�����
        AllFlagDown();
        //���������߂Ȃ�

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
        if (nowRound <= Round.FIVE)
        {
            if (roundInfo[nowRound].flagUpMaxCount <= nowFlagUpCount)
            {
                Invoke("ChangeTurn",1.0f);
                StartCoroutine(RoundStart(2.0f));
            }
            else
                StartCoroutine(FlagUpStart(0.5f));

        }

        isFlagUpPermit = false;
    }

    //�S�Ẵv���C���[�̊���������
    private void AllFlagDown()
    {
        for (int i = 0; i < player.Count; i++)
            player[i].GetComponent<PlayerHand>().AllFlagDown();
    }

    private void TurnReset()
    {
        for (int i = 0; i < player.Count; i++)
            player[i].GetComponent<PlayerHand>().TurnReset();

        for (int j = 0; j < 5; j++)
        {
            oneFlagState[j] = player[0].GetComponent<PlayerHand>().GetFlagState(j);
           
        }
            
    }

    //�^�[���ύX
    public void ChangeTurn() {

        if (isPlayerAllDead) return;

        //���グ�񐔏�����
        nowFlagUpCount = 0;

        if (turn == Turn.ONE_PLAYER)
        {
            turn = Turn.THREE_PLAYER;
            mainCamera.position = threePlayerTurnCameraPos;
            mainCamera.localEulerAngles = threePlayerTurnCameraRotate;
        }
        else
        {
            if (nowRound < Round.FIVE)
            {
                nowRound++;
                turn = Turn.ONE_PLAYER;
                mainCamera.position = onePlayerTurnCameraPos;
                mainCamera.localEulerAngles = onePlayerTurnCameraRotate;
            }
            else
            {
                SetMiniGameFinish();
            }

            roundText.text = ((int)nowRound).ToString();

        }

    }

    public void SetFlagState(int[] FlagState)
    {
        for(int i = 0;i < 5;i++)
        {
            oneFlagState[i] = FlagState[i];
        }
    }

    public override void MiniGameFinish()
    {
        //ScoreManager.AddScore(playerNum,rank);

        //�P�l�������������ǂ���
        bool isWinOnePLayer = false;

        //�v���C���[�����ׂĎ���ł���̂Ȃ�
        if (isPlayerAllDead)
        {
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            isWinOnePLayer = true;
        }
        else
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

        //���ʂ��m�F
        byte nowRank = (isWinOnePLayer ? (byte)2 : (byte)1);
        byte sameRank = 0;

        //�����c���Ă���l�ɏ��ʂ�����
        foreach (var player in threePlayer)
        {
            //�����Ă���̂Ȃ�
            if (!player.Value)
            {
                ScoreManager.AddScore(player.Key, nowRank);
                sameRank++;
            }
        }

        //3�l���̓��_���\�[�g�ŕ��ѕς���
        var sortedDictionary = rankInfo.OrderByDescending(pair => pair.Value);
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            //�����Ă���̂Ȃ炱�̐揈�����Ȃ�
            if (!threePlayer[item.Key]) continue;

            //�O��̒l�ƈႤ�̂Ȃ��
            if (beforeValue != item.Value)
            {
                nowRank += sameRank;
                sameRank = 1;
            }
            else
                sameRank++;

            beforeValue = item.Value;
            ScoreManager.AddScore(item.Key, nowRank);
        }

    }
}
