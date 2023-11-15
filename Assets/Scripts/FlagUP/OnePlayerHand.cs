using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePlayerHand : MonoBehaviour
{
    [SerializeField] private int playerNum;                   // プレイヤー番号
    public GameObject leftOb;
    public GameObject rightOb;

    public bool isMyTurn;
    public bool isInput;//1回

    private int flagUpNum;
    private int flagMax;
    private int roundNum;

    // Start is called before the first frame update
    void Start()
    {
        isMyTurn = true;
        isInput = false;
        flagUpNum = 0;
        roundNum = 0;
        flagMax = 3;
        //開始まで
        Invoke("StopPlayer",10.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMyTurn)
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

                if (isInput)
                {
                    //笛鳴らす

                    //上げれる時間
                    Invoke("StopPlayer", 5.0f);
                }
            }
            else
            {
                if(flagMax >= flagUpNum)
                {
                    //あげれない時間
                    Invoke("PlayPlayer", 5.0f);
                }
            }

        }
        
    }

    void StopPlayer()
    {
        isInput = false;

        flagUpNum++;
    }

    void PlayPlayer()
    {
        isInput = true;
    }
}
