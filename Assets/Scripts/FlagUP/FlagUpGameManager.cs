using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static FlagUpGameManager;


//ラウンド
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

    //ターン
    public enum Turn
    {
        ONE_PLAYER,
        THREE_PLAYER,
        MAX
    }

    //ラウンド情報
    public struct RoundInfo
    {
        public float flagUpTime;    //旗上げの時間
        public int flagUpMaxCount;  //旗上げ何回行うか
    }

    [SerializeField] private List<GameObject> player;             //プレイヤー
    [SerializeField] private Transform mainCamera;                //メインカメラのトランスフォーム
    [SerializeField] private Vector3 onePlayerTurnCameraPos;      //1人側ターンの時のカメラ位置
    [SerializeField] private Vector3 onePlayerTurnCameraRotate;   //1人側ターンの時のカメラ位置
    [SerializeField] private Vector3 threePlayerTurnCameraPos;    //3人側ターンの時のカメラ位置
    [SerializeField] private Vector3 threePlayerTurnCameraRotate; //3人側ターンの時のカメラ位置
    [SerializeField] private float firstHalfFlagUpTime;           //前半の旗上げ時間
    [SerializeField] private float secondHalfFlagUpTime;          //後半の旗上げ時間
    [SerializeField] private int firstHalfFlagUpMax;              //前半の旗上げ回数
    [SerializeField] private int secondHalfFlagUpMax;             //後半の旗上げ回数
    [SerializeField] private SETable se;                          //SE
    [SerializeField] private TextMeshProUGUI roundText;       //ラウンドテキスト
    [SerializeField] private TextMeshProUGUI finishText;       //ラウンドテキスト
    [SerializeField] public int[] oneFlagState;    // 旗状態
    [SerializeField] public Dictionary<byte,int> rankInfo = new Dictionary<byte, int>(); //ランク情報(プレイヤー番号、どのくらい生きたか)
    [SerializeField] public int nowFlagUp;                 //どのくらいの回数旗上げしたか

    public Turn turn;                                          //どっちのターンか
    public Round nowRound;                                    //現在のラウンド数
    private int nowFlagUpCount;                               //現在の旗上げ回数
    public bool isFlagUpPermit;                                //旗上げ許可するか
    private Dictionary<Round, RoundInfo> roundInfo = new Dictionary<Round, RoundInfo>(); //ラウンド情報

    public override void SceneStart()
    {
        //初期化
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

        //ラウンド情報の初期化
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

        //全て下げる
        AllFlagDown();

        TurnReset();

        //長い笛
        se.PlayLongFlute();
        StartCoroutine(FlagUpStart(2.0f));
    }

    //旗上げ開始
    IEnumerator FlagUpStart(float delay)
    {
        yield return new WaitForSeconds(delay);

        //旗上げ回数プラス
        nowFlagUpCount++;
        nowFlagUp++;

        //全て下げる
        AllFlagDown();
        //いっかいめなし

        //短い笛
        se.PlayShortFlute();
        isFlagUpPermit = true;
        StartCoroutine(NextFlagUp(roundInfo[nowRound].flagUpTime));
    }

    //次の旗上げへ
    IEnumerator NextFlagUp(float delay)
    {
        yield return new WaitForSeconds(delay);

        //1人側か3人側プレイヤーの旗上げが終了したならターン変更
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

    //全てのプレイヤーの旗を下げる
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

    //ターン変更
    public void ChangeTurn() {

        if (isPlayerAllDead) return;

        //旗上げ回数初期化
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

        //１人側が勝ったかどうか
        bool isWinOnePLayer = false;

        //プレイヤーがすべて死んでいるのなら
        if (isPlayerAllDead)
        {
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            isWinOnePLayer = true;
        }
        else
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

        //順位を確認
        byte nowRank = (isWinOnePLayer ? (byte)2 : (byte)1);
        byte sameRank = 0;

        //生き残っている人に順位をつける
        foreach (var player in threePlayer)
        {
            //生きているのなら
            if (!player.Value)
            {
                ScoreManager.AddScore(player.Key, nowRank);
                sameRank++;
            }
        }

        //3人側の得点をソートで並び変える
        var sortedDictionary = rankInfo.OrderByDescending(pair => pair.Value);
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            //生きているのならこの先処理しない
            if (!threePlayer[item.Key]) continue;

            //前回の値と違うのならば
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
