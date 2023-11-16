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
    private int roundNum;

    // Start is called before the first frame update
    void Start()
    {
        isInput = false;
        isFirst = true;
        flagUpNum = 0;
        roundNum = 0;
        flagMax = 3;
        //開始まで
        Invoke("PlayPlayer",5.0f);
    }

    // Update is called once per frame
    void Update()
    {
            if(isInput)
            {
                if (Input.GetButtonDown("LBbutton" + playerNum))
                {
                    leftOb.transform.DORotate(Vector3.forward * 0f, 0.1f);
                }
                else if (Input.GetButtonDown("RBbutton" + playerNum))
                {
                    rightOb.transform.DORotate(Vector3.forward * 90f, 0.1f);
                }
                else if (Input.GetButtonDown("Abutton" + playerNum))
                {
                    leftOb.transform.DORotate(Vector3.forward * 90f, 0.1f);
                    rightOb.transform.DORotate(Vector3.forward * 0f, 0.1f);
                }

                //if (isInput)
                {
                    if(isFirst)
                    {
                        isFirst = false;
                        //上げれる時間
                        Invoke("StopPlayer", 5.0f);
                    }
                    
                }
            }
            else
            {
                if(flagMax > flagUpNum && isFirst)
                {
                    isFirst = false;
                    //あげれない時間
                    Invoke("PlayPlayer", 5.0f);
                }
            }
    }

    //上げれない
    void StopPlayer()
    {
        isInput = false;
        isFirst = true;
        flagUpNum++;
    }

    //上げれる
    void PlayPlayer()
    {
        //笛鳴らす


        isInput = true;
        isFirst = true;
    }
}
