using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private int playerNum;                   // ÉvÉåÉCÉÑÅ[î‘çÜ
    public GameObject leftOb;
    public GameObject rightOb;

    public bool isStop;

    // Start is called before the first frame update
    void Start()
    {
        isStop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isStop == false)
        {
            if (Input.GetButtonDown("LBbutton" + playerNum))
            {
                leftOb.transform.DORotate(Vector3.forward * 0f, 0.1f);
            }
            else if (Input.GetButtonDown("RBbutton" + playerNum))
            {
                rightOb.transform.DORotate(Vector3.forward * 90f, 0.1f);
            }
        }
    }
}
