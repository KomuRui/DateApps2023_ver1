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
        THREE_PLAYER,
        MAX
    }

    //ラウンド
    public enum Round
    {
        ONE,
        TWO,
        THREE,
        FOUR,
        FIVE,
        MAX
    }

    //ラウンド情報
    public struct RoundInfo
    {

    }

    [SerializeField] private Transform mainCamera;             //メインカメラのトランスフォーム
    [SerializeField] private Vector3 onePlayerTurnCameraPos;   //1人側ターンの時のカメラ位置
    [SerializeField] private Vector3 threePlayerTurnCameraPos; //3人側ターンの時のカメラ位置
    [SerializeField] private SETable se; //SE

    public Turn turn;                               //どっちのターンか
    public Round nowRound;                          //現在のラウンド数
    private Dictionary<Round, RoundInfo> roundInfo; //ラウンド情報

    public override void SceneStart()
    {
        //初期化
        turn = Turn.ONE_PLAYER;
        nowRound = Round.ONE;
    }

    //ゲーム開始時に呼ばれる
    public override void MiniGameStart() 
    {
        //コルーチン
        StartCoroutine(RoundStart(2.0f));
    }

    //ラウンドスタート
    IEnumerator RoundStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //長い笛
        se.PlayLongFlute();
        StartCoroutine(FlagUpStart(2.0f));
    }

    //旗上げ開始
    IEnumerator FlagUpStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //短い笛
        se.PlayShortFlute();
    }

    //ターン変更
    public void ChangeTurn() { turn = (turn == Turn.ONE_PLAYER) ? Turn.THREE_PLAYER : Turn.ONE_PLAYER; }
}
