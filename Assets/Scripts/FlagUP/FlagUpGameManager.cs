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

    }

    [SerializeField] private Transform mainCamera;             //���C���J�����̃g�����X�t�H�[��
    [SerializeField] private Vector3 onePlayerTurnCameraPos;   //1�l���^�[���̎��̃J�����ʒu
    [SerializeField] private Vector3 threePlayerTurnCameraPos; //3�l���^�[���̎��̃J�����ʒu
    [SerializeField] private SETable se; //SE

    public Turn turn;                               //�ǂ����̃^�[����
    public Round nowRound;                          //���݂̃��E���h��
    private Dictionary<Round, RoundInfo> roundInfo; //���E���h���

    public override void SceneStart()
    {
        //������
        turn = Turn.ONE_PLAYER;
        nowRound = Round.ONE;
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

        //�Z���J
        se.PlayShortFlute();
    }

    //�^�[���ύX
    public void ChangeTurn() { turn = (turn == Turn.ONE_PLAYER) ? Turn.THREE_PLAYER : Turn.ONE_PLAYER; }
}
