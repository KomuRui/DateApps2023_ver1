using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllParentRotate : MonoBehaviour
{
    [SerializeField] private int playerNum = 1;
    [SerializeField] private float speed;

    private Vector3 power = new Vector3(0.0f, 0.0f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //âÒì]
        Rotation();
    }

    //âÒì]
    private void Rotation()
    {
        //äJénÇµÇƒÇ¢Ç»Ç¢Ç©èIÇÌÇ¡ÇƒÇ¢ÇÈÇÃÇ»ÇÁ
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;


        bool isLB = false;
        bool isRB = false;
        if (Input.GetAxis("L_Stick_H" + playerNum) < -0.8f) isLB = true;
        if (Input.GetAxis("L_Stick_H" + playerNum) > 0.8f) isRB = true;

        //óÕÇ™â¡Ç¶ÇÁÇÍÇƒÇ»Ç¢ÇÃÇ»ÇÁå∏ë¨Ç∑ÇÈ
        if (!isRB && !isLB)
            power *= 0.997f;
        else if (isLB)
        {
            power -= new Vector3(0, speed * Time.deltaTime, 0);
            power.y = Math.Min(0.09f, Math.Abs(power.y)) * Math.Sign(power.y);
        }
        else
        {
            power += new Vector3(0, speed * Time.deltaTime, 0);
            power.y = Math.Min(0.09f, Math.Abs(power.y)) * Math.Sign(power.y);
        }

        transform.eulerAngles += power;
    }
}
