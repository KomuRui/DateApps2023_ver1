using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHand : MonoBehaviour
{

    private enum Flag
    {
        LEFT, //左旗
        RIGHT //右旗
    }

    //旗上げ情報
    private class FlagUpInfo
    {
        public GameObject flag; //旗
        public int flagSign;    //旗の符号
        public bool isUp;       //上がっているか
    }

    private enum FlagState
    {
        ALL_DOWN,
        RIGHT_UP,
        LEFT_UP,
        ALL_UP,
    }



    [SerializeField] private int playerNum;       // プレイヤー番号
    [SerializeField] private GameObject leftOb;   // 左の旗
    [SerializeField] private GameObject rightOb;  // 右の旗
    [SerializeField] private bool isOnePlayer;    // 1人プレイヤーかどうか
    [SerializeField] private int flagCount = 3;    // 
    [SerializeField] private int flagTurn;    // 
    [SerializeField] private int turnMax;    // 
    [SerializeField] private bool isFirstFlag;    // 
    [SerializeField] private bool isRank;    // 
    [SerializeField] public int[] flagState = new int[5];    // 旗状態
    [SerializeField] public int ranking;    // 
    [SerializeField] private GameObject GMOb;  // 1


    private Dictionary<Flag, FlagUpInfo> flagInfo = new Dictionary<Flag, FlagUpInfo>();
    private Vector3 initializeRotate; 


    // Start is called before the first frame update
    void Start()
    {
        //旗情報設定
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

        //初期
        initializeRotate = new Vector3(0,180,0);
        playerNum = this.transform.GetChild(5).GetComponent<PlayerNum>().playerNum;
    }

    // Update is called once per frame
    void Update()
    {

        //旗上げ
        FlagUp();

        if(isRank)
        {
            ((FlagUpGameManager)GameManager.nowMiniGameManager).nowRank--;
            isRank = false;
        }

        ////ストップしてない&自分のターン
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
        //            //上げれる時間
        //            Invoke("StopPlayer", 5.0f);
        //        }
        //    }
        //    else
        //    {
        //        if (flagMax > flagUpNum && isFirst)
        //        {
        //            isFirst = false;

        //            //1人側ターンにする
        //            flagUpGameManager.isAloneTurn = true;

        //            if (flagMax == flagUpNum)
        //            {

        //                //旗上げ回数0に戻す
        //                flagUpNum = 0;
        //                //ラウンド3以上だったら旗上げ回数5
        //                if (flagUpGameManager.roundNum >= 2)
        //                {
        //                    flagMax = 5;
        //                }

        //                flagUpGameManager.roundNum++;
        //            }

        //            //あげれない時間
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
        //旗上げ許可しないならこの先処理しない
        if (!((FlagUpGameManager)GameManager.nowMiniGameManager).isFlagUpPermit) return;

        //ターンとプレイヤーが合っていないのならこの先処理しない
        if (((FlagUpGameManager)GameManager.nowMiniGameManager).turn == FlagUpGameManager.Turn.ONE_PLAYER && !isOnePlayer) return;
        if (((FlagUpGameManager)GameManager.nowMiniGameManager).turn == FlagUpGameManager.Turn.THREE_PLAYER && isOnePlayer) return;

        //旗上げ処理
        if (Input.GetButtonDown("LBbutton" + playerNum))
            FlagUp(Flag.LEFT);
        else if (Input.GetButtonDown("RBbutton" + playerNum))
            FlagUp(Flag.RIGHT);
    }

    private void FlagUp(Flag f)
    {
        //旗が上がっているのなら
        if (flagInfo[f].isUp)
            flagInfo[f].flag.transform.DORotate(initializeRotate, 0.1f);
        else
            flagInfo[f].flag.transform.DORotate(new Vector3(0, initializeRotate.y,1 * 90) * flagInfo[f].flagSign, 0.1f);

        flagInfo[f].isUp = !(flagInfo[f].isUp);
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

        if (isOnePlayer)
        //if(((FlagUpGameManager)GameManager.nowMiniGameManager).turn == 0)
        {
            //if (isOnePlayer)
            {
                if (flagTurn < turnMax && isFirstFlag == false)
                {
                    Debug.Log(flagTurn);
                    flagState[flagTurn] = 0;

                    if (flagInfo[Flag.RIGHT].isUp)
                        flagState[flagTurn] += 1;

                    if (flagInfo[Flag.LEFT].isUp)
                        flagState[flagTurn] += 2;
                    Debug.Log(flagState[flagTurn]);
                    flagTurn++;


                }
                else
                {
                    isFirstFlag = false;
                    Debug.Log(flagState[0] + "," + flagState[1] + "," + flagState[2]);
                    //Debug.Log("ageru");
                    // GMOb.instance.SetFlagState
                }
                Debug.Log(flagState[0] + "," + flagState[1] + "," + flagState[2]);
            }
                


        }
        else
        {
            if (!isOnePlayer)
            {
                if (flagTurn < turnMax && isFirstFlag == false)
                {
                    Debug.Log("3ninn" + flagTurn);

                    flagState[flagTurn] = 0;

                    if (flagInfo[Flag.RIGHT].isUp)
                        flagState[flagTurn] += 1;

                    if (flagInfo[Flag.LEFT].isUp)
                        flagState[flagTurn] += 2;

                    if (((FlagUpGameManager)GameManager.nowMiniGameManager).oneFlagState[flagTurn] != flagState[flagTurn])
                    {

                        ranking = ((FlagUpGameManager)GameManager.nowMiniGameManager).nowRank;
                        ScoreManager.AddScore(GameManager.nowMiniGameManager.onePlayerObj.GetComponent<PlayerNum>().playerNum, ((byte)((FlagUpGameManager)GameManager.nowMiniGameManager).nowRank));


                        isRank = true;

                        GameManager.nowMiniGameManager.PlayerDead(this.transform.GetChild(5).GetComponent<PlayerNum>().playerNum);
                        GameManager.nowMiniGameManager.PlayerFinish(this.transform.GetChild(5).GetComponent<PlayerNum>().playerNum);
                        Rigidbody rb;
                        rb = this.GetComponent<Rigidbody>();
                        rb.constraints &= ~RigidbodyConstraints.FreezePositionY;
                        rb.useGravity = true;
                        Debug.Log("4");

                    }

                    flagTurn++;
                }
                else
                {
                    isFirstFlag = false;
                    Debug.Log("start");
                }
            }
                



        }

        flagInfo[Flag.LEFT].flag.transform.DORotate(initializeRotate, 0.1f);
        flagInfo[Flag.RIGHT].flag.transform.DORotate(initializeRotate, 0.1f);
        flagInfo[Flag.LEFT].isUp = false;
        flagInfo[Flag.RIGHT].isUp = false;


    }

    //一人側の手
    public int GetFlagState(int flagTurn)
    {
        return flagState[flagTurn];
    }

    //上げれない
    void StopPlayer()
    {
        //isInput = false;
        //isFirst = true;
        //flagUpNum++;
        
        ////1人側ターンにする
        //flagUpGameManager.isAloneTurn = true;

        //if (flagMax == flagUpNum)
        //{
            
        //    //旗上げ回数0に戻す
        //    flagUpNum = 0;
        //    //ラウンド3以上だったら旗上げ回数5
        //    if (flagUpGameManager.roundNum >= 2)
        //    {
        //        flagMax = 5;
        //    }
        //}
    }

    //上げれる
    void PlayPlayer()
    {
        //笛鳴らす


        //isInput = true;
        //isFirst = true;
    }
}
