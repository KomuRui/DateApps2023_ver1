using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private int playerNum;      // プレイヤー番号
    [SerializeField] public GameObject leftOb;   // 左の旗
    [SerializeField] public GameObject rightOb;  // 右の旗

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
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
