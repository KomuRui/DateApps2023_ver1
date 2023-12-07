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
    float width = 0.004f;
    float speed = 0.6f;
    float height = 5f * 0.6f;
    float a = 0.6f;
    float fishAngle;
    int rnd;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        isCurve = true;
        curve = 0;

        List<GameObject> notKillPlayer = new List<GameObject>();
        if (GameObject.Find("Player2") != null) notKillPlayer.Add(GameObject.Find("Player2").transform.GetChild(0).gameObject);
        if (GameObject.Find("Player3") != null) notKillPlayer.Add(GameObject.Find("Player3").transform.GetChild(0).gameObject);
        if (GameObject.Find("Player4") != null) notKillPlayer.Add(GameObject.Find("Player4").transform.GetChild(0).gameObject);

        if (notKillPlayer.Count == 1)
            rnd = 0;
        else
            rnd = Random.Range(0, notKillPlayer.Count);//1〜3

        player = notKillPlayer[rnd];

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

        //画面外に行ったら
        if (transform.position.z < -15)
        {
        //    Destroy(this.gameObject);
            //画面外に行ったら非アクティブにする
            this.gameObject.SetActive(false);
        }
    }

    //ペンギン
    public void Penguin()
    {
        transform.position += new Vector3(0, 0, -10f * Time.deltaTime * speed);
    }

    //イルカ
    public void Dolphin()
    {
        transform.position += new Vector3(0, 0, -3.5f * Time.deltaTime * speed);
        //this.transform.DOJump(new Vector3(transform.position.x, -3, -15.0f), jumpPower: 1.5f, numJumps: 5, duration: 7f);
        transform.position += new Vector3(0, height * Time.deltaTime * speed, -3.5f * Time.deltaTime * speed);
        //2.25;
        transform.Rotate(new Vector3(100f * Time.deltaTime * speed, 0, 0));
        if (transform.position.y > 0.0f)
        {
            height = -height;
            //transform.Rotate(new Vector3(300, 0, 0));
            //transform.LookAt(new Vector3(300, 180, 0)); // (300, 0, 0)の方向を向く
            //transform.eulerAngles = (new Vector3(300, 180, 0));
        }


        //250 350;

        if (transform.position.y < -3.2f)
        {
            height = -height;
            transform.eulerAngles = (new Vector3(220, 180, 0));
        }

        //transform.eulerAngles = (new Vector3(260, 180, 0));
    }

    //魚群
    public void Fishes()
    {
        if (Mathf.Abs(this.transform.position.x) >= 4.3f)
        {
            width = -width;
        }

        if (width < 0)
        {
            transform.eulerAngles = (new Vector3(0, 135, 0));
        }
        else
        {
            transform.eulerAngles = (new Vector3(0, 225, 0));
        }

        this.transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.3f, 4.3f), this.transform.position.y, this.transform.position.z);

        this.transform.position += this.transform.forward * 5f *Time.deltaTime * speed;
    }

    //サメ
    public void Shark()
    {
        transform.position += new Vector3(0, 0, -5f * Time.deltaTime * speed);
        if (player != null && player.transform.position.z < this.transform.position.z - 5)
        {
            if (player.transform.position.x < this.transform.position.x)
            {
                transform.position += new Vector3(-1f * Time.deltaTime * speed, 0, 0);
            }
            else
            {
                transform.position += new Vector3(1f * Time.deltaTime * speed, 0, 0);
            }
            this.transform.LookAt(player.transform.position);
            transform.eulerAngles = (new Vector3(0, 270, 0));
        }
    }
}
