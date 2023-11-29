using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FlagUpGameManager;

public class FlagUpGameManager : MiniGameManager
{

    //ターン
    public enum Turn
    {
        ONE_PLAYER,
        THREE_PLAYER
    }

    [SerializeField] private Transform mainCamera;             //メインカメラのトランスフォーム
    [SerializeField] private Vector3 onePlayerTurnCameraPos;   //1人側ターンの時のカメラ位置
    [SerializeField] private Vector3 threePlayerTurnCameraPos; //3人側ターンの時のカメラ位置

    public Turn turn;       //どっちのターンか
    public int nowRoundNum; //現在のラウンド数

    public override void SceneStart()
    {
        turn = Turn.ONE_PLAYER;
        nowRoundNum = 0;
    }

    //ターン変更
    public void ChangeTurn() { turn = (turn == Turn.ONE_PLAYER) ? Turn.THREE_PLAYER : Turn.ONE_PLAYER; }
}
