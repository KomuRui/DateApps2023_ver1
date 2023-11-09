using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerHand : MonoBehaviour
{
    [SerializeField] private int playerNum;                   // ƒvƒŒƒCƒ„[”Ô†
    public GameObject redOb;
    public GameObject whiteOb;

    bool isLeftUp;
    bool isRightUp;

    // Start is called before the first frame update
    void Start()
    {
        isLeftUp = false;
        isRightUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("LBbutton" + playerNum))
        {
            isLeftUp = true;
            redOb.transform.DORotate(Vector3.forward * 0f, 0.1f);
        }
        else if (Input.GetButtonDown("RBbutton" + playerNum))
        {
            isRightUp = true;
            whiteOb.transform.DORotate(Vector3.forward * 90f, 0.1f);
        }
        
        //if(isLeftUp == true)
        //{
        //    if (redOb.transform.localEulerAngles.z >= 0)
        //    {
        //        redOb.transform.Rotate(0, 0, -10);
        //    }
        //    else
        //    {
        //        isLeftUp = false; 
        //    }
        //    //redOb.transform.eulerAngles = (new Vector3(0, 0, 0));
        //}

        //if (isLeftUp == true)
        //{
        //    if (redOb.transform.localEulerAngles.z >= 0)
        //    {
        //        redOb.transform.Rotate(0, 0, -10);
        //    }
        //    else
        //    {
        //        isLeftUp = false;
        //    }
        //    //redOb.transform.eulerAngles = (new Vector3(0, 0, 0));
        //}
    }
}
