using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public class Fish : MonoBehaviour
{
    bool isCurve;
    float curve;
    const float WIDTH = 0.004f;
    float height = 0.02f;
    float a = 0.6f;

    // Start is called before the first frame update
    void Start()
    {
        isCurve = true;
        curve = 0;
    }

    // Update is called once per frame
    void Update()
    {
        switch(this.transform.tag)
        {
            case ("Penguin"):
                Penguin();
                break;
            case ("Dolphin"):
                    Dolphin();
                break;
            case ("Fishes"):
                Fishes();
                break;
            case ("Shark"):
                Shark();
                break;
            default:
                break;
        }

        if (transform.position.z < -15)
        {
            Destroy(this.gameObject);
        }
    }

    //ペンギン
    public void Penguin()
    {
        transform.position += new Vector3(0, 0, -0.02f);
    }

    //イルカ
    public void Dolphin()
    {
        //this.transform.DOJump(new Vector3(transform.position.x, -3, -15.0f), jumpPower: 1.5f, numJumps: 5, duration: 7f);
        transform.position += new Vector3(0, height, -0.007f);
        //2.25;
        transform.Rotate(new Vector3(0.25f, 0, 0));
        if (transform.position.y > 1.5f)
        {
            height = -height;
            //transform.Rotate(new Vector3(300, 0, 0));
            //transform.LookAt(new Vector3(300, 180, 0)); // (300, 0, 0)の方向を向く
            //transform.eulerAngles = (new Vector3(300, 180, 0));
        }


        //250 350;

        if (transform.position.y < -5.0f)
        {
            height = -height;
            transform.eulerAngles = (new Vector3(220, 180, 0));
        }
    }

    //魚群
    public void Fishes()
    {
        if(isCurve)
        {
            transform.position += new Vector3(-WIDTH, 0,0);
            curve -= WIDTH;
            if (curve <= -a)
            {
                isCurve =false;
                transform.eulerAngles = (new Vector3(0, 135, 0));
            }
        }
        else
        {
            transform.position += new Vector3(WIDTH, 0, 0);
            curve += WIDTH;
            if (curve >= a)
            {
                isCurve = true;
                transform.eulerAngles = (new Vector3(0, 225, 0));
            }
        }

        transform.position += new Vector3(0, 0, -0.01f);
        
    }

    //サメ
    public void Shark()
    {
        transform.position += new Vector3(0, 0, -0.01f);
    }
}
