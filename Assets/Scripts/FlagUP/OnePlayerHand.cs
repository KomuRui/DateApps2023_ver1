using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnePlayerHand : MonoBehaviour
{
    [SerializeField] private int playerNum;                   // ÉvÉåÉCÉÑÅ[î‘çÜ
    public GameObject leftOb;
    public GameObject rightOb;

    public bool isStop;//ìÆÇØÇ»Ç¢
    public bool isMyTurn;
    public bool isFirst;//1âÒ

    // Start is called before the first frame update
    void Start()
    {
        isStop = false;
        isMyTurn = true;
        isFirst = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMyTurn)
        {
            if (isStop == false)
            {
                if (Input.GetButtonDown("LBbutton" + playerNum))
                {
                    leftOb.transform.DORotate(Vector3.forward * 0f, 0.1f);
                }
                else if (Input.GetButtonDown("RBbutton" + playerNum))
                {
                    rightOb.transform.DORotate(Vector3.forward * 90f, 0.1f);
                }
                
                if(isFirst)
                {
                    isFirst = false;
                    Invoke("StopPlayer", 5.0f);
                }
            }
        }
        
    }

    void StopPlayer()
    {
        isStop = true;
    }
}
