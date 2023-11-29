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

    [SerializeField] private int playerNum;       // プレイヤー番号
    [SerializeField] private GameObject leftOb;   // 左の旗
    [SerializeField] private GameObject rightOb;  // 右の旗
    [SerializeField] private bool isOnePlayer;    // 1人プレイヤーかどうか

    private Dictionary<Flag, FlagUpInfo> flagInfo = new Dictionary<Flag, FlagUpInfo>();
    private int leftFlagSign;  //左の旗の符号 
    private int rightFlagSign; //右の旗の符号

    // Start is called before the first frame update
    void Start()
    {
        //旗情報設定
        FlagUpInfo leftFlagIngo = new FlagUpInfo();
        FlagUpInfo RightFlagIngo = new FlagUpInfo();
        leftFlagIngo.flagSign = (isOnePlayer) ? -1 : 1;
        RightFlagIngo.flagSign = (isOnePlayer) ? 1 : -1;
        leftFlagIngo.isUp = false;
        RightFlagIngo.isUp = false;
        leftFlagIngo.flag = leftOb;
        RightFlagIngo.flag = rightOb;

        flagInfo[Flag.LEFT] = leftFlagIngo;
        flagInfo[Flag.RIGHT] = RightFlagIngo;
    }

    // Update is called once per frame
    void Update()
    {
        //旗上げ
        FlagUp();
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
            flagInfo[f].flag.transform.DORotate(Vector3.zero, 0.1f);
        else
            flagInfo[f].flag.transform.DORotate(Vector3.forward * 90 * flagInfo[f].flagSign, 0.1f);

        flagInfo[f].isUp = !(flagInfo[f].isUp);
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
