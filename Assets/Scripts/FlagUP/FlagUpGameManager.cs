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
        THREE_PLAYER
    }

    [SerializeField] private Transform mainCamera;             //���C���J�����̃g�����X�t�H�[��
    [SerializeField] private Vector3 onePlayerTurnCameraPos;   //1�l���^�[���̎��̃J�����ʒu
    [SerializeField] private Vector3 threePlayerTurnCameraPos; //3�l���^�[���̎��̃J�����ʒu

    public Turn turn;       //�ǂ����̃^�[����
    public int nowRoundNum; //���݂̃��E���h��

    public override void SceneStart()
    {
        turn = Turn.ONE_PLAYER;
        nowRoundNum = 0;
    }

    //�^�[���ύX
    public void ChangeTurn() { turn = (turn == Turn.ONE_PLAYER) ? Turn.THREE_PLAYER : Turn.ONE_PLAYER; }
}
