using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllParentRotate : MonoBehaviour
{
    [SerializeField] private int playerNum = 1;
    [SerializeField] private float speed = 1.0f;

    private Vector3 power = new Vector3(0.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool isLB = Input.GetButton("LBbutton" + playerNum);
        bool isRB = Input.GetButton("RBbutton" + playerNum);

        //—Í‚ª‰Á‚¦‚ç‚ê‚Ä‚È‚¢‚Ì‚È‚çŒ¸‘¬‚·‚é
        if (!Input.GetButton("LBbutton" + playerNum) && !Input.GetButton("RBbutton" + playerNum)) 
            power *= 0.997f;
        else if(isLB)
        {
            power += new Vector3(0, speed * Time.deltaTime, 0);
            power.y = Math.Min(0.03f, Math.Abs(power.y)) * Math.Sign(power.y);
        }
        else
        {
            power -= new Vector3(0, speed * Time.deltaTime, 0);
            power.y = Math.Min(0.03f, Math.Abs(power.y)) * Math.Sign(power.y);
        }

        transform.eulerAngles += power;
        
    }
}
