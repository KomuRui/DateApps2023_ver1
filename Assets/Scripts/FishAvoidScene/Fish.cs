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
    float fishAngle;
    int rnd;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        isCurve = true;
        curve = 0;
        rnd = Random.Range(1, 4);//1〜3
        player = GameObject.Find("Player" + rnd);
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
        transform.position += transform.forward * 0.01f;
        fishAngle = (Mathf.Sin(Time.time) * 45) + 180;
        transform.eulerAngles = (new Vector3(0, fishAngle, 0));
    }

    //サメ
    public void Shark()
    {
        transform.position += new Vector3(0, 0, -0.01f);
        if(player.transform.position.x < this.transform.position.x)
        {
            transform.position += new Vector3(-0.002f, 0, 0);
        }
        else
        {
            transform.position += new Vector3(0.002f, 0, 0);
        }
        this.transform.LookAt(player.transform.position);
        transform.eulerAngles = (new Vector3(0, 270, 0));
    }
}
