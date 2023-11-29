using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePlayerHand : MonoBehaviour
{
    [SerializeField] private int playerNum;                   // プレイヤー番号
    public GameObject leftOb;
    public GameObject rightOb;
    public GameObject GMOb;

    public bool isInput;//1回
    public bool isFirst;//1回

    private int flagUpNum;
    private int flagMax;

    FlagUpGameManager flagUpGameManager;

    // Start is called before the first frame update
    void Start()
    {
        //isInput = true;
        //isFirst = true;
        //flagUpNum = 0;
        //flagMax = 3;
        ////開始まで
        //Invoke("PlayPlayer",5.0f);
        //flagUpGameManager = GMOb.GetComponent<FlagUpGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //ストップしてない&自分のターン
        //if(flagUpGameManager.isStop == false && flagUpGameManager.isAloneTurn == true)
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
        //            leftOb.transform.DORotate(Vector3.forward * 0f, 0.1f);
        //            rightOb.transform.DORotate(Vector3.forward * 0f, 0.1f);
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

        //            //3人側ターンにする
        //            flagUpGameManager.isAloneTurn = false;
        //            Debug.Log("3人だよ");

        //            if (flagMax == flagUpNum)
        //            {
        //                //旗上げ回数0に戻す
        //                flagUpNum = 0;
        //                //ラウンド3以上だったら旗上げ回数5
        //                if (flagUpGameManager.roundNum >= 2)
        //                {
        //                    flagMax = 5;
        //                }
        //            }

        //            //あげれない時間
        //            Invoke("PlayPlayer", 5.0f);
        //        }
        //    }
        //}

        
    }

    //上げれない
    void StopPlayer()
    {
        //isInput = false;
        //isFirst = true;
       // flagUpNum++;

        
    }

    //上げれる
    void PlayPlayer()
    {
        //笛鳴らす


        //isInput = true;
        //isFirst = true;
    }
}
